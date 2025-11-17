using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UAMPass.Models;
using System.Security.Cryptography;
using System.Text;

namespace UAMPass.Controllers
{
    public class EstudiantesController : Controller
    {
        private readonly ApplicationDbContext _db;
        public EstudiantesController(ApplicationDbContext db) => _db = db;

        // MVC: Lista paginada simple
        public async Task<IActionResult> Index()
        {
            var estudiantes = await _db.Estudiantes
                .AsNoTracking()
                .OrderByDescending(e => e.FechaRegistro)
                .ToListAsync();
            return View(estudiantes);
        }

        // MVC: Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var estudiante = await _db.Estudiantes
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id.Value);
            if (estudiante == null) return NotFound();
            return View(estudiante);
        }

        // MVC: GET Create
        public IActionResult Create() => View();

        // MVC: POST Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Estudiante estudiante)
        {
            if (!ModelState.IsValid) return View(estudiante);

            // Normalizar carreras
            if (estudiante.Carreras != null)
            {
                estudiante.Carreras = estudiante.Carreras
                    .Select(c => c.Trim())
                    .Where(c => !string.IsNullOrWhiteSpace(c))
                    .Distinct()
                    .ToList();
            }

            // CORREGIDO: Hash de contraseña usando ContrasenaPlano
            if (!string.IsNullOrWhiteSpace(estudiante.ContrasenaPlano))
            {
                estudiante.ContrasenaHash = HashPassword(estudiante.ContrasenaPlano);
            }

            _db.Add(estudiante);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // MVC: GET Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var estudiante = await _db.Estudiantes.FindAsync(id.Value);
            if (estudiante == null) return NotFound();
            return View(estudiante);
        }

        // MVC: POST Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Estudiante estudiante)
        {
            if (id != estudiante.Id) return BadRequest();
            if (!ModelState.IsValid) return View(estudiante);

            var original = await _db.Estudiantes.FindAsync(id);
            if (original == null) return NotFound();

            // Mantener campos existentes
            original.Nombre = estudiante.Nombre;
            original.Correo = estudiante.Correo;
            original.Facultad = estudiante.Facultad;
            original.CIF = estudiante.CIF;

            // Normalizar carreras
            if (estudiante.Carreras != null)
            {
                original.Carreras = estudiante.Carreras
                    .Select(c => c.Trim())
                    .Where(c => !string.IsNullOrWhiteSpace(c))
                    .Distinct()
                    .ToList();
            }

            // CORREGIDO: Hash solo si viene una nueva ContrasenaPlano
            if (!string.IsNullOrWhiteSpace(estudiante.ContrasenaPlano))
            {
                original.ContrasenaHash = HashPassword(estudiante.ContrasenaPlano);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // MVC: GET Delete confirmation
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var estudiante = await _db.Estudiantes.FirstOrDefaultAsync(e => e.Id == id.Value);
            if (estudiante == null) return NotFound();
            return View(estudiante);
        }

        // MVC: POST Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estudiante = await _db.Estudiantes.FindAsync(id);
            if (estudiante != null)
            {
                _db.Estudiantes.Remove(estudiante);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // -------------------------
        // JSON/API endpoints
        // -------------------------

        [HttpGet("/api/estudiantes")]
        public async Task<IActionResult> GetAllJson()
        {
            var list = await _db.Estudiantes
                .AsNoTracking()
                .Select(e => new {
                    e.Id,
                    e.Nombre,
                    e.Correo,
                    e.Facultad,
                    Carreras = e.CareersCsv
                })
                .ToListAsync();
            return Json(list);
        }

        [HttpGet("/api/estudiantes/{id:int}")]
        public async Task<IActionResult> GetJson(int id)
        {
            var e = await _db.Estudiantes.AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new {
                    x.Id,
                    x.Nombre,
                    x.Correo,
                    x.Facultad,
                    Carreras = x.CareersCsv
                })
                .FirstOrDefaultAsync();
            if (e == null) return NotFound();
            return Json(e);
        }

        [HttpPost("/api/estudiantes")]
        public async Task<IActionResult> CreateJson([FromBody] Estudiante estudiante)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _db.Estudiantes.Add(estudiante);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetJson), new { id = estudiante.Id }, estudiante);
        }

        // MÉTODO EXTRA: Hash de contraseñas
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}

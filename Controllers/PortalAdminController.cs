using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using UAMPass.Models;

namespace UAMPass.Controllers.Admin
{
    public class AdministradoresController : Controller
    {
        private readonly ApplicationDbContext _db;
        public AdministradoresController(ApplicationDbContext db) => _db = db;

        private const string VIEW_PATH = "~/Views/PortalAdmin/Administradores/";

        // MVC: Lista paginada simple
        public async Task<IActionResult> Index()
        {
            var administradores = await _db.Administradores
                .AsNoTracking()
                .OrderByDescending(a => a.Id) // Usamos Id ya que no hay FechaRegistro en el modelo simple
                .ToListAsync();

            return View(VIEW_PATH + "Index.cshtml", administradores);
        }

        // MVC: Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var administrador = await _db.Administradores
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id.Value);

            if (administrador == null) return NotFound();

            return View(VIEW_PATH + "Details.cshtml", administrador);
        }

        // MVC: GET Create
        public IActionResult Create() =>
            View(VIEW_PATH + "Create.cshtml");

        // MVC: POST Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Administrador administrador)
        {
            // Nota: El modelo Administrador es más simple, no tiene todas las validaciones de Estudiante.
            if (!ModelState.IsValid)
                return View(VIEW_PATH + "Create.cshtml", administrador);

            // Hash contraseña
            if (!string.IsNullOrWhiteSpace(administrador.ContrasenaPlano))
            {
                administrador.ContrasenaHash = HashPassword(administrador.ContrasenaPlano);
            }
            else
            {
                ModelState.AddModelError("ContrasenaPlano", "La contraseña es obligatoria.");
                return View(VIEW_PATH + "Create.cshtml", administrador);
            }

            _db.Add(administrador);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // MVC: GET Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var administrador = await _db.Administradores.FindAsync(id.Value);
            if (administrador == null) return NotFound();

            // Limpiamos el hash para que el formulario se pueda cargar sin exponer la contraseña
            administrador.ContrasenaHash = string.Empty;

            return View(VIEW_PATH + "Edit.cshtml", administrador);
        }

        // MVC: POST Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Administrador administrador)
        {
            if (id != administrador.Id) return BadRequest();
            if (!ModelState.IsValid)
                return View(VIEW_PATH + "Edit.cshtml", administrador);

            var original = await _db.Administradores.FindAsync(id);
            if (original == null) return NotFound();

            original.Usuario = administrador.Usuario;

            // Solo actualizar el hash si se proporciona una nueva ContraseñaPlano
            if (!string.IsNullOrWhiteSpace(administrador.ContrasenaPlano))
            {
                original.ContrasenaHash = HashPassword(administrador.ContrasenaPlano);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // MVC: GET Delete confirmation
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var administrador = await _db.Administradores.FirstOrDefaultAsync(a => a.Id == id.Value);
            if (administrador == null) return NotFound();

            return View(VIEW_PATH + "Delete.cshtml", administrador);
        }

        // MVC: POST Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var administrador = await _db.Administradores.FindAsync(id);
            if (administrador != null)
            {
                _db.Administradores.Remove(administrador);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // JSON API endpoints
        [HttpGet("/api/administradores")]
        public async Task<IActionResult> GetAllJson()
        {
            var list = await _db.Administradores
                .AsNoTracking()
                .Select(a => new
                {
                    a.Id,
                    a.Usuario
                })
                .ToListAsync();
            return Json(list);
        }

        [HttpGet("/api/administradores/{id:int}")]
        public async Task<IActionResult> GetJson(int id)
        {
            var a = await _db.Administradores.AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    x.Usuario
                })
                .FirstOrDefaultAsync();
            if (a == null) return NotFound();
            return Json(a);
        }

        // API POST Create
        [HttpPost("/api/administradores")]
        public async Task<IActionResult> CreateJson([FromBody] Administrador administrador)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Hash contraseña
            if (!string.IsNullOrWhiteSpace(administrador.ContrasenaPlano))
            {
                administrador.ContrasenaHash = HashPassword(administrador.ContrasenaPlano);
            }
            else if (string.IsNullOrWhiteSpace(administrador.ContrasenaHash))
            {
                return BadRequest("Se requiere contraseña.");
            }

            _db.Administradores.Add(administrador);
            await _db.SaveChangesAsync();
            return Json(administrador);
        }

        // Utilidad para hashear contraseña 
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password ?? string.Empty));
            return Convert.ToBase64String(bytes);
        }
        public async Task<IActionResult> Profile()
        {
            // Obtener el ID del administrador desde la sesión
            var idString = HttpContext.Session.GetString("AdminId");
            if (string.IsNullOrEmpty(idString) || !int.TryParse(idString, out int id))
                return RedirectToAction("Login", "AdminAuth"); // Redirige al login si no hay sesión

            // Buscar el administrador en la base de datos
            var admin = await _db.Administradores.AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);

            if (admin == null)
                return RedirectToAction("Login", "AdminAuth"); // Redirige al login si no existe

            // Devuelve la vista con los datos del administrador
            return View("~/Views/PortalAdmin/Profile.cshtml", admin);

        }

    }
}
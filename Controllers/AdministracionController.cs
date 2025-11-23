using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UAMPass.Models;

namespace UAMPass.Controllers
{
    public class AdministracionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdministracionController(ApplicationDbContext context)
        {
            _context = context;
        }
        /*============================
         inicio de sesion administrador
        ================================*/


        // =============================
        // INDEX PRINCIPAL DEL PORTAL
        // =============================
        public IActionResult Index()
        {
            return View(); // → buscará /Views/Administracion/Index.cshtml
        }

        // =============================
        // LISTA DE ESTUDIANTES
        // =============================
        public async Task<IActionResult> Estudiantes()
        {
            var lista = await _context.Estudiantes.ToListAsync();
            return View("Estudiantes/Index", lista);
        }

        // =============================
        // DETALLES
        // =============================
        public async Task<IActionResult> Details(int id)
        {
            var estudiante = await _context.Estudiantes.FindAsync(id);

            if (estudiante == null)
                return NotFound();

            return View("Estudiantes/Details", estudiante);
        }

        // =============================
        // CREAR
        // =============================
        public IActionResult Create()
        {
            return View("Estudiantes/Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Estudiante estudiante)
        {
            if (ModelState.IsValid)
            {
                _context.Add(estudiante);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Estudiantes));
            }

            return View("Estudiantes/Create", estudiante);
        }

        // =============================
        // EDITAR
        // =============================
        public async Task<IActionResult> Edit(int id)
        {
            var estudiante = await _context.Estudiantes.FindAsync(id);

            if (estudiante == null)
                return NotFound();

            return View("Estudiantes/Edit", estudiante);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Estudiante estudiante)
        {
            if (id != estudiante.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(estudiante);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Estudiantes));
            }

            return View("Estudiantes/Edit", estudiante);
        }

        // =============================
        // ELIMINAR
        // =============================
        public async Task<IActionResult> Delete(int id)
        {
            var estudiante = await _context.Estudiantes.FindAsync(id);

            if (estudiante == null)
                return NotFound();

            return View("Estudiantes/Delete", estudiante);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estudiante = await _context.Estudiantes.FindAsync(id);

            if (estudiante != null)
            {
                _context.Estudiantes.Remove(estudiante);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Estudiantes));
        }
    }
}

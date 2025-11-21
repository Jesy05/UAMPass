using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UAMPass.Models;
using System.Threading.Tasks;

namespace UAMPass.Controllers
{
    public class PortalEstudianteController : Controller
    {
        private readonly ApplicationDbContext _db;

        public PortalEstudianteController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Helper consistente para la sesión
        private int? GetEstudianteIdFromSession()
        {
            var idStr = HttpContext.Session.GetString("EstudianteId");
            if (string.IsNullOrWhiteSpace(idStr)) return null;
            if (int.TryParse(idStr, out var id)) return id;
            return null;
        }

        private bool IsLoggedIn() => GetEstudianteIdFromSession().HasValue;

        // GET: /PortalEstudiante/Profile
        public async Task<IActionResult> Profile()
        {
            var id = GetEstudianteIdFromSession();
            if (!id.HasValue)
                return RedirectToAction("Login", "EstudianteAuth");

            var estudiante = await _db.Estudiantes
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id.Value);

            if (estudiante == null)
            {
                // sesión inválida, limpiar y redirigir
                HttpContext.Session.Remove("EstudianteId");
                HttpContext.Session.Remove("EstudianteCorreo");
                HttpContext.Session.Remove("EstudianteNombre");
                return RedirectToAction("Login", "EstudianteAuth");
            }

            // pasar modelo a la vista (evita NullReference en ViewData/ViewModel)
            ViewData["Title"] = "Mi Perfil";
            return View(estudiante);
        }

        // GET: /PortalEstudiante/Home  (opcional)
        public async Task<IActionResult> Home()
        {
            var id = GetEstudianteIdFromSession();
            if (!id.HasValue)
                return RedirectToAction("Login", "EstudianteAuth");

            // Puedes obtener datos para la home (notificaciones, aplicaciones, etc.)
            var estudiante = await _db.Estudiantes
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id.Value);

            if (estudiante == null)
                return RedirectToAction("Login", "EstudianteAuth");

            ViewData["Title"] = "Inicio - Portal Estudiante";
            return View(estudiante); // si la home requiere modelo, o return View() si no
        }
    }
}

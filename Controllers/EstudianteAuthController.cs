using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using UAMPass.Models;
using UAMPass.Models.DTOs;
using System.Threading.Tasks;

namespace UAMPass.Controllers
{
    public class EstudianteAuthController : Controller
    {
        private readonly ApplicationDbContext _db;

        public EstudianteAuthController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: /EstudianteAuth/Landing
        [HttpGet]
        public IActionResult Landing()
        {
            return View();
        }

        // GET: /EstudianteAuth/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginEstudianteDTO());
        }

        // POST: /EstudianteAuth/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginEstudianteDTO dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            // Buscar estudiante por correo
            var estudiante = await _db.Estudiantes
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Correo == dto.Correo);

            if (estudiante == null)
            {
                ViewData["Error"] = "Correo o contraseña incorrectos.";
                return View(dto);
            }

            var hashIngresado = Hash(dto.Contrasena);

            if (estudiante.ContrasenaHash != hashIngresado)
            {
                ViewData["Error"] = "Correo o contraseña incorrectos.";
                return View(dto);
            }

            // Login exitoso: guardar datos en sesión usando UNA sola key consistente
            HttpContext.Session.SetString("EstudianteId", estudiante.Id.ToString());
            HttpContext.Session.SetString("EstudianteCorreo", estudiante.Correo);
            HttpContext.Session.SetString("EstudianteNombre", estudiante.Nombre);

            // Actualizar último login (opcional)
            estudiante.UltimoLogin = System.DateTime.UtcNow;
            _db.Update(estudiante);
            await _db.SaveChangesAsync();

            // Redirigir al perfil del estudiante
            return RedirectToAction("Profile", "PortalEstudiante");
        }

        // GET: /EstudianteAuth/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /EstudianteAuth/Register
        [HttpPost]
        public async Task<IActionResult> Register(Estudiante estudiante, string password)
        {
            if (!ModelState.IsValid)
                return View(estudiante);

            // Validaciones mínimas
            if (string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("Contrasena", "La contraseña es obligatoria.");
                return View(estudiante);
            }

            // Evitar duplicados por correo
            var exists = await _db.Estudiantes.AnyAsync(e => e.Correo == estudiante.Correo);
            if (exists)
            {
                ModelState.AddModelError("Correo", "Ya existe una cuenta con ese correo.");
                return View(estudiante);
            }

            estudiante.ContrasenaHash = Hash(password);
            estudiante.FechaRegistro = System.DateTime.UtcNow;

            _db.Estudiantes.Add(estudiante);
            await _db.SaveChangesAsync();

            // Opcional: iniciar sesión inmediatamente
            HttpContext.Session.SetString("EstudianteId", estudiante.Id.ToString());
            HttpContext.Session.SetString("EstudianteCorreo", estudiante.Correo);
            HttpContext.Session.SetString("EstudianteNombre", estudiante.Nombre);

            return RedirectToAction("Profile", "PortalEstudiante");
        }

        // GET: /EstudianteAuth/ForgotPassword
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // POST: /EstudianteAuth/ForgotPassword
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string correo)
        {
            if (string.IsNullOrWhiteSpace(correo))
            {
                ViewBag.Mensaje = "Ingresa tu correo.";
                return View();
            }

            var estudiante = await _db.Estudiantes
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Correo == correo);

            if (estudiante == null)
            {
                ViewBag.Mensaje = "No se encontró una cuenta con ese correo.";
                return View();
            }

            // Demo: en producción enviar mail; aquí mostrar instrucción
            ViewBag.Mensaje = "Para recuperar la contraseña contacta a administración. (Modo demo)";
            return View();
        }

        // Logout
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("EstudianteId");
            HttpContext.Session.Remove("EstudianteCorreo");
            HttpContext.Session.Remove("EstudianteNombre");
            return RedirectToAction("Login");
        }

        // Utilidad para hashear contraseña
        private string Hash(string input)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input ?? string.Empty));
            return Convert.ToBase64String(bytes);
        }
    }
}

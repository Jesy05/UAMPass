using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using UAMPass.Models;
using System;
using System.Threading.Tasks;

namespace UAMPass.Controllers.Admin
{
    public class AdminAuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminAuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /AdminAuth/Landing (Muestra la página de opciones: Iniciar Sesión / Registrarse)
        public IActionResult Landing()
        {
            // Apunta a la vista Landing.cshtml que contiene los dos botones
            return View();
        }

        // GET: /AdminAuth/Login (Muestra el formulario de Login)
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /AdminAuth/Login (Procesa el Login)
        [HttpPost]
        public async Task<IActionResult> Login(string usuario, string contrasena)
        {
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(contrasena))
            {
                ViewBag.Error = "Debe ingresar usuario y contraseña.";
                return View();
            }

            var admin = await _context.Administradores
                .FirstOrDefaultAsync(a => a.Usuario == usuario);

            if (admin == null)
            {
                ViewBag.Error = "Credenciales incorrectas.";
                return View();
            }

            var hashIngresado = Hash(contrasena);

            // Bloque donde verifica si la contraseña es incorrecta
            if (admin.ContrasenaHash != hashIngresado)
            {
                ViewBag.Error = "Credenciales incorrectas.";
                return View();
            }

            // 🛑 LÓGICA DE REDIRECCIÓN EXITOSA
            // Login exitoso: guardar datos en sesión
            HttpContext.Session.SetString("AdminId", admin.Id.ToString());
            HttpContext.Session.SetString("AdminUser", admin.Usuario);

            // Redirige al método Index del controlador Administradores (la lista de estudiantes)
            return RedirectToAction("Index", "Administradores");
        }

        // GET: /AdminAuth/Registro
        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        // POST: /AdminAuth/Registro 
        [HttpPost]
        public IActionResult Registro(string usuario, string contrasena)
        {
            // Redirige al Login o a donde necesites después de un intento de registro
            return RedirectToAction("Login");
        }

        // GET: /AdminAuth/ForgotPassword (Recuperación)
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            ViewBag.Mensaje = "Contacte a administración para recuperación.";
            return View();
        }

        // POST: /AdminAuth/ForgotPassword
        [HttpPost]
        public IActionResult ForgotPassword(string correo)
        {
            ViewBag.Mensaje = "Contacte a administración para recuperación. (Modo demo)";
            return View();
        }

        // Logout
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // UTILIDAD PARA HASHEAR CONTRASEÑA
        private string Hash(string input)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input ?? string.Empty));
            return Convert.ToBase64String(bytes);
        }
    }
}
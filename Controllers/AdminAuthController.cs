using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using UAMPass.Models;
using UAMPass.Models.Dto;
using Microsoft.AspNetCore.Http;

namespace UAMPass.Controllers
{
    public class AdminAuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminAuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /AdminAuth/Landing
        public IActionResult Landing()
        {
            return View();
        }

        // GET: /AdminAuth/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginAdministradorDTO());
        }

        // POST: /AdminAuth/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginAdministradorDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 1. Buscar admin por usuario (case-insensitive)
            var admin = await _context.Administradores
                .FirstOrDefaultAsync(a => a.Usuario.ToLower() == model.Usuario.ToLower());

            if (admin == null)
            {
                ViewBag.Error = "Credenciales incorrectas.";
                return View(model);
            }

            // 2. Verificar contraseña hasheada
            var hashIngresado = HashPassword(model.Contrasena);

            if (admin.ContrasenaHash != hashIngresado)
            {
                ViewBag.Error = "Credenciales incorrectas.";
                return View(model);
            }

            // 3. Guardar sesión de admin
            HttpContext.Session.SetString("AdminId", admin.Id.ToString());
            HttpContext.Session.SetString("AdminUser", admin.Usuario);
            HttpContext.Session.SetString("AdminNombre", admin.Nombre);

            // 4. Ir al perfil del admin
            return RedirectToAction("Profile", "Administradores");
        }

        // GET: /AdminAuth/Registro
        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        // POST: /AdminAuth/Registro
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(Administrador model)
        {
            // Evitar error por ContrasenaHash vacío (lo calculamos aquí)
            ModelState.Remove("ContrasenaHash");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Usuario duplicado
            if (await _context.Administradores.AnyAsync(a => a.Usuario.ToLower() == model.Usuario.ToLower()))
            {
                ModelState.AddModelError("Usuario", "Este usuario ya está en uso.");
                return View(model);
            }

            // Correo duplicado
            if (await _context.Administradores.AnyAsync(a => a.Correo.ToLower() == model.Correo.ToLower()))
            {
                ModelState.AddModelError("Correo", "Este correo ya está registrado.");
                return View(model);
            }

            // Hashear contraseña y guardar
            model.ContrasenaHash = HashPassword(model.ContrasenaPlano);

            _context.Add(model);
            await _context.SaveChangesAsync();

            HttpContext.Session.SetString("AdminId", model.Id.ToString());
            HttpContext.Session.SetString("AdminUser", model.Usuario);
            HttpContext.Session.SetString("AdminNombre", model.Nombre);

            return RedirectToAction("Profile", "Administradores");
        }

        // POST: /AdminAuth/Logout
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // GET: /AdminAuth/ForgotPassword
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            // Para que la vista sepa que es flujo de ADMIN
            ViewBag.EsAdmin = true;
            return View();
        }

        // POST: /AdminAuth/ForgotPassword
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string correo)
        {
            // Seguimos marcando que es flujo admin
            ViewBag.EsAdmin = true;

            // Buscamos por correo porque en la vista pides correo electrónico
            var admin = await _context.Administradores
                .FirstOrDefaultAsync(a => a.Correo == correo);

            if (admin == null)
            {
                ViewBag.Mensaje = "No existe un administrador con ese correo.";
                return View();
            }

            ViewBag.Mensaje = "Contacta a soporte técnico para restablecer tu contraseña. (Modo Demo)";
            return View();
        }

        // Utilidad Hash (SHA256)
        private string HashPassword(string input)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input ?? string.Empty));
            return Convert.ToBase64String(bytes);
        }
    }
}

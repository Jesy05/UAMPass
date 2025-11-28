using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using UAMPass.Models;
using System;
using System.Threading.Tasks;
using UAMPass.Models.Dto; 

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
        //  Recibe el DTO (que contiene las reglas [Required])
        public async Task<IActionResult> Login(LoginAdministradorDTO model)
        {
            // Si el ModelState NO es válido (campos vacíos), regresa la vista para mostrar los errores del DTO.
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //  Usar model.Usuario en lugar de la variable 'usuario'
            var admin = await _context.Administradores
                .FirstOrDefaultAsync(a => a.Usuario == model.Usuario);

            if (admin == null)
            {
                ViewBag.Error = "Credenciales incorrectas.";
                return View(model); // Retorna el modelo para que la vista recargue los campos
            }

            //  Usar model.Contrasena en lugar de la variable 'contrasena'
            var hashIngresado = Hash(model.Contrasena);

            // Bloque donde verifica si la contraseña es incorrecta
            if (admin.ContrasenaHash != hashIngresado)
            {
                ViewBag.Error = "Credenciales incorrectas.";
                return View(model); // Retorna el modelo
            }

            // LÓGICA DE REDIRECCIÓN EXITOSA
            HttpContext.Session.SetString("AdminId", admin.Id.ToString());
            HttpContext.Session.SetString("AdminUser", admin.Usuario);


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
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(string usuario)
        {
            // buscar admin por usuario
            var admin = _context.Administradores.FirstOrDefault(a => a.Usuario == usuario);

            if (admin == null)
            {
                ViewBag.Mensaje = "No existe un administrador con ese usuario.";
                return View();
            }

            // modo demo: sin enviar correo aún
            ViewBag.Mensaje = "Contacta a soporte para restablecer tu contraseña. (Modo demo)";
            return View();
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
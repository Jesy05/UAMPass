using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;        // NECESARIO para consultas LINQ
using System.Net;         // NECESARIO para NetworkCredential
using System.Net.Mail;     // NECESARIO para MailMessage
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UAMPass.Models;
using UAMPass.Models.Dto; // Asegúrate de que LoginEstudianteDTO esté aquí

// Si las clases ResetPasswordViewModel y Estudiante.ResetToken/TokenExpiration no existen, el código fallará al compilar.

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

            // Login exitoso: guardar datos en sesión
            HttpContext.Session.SetString("EstudianteId", estudiante.Id.ToString());
            HttpContext.Session.SetString("EstudianteCorreo", estudiante.Correo);
            HttpContext.Session.SetString("EstudianteNombre", estudiante.Nombre);

            // Actualizar último login (opcional)

            estudiante.UltimoLogin = System.DateTime.UtcNow;
            _db.Update(estudiante);
            await _db.SaveChangesAsync();

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

            if (string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("Contrasena", "La contraseña es obligatoria.");
                return View(estudiante);
            }

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

        // POST: /EstudianteAuth/ForgotPassword (Funcionalidad de Envío de Correo)
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string correo)
        {
            if (string.IsNullOrWhiteSpace(correo))
            {
                ViewBag.Mensaje = "Ingresa tu correo.";
                return View();
            }

            var estudiante = await _db.Estudiantes
                .FirstOrDefaultAsync(e => e.Correo == correo);

            if (estudiante == null)
            {
                ViewBag.Mensaje = "No se encontró una cuenta relacionada con el correo.";
                return View();
            }

            var token = Guid.NewGuid().ToString();
            // Estas propiedades (ResetToken y TokenExpiration) deben existir en el modelo Estudiante.
            // Si tu modelo Estudiante no tiene estas propiedades, debes agregarlas:
            estudiante.ResetToken = token;
            estudiante.TokenExpiration = DateTime.Now.AddHours(1);
            await _db.SaveChangesAsync();

            // Construccion del enlace del reseteo
            var resetLink = Url.Action("ResetPassword", "EstudianteAuth",
                new { token = token }, Request.Scheme);

            // Envio del correo (Requiere credenciales SMTP correctas)
            var mail = new MailMessage();
            mail.To.Add(estudiante.Correo);
            mail.From = new MailAddress("UAMPass@uamv.edu.ni");
            mail.Subject = "Recuperación de contraseña UAMPass";
            mail.Body = $"Hola {estudiante.Nombre},\n\n" +
                $"Haz clic en el siguiente enlace para restablecer tu contraseña:\n{resetLink}\n\n" +
                "Si no solicitaste este cambio, ignora este correo.\n\n" +
                "Saludos,\nEquipo UAMPass";
            mail.IsBodyHtml = true;

            using (var smtp = new SmtpClient("smtp.uamv.edu.ni", 587))
            {
                // Debes descomentar y reemplazar con tus credenciales SMTP reales para que funcione
                // smtp.Credentials = new NetworkCredential("usuarioSMTP", "claveSMTP"); 
                smtp.EnableSsl = true;
                // await smtp.SendMailAsync(mail); // Descomentar para enviar el correo real
            }
            ViewBag.Mensaje = "Se ha enviado un enlace de restablecimiento de contraseña a tu correo.";

            return View();
        }

        // GET: /EstudianteAuth/ResetPassword
        [HttpGet]
        public IActionResult ResetPassword(string token)
        {
            // ResetPasswordViewModel debe existir
            var estudiante = _db.Estudiantes.FirstOrDefault(e => e.ResetToken == token && e.TokenExpiration > DateTime.Now);
            if (estudiante == null)
            {
                return BadRequest("Token inválido o expirado.");
            }
            // return View(new ResetPasswordViewModel { Token = token }); // Descomentar si usas el ViewModel
            return View();
        }

        // POST: /EstudianteAuth/ResetPassword
        [HttpPost]
        public async Task<IActionResult> ResetPassword(string token, string NewPassword) // Adaptado a modelo simple
        {
            var estudiante = await _db.Estudiantes.FirstOrDefaultAsync(e => e.ResetToken == token && e.TokenExpiration > DateTime.Now);
            if (estudiante == null)
            {
                return BadRequest("Token inválido o expirado.");
            }

            // Guardar nueva contraseña (con hash)
            estudiante.ContrasenaHash = Hash(NewPassword);
            estudiante.ResetToken = null; // invalidar token
            estudiante.TokenExpiration = null; // invalidar fecha de expiración
            await _db.SaveChangesAsync();

            return RedirectToAction("Login");
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

using Microsoft.AspNetCore.Mvc;
using UAMPass.Models;

namespace UAMPass.Controllers
{
    public class EstudianteAuthController : Controller
    {
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
            return View();
        }

        // POST: /EstudianteAuth/Login
        [HttpPost]
        public IActionResult Login(string correo, string password)
        {
            // TODO: buscar estudiante en la BD
            // TODO: comparar hash de contraseña
            // TODO: crear sesión

            // Por ahora simulamos login exitoso
            HttpContext.Session.SetString("Estudiante", correo);

            return RedirectToAction("Home", "PortalEstudiante");
        }

        // GET: /EstudianteAuth/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /EstudianteAuth/Register
        [HttpPost]
        public IActionResult Register(Estudiante estudiante, string password)
        {
            // TODO: generar hash y guardar estudiante

            return RedirectToAction("Login");
        }


        // GET: /EstudianteAuth/ForgotPassword
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // POST: /EstudianteAuth/ForgotPassword
        [HttpPost]
        public IActionResult ForgotPassword(string correo)
        {
            // TODO: buscar estudiante y mostrar la contraseña
            // Solo como demo: no se enviará correo

            ViewBag.Mensaje = "Tu contraseña es: uampass123 (modo demo)";
            return View();
        }
    }
}

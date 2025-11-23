using Microsoft.AspNetCore.Mvc;
using UAMPass.Models;
using Microsoft.EntityFrameworkCore;

namespace UAMPass.Controllers.Admin
{
    public class AdminAuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminAuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string usuario, string contrasena)
        {
            var admin = await _context.Administradores
                .FirstOrDefaultAsync(a => a.Usuario == usuario);

            if (admin == null || admin.ContrasenaHash != contrasena)
            {
                ViewBag.Error = "Credenciales incorrectas.";
                return View();
            }

            HttpContext.Session.SetString("AdminId", admin.Id.ToString());
            HttpContext.Session.SetString("AdminUser", admin.Usuario);

            return RedirectToAction("Dashboard", "PortalAdmin");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}

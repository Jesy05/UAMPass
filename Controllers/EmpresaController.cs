using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using UAMPass.Models;
using System.Threading.Tasks;
using UAMPass.Models.Dto;
using Microsoft.AspNetCore.Http;

namespace UAMPass.Controllers
{
    public class EmpresaController : Controller
    {
        private readonly ApplicationDbContext _db;
        public EmpresaController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var session = HttpContext.Session.GetString("empresaID");
            if (!string.IsNullOrEmpty(session))
                return RedirectToAction("portalEmpresa", "Empresas");
            return View();
        }

        //get: Empresa/login
        [HttpGet]
        public IActionResult Login()
        {
            // CORREGIDO: LoginEmpresaDTO (Mayúscula)
            return View(new LoginEmpresaDTO());
        }

        [HttpGet]
        public IActionResult portalEmpresa()
        {
            var session = HttpContext.Session.GetString("empresaID");
            if (string.IsNullOrEmpty(session))
                return RedirectToAction("Login", "Empresas");
            return View();
        }
        [HttpGet]
        public IActionResult pasantias()
        {
            return View();
        }
        //post: Empresa/login
        [HttpPost]
        public async Task<IActionResult> Login(LoginEmpresaDTO dto) // CORREGIDO: LoginEmpresaDTO
        {
            try
            {

                if (dto.ContactoEmail == string.Empty)
                    throw new Exception("El correo no puede estar vacío.");
                if (dto.Contrasena == string.Empty)
                    throw new Exception("La contraseña no puede estar vacía.");
                using var sha256 = SHA256.Create();
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(dto.Contrasena ?? string.Empty));
                dto.Contrasena = Convert.ToBase64String(bytes);
                // Buscar empresas por correo
                var empresa = await _db.Empresas.Where(w => w.ContactoEmail == dto.ContactoEmail && w.ContrasenaHash == dto.Contrasena
                )
                    .FirstOrDefaultAsync();

                if (empresa != null)
                {
                    // Autenticación exitosa
                    HttpContext.Session.SetString("empresaID", empresa.Id.ToString());
                    return RedirectToAction("portalEmpresa", "Empresas");
                }
                else
                {
                    // Autenticación fallida
                    ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos.");
                    return View(dto);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpPost]
        public IActionResult logout()
        {
            try
            {
                HttpContext.Session.Remove("empresaID");
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
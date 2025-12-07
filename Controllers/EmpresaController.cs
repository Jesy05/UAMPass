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
    //  "EmpresasController" (Plural) para coincidir con asp-controller="Empresas"
    public class EmpresasController : Controller
    {
        private readonly ApplicationDbContext _db;
        public EmpresasController(ApplicationDbContext db)
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

        // GET: /Empresas/Login
        [HttpGet]
        public IActionResult Login()
        {
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
            // Validar sesión también aquí por seguridad
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("empresaID")))
                return RedirectToAction("Login");

            return View();
        }

        public IActionResult Postulaciones()
        {
            return View();
        }

        // POST: /Empresas/Login

        [HttpPost]
        public async Task<IActionResult> Login(LoginEmpresaDTO dto)
        {
            try
            {
                using var sha256 = SHA256.Create();
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(dto.Contrasena ?? string.Empty));
                dto.Contrasena = Convert.ToBase64String(bytes);

                var empresa = await _db.Empresas
                    .FirstOrDefaultAsync(w => w.ContactoEmail == dto.ContactoEmail && w.ContrasenaHash == dto.Contrasena);

                if (empresa != null)
                {
                    HttpContext.Session.SetString("empresaID", empresa.Id.ToString());
                    return RedirectToAction("portalEmpresa", "Empresas");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos.");
                    return View(dto);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(dto);

            }
        }

        [HttpPost]
        public IActionResult logout()
        {
            HttpContext.Session.Remove("empresaID");
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // GET: /Empresas/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(CreateEmpresa obj)
        {
            try
            {
                if (!ModelState.IsValid) return View(obj);

                var data = await _db.Empresas.FirstOrDefaultAsync(w => w.ContactoEmail == obj.ContactoEmail);
                if (data != null)
                {
                    ModelState.AddModelError("ContactoEmail", "La empresa ya existe");
                    return View(obj);
                }

                Empresa empresa = new Empresa
                {
                    Nombre = obj.Nombre,
                    ContactoEmail = obj.ContactoEmail,
                    SitioWeb = obj.SitioWeb,
                    Direccion = obj.Direccion
                };

                using var sha256 = SHA256.Create();
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(obj.ContrasenaHash ?? string.Empty));
                empresa.ContrasenaHash = Convert.ToBase64String(bytes);

                await _db.Empresas.AddAsync(empresa);
                await _db.SaveChangesAsync();

                return RedirectToAction("Login", "Empresas");
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", ex.Message);
                return View(obj);

            }
        }

        // GET: Empresas/ForgotPassword
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string correo)
        {
            var admin = await _db.Empresas.FirstOrDefaultAsync(a => a.ContactoEmail == correo);

            if (admin == null)
            {
                ViewBag.Mensaje = "No existe una empress con ese usuario.";
                return View();
            }

            ViewBag.Mensaje = "Contacta a soporte técnico para restablecer tu contraseña. (Modo Demo)";
            return View();
        }

        [HttpGet]
        [Route("api/empresas")]
        public async Task<IActionResult> GetEmpresas()
        {
            var empresas = await _db.Empresas
                .Select(e => new { id = e.Id, nombre = e.Nombre })
                .ToListAsync();

            return Json(empresas); 
        }




    }
}
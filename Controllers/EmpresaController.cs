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

        //get: Empresa/login
        [HttpGet]
        public IActionResult Login()
        {
            return View(new loginEmpresaDTO());
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

        public IActionResult Postulaciones()
        {
            return View();
        }
        //post: Empresa/login
        [HttpPost]
        public async Task<IActionResult> Login(loginEmpresaDTO dto)
        {
            try
            {
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
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
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

        // GET: /empresa/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(createEmpresa obj)
        {
            try
            {
                Empresa empresa = new Empresa();

                var data = await _db.Empresas.Where(w => w.ContactoEmail == obj.ContactoEmail).FirstOrDefaultAsync();

                if (data != null)
                    throw new Exception("La empresa ya existe");

                empresa.Nombre = obj.Nombre;
                empresa.ContactoEmail = obj.ContactoEmail;
                empresa.SitioWeb = obj.SitioWeb;
                empresa.Direccion = obj.Direccion;
                using var sha256 = SHA256.Create();
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(obj.ContrasenaHash ?? string.Empty));
                empresa.ContrasenaHash = Convert.ToBase64String(bytes);

                await _db.Empresas.AddAsync(empresa);
                await _db.SaveChangesAsync();

                return RedirectToAction("Login", "Empresas");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpGet]
        [Route("api/empresas")]
        public async Task<IActionResult> getEmpresas()
        {
            try
            {
                var data = await _db.Empresas
                    .Select(s => new listEmpresa
                    {
                        Id = s.Id,
                        Nombre = s.Nombre
                    }).ToListAsync();

                return Ok(data);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(string usuario)
        {
            // buscar admin por usuario
            var empresa = _db.Empresas.FirstOrDefault(a => a.ContactoEmail == usuario);

            if (empresa == null)
            {
                ViewBag.Mensaje = "No existe una empresa con ese usuario.";
                return View();
            }

            // modo demo: sin enviar correo aún
            ViewBag.Mensaje = "Contacta a soporte para restablecer tu contraseña. (Modo demo)";
            return View();
        }
    }

}
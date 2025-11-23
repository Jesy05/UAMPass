using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using UAMPass.Models;
using System.Threading.Tasks;
using UAMPass.Models.Dto;

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
            return View();
        }
        [HttpGet]
        public IActionResult pasantias()
        {
            return View();
        }
        //post: Empresa/login
        [HttpPost]
        public async Task<IActionResult> Login(loginEmpresaDTO dto)
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
                    return RedirectToAction("portalEmpresa","Empresas");
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
                using var sha256 = SHA256.Create();
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(obj.ContrasenaHash ?? string.Empty));
                empresa.ContrasenaHash = Convert.ToBase64String(bytes);

                await _db.Empresas.AddAsync(empresa);
                await _db.SaveChangesAsync();

                return RedirectToAction("index","Empresas");
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

    }

}

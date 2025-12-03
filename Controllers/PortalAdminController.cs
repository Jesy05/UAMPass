using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using UAMPass.Models;
using UAMPass.Models.Dto;

namespace UAMPass.Controllers.Admin
{
    public class AdministradoresController : Controller
    {
        private readonly ApplicationDbContext _db;
        public AdministradoresController(ApplicationDbContext db) => _db = db;

        private const string VIEW_PATH = "~/Views/PortalAdmin/Administradores/";

        // MVC: Lista paginada simple
        public async Task<IActionResult> Index()
        {
            var administradores = await _db.Administradores
                .AsNoTracking()
                .OrderByDescending(a => a.Id) // Usamos Id ya que no hay FechaRegistro en el modelo simple
                .ToListAsync();

            return View(VIEW_PATH + "Index.cshtml", administradores);
        }

        // MVC: Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var administrador = await _db.Administradores
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id.Value);

            if (administrador == null) return NotFound();

            return View(VIEW_PATH + "Details.cshtml", administrador);
        }

        // MVC: GET Create
        public IActionResult Create() =>
            View(VIEW_PATH + "Create.cshtml");

        // MVC: POST Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Administrador administrador)
        {
            // Nota: El modelo Administrador es más simple, no tiene todas las validaciones de Estudiante.
            if (!ModelState.IsValid)
                return View(VIEW_PATH + "Create.cshtml", administrador);

            // Hash contraseña
            if (!string.IsNullOrWhiteSpace(administrador.ContrasenaPlano))
            {
                administrador.ContrasenaHash = HashPassword(administrador.ContrasenaPlano);
            }
            else
            {
                ModelState.AddModelError("ContrasenaPlano", "La contraseña es obligatoria.");
                return View(VIEW_PATH + "Create.cshtml", administrador);
            }

            _db.Add(administrador);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // MVC: GET Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var administrador = await _db.Administradores.FindAsync(id.Value);
            if (administrador == null) return NotFound();

            // Limpiamos el hash para que el formulario se pueda cargar sin exponer la contraseña
            administrador.ContrasenaHash = string.Empty;

            return View(VIEW_PATH + "Edit.cshtml", administrador);
        }

        // MVC: POST Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Administrador administrador)
        {
            if (id != administrador.Id) return BadRequest();
            if (!ModelState.IsValid)
                return View(VIEW_PATH + "Edit.cshtml", administrador);

            var original = await _db.Administradores.FindAsync(id);
            if (original == null) return NotFound();

            original.Usuario = administrador.Usuario;

            // Solo actualizar el hash si se proporciona una nueva ContraseñaPlano
            if (!string.IsNullOrWhiteSpace(administrador.ContrasenaPlano))
            {
                original.ContrasenaHash = HashPassword(administrador.ContrasenaPlano);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // MVC: GET Delete confirmation
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var administrador = await _db.Administradores.FirstOrDefaultAsync(a => a.Id == id.Value);
            if (administrador == null) return NotFound();

            return View(VIEW_PATH + "Delete.cshtml", administrador);
        }

        // MVC: POST Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var administrador = await _db.Administradores.FindAsync(id);
            if (administrador != null)
            {
                _db.Administradores.Remove(administrador);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // JSON API endpoints
        [HttpGet("/api/administradores")]
        public async Task<IActionResult> GetAllJson()
        {
            var list = await _db.Administradores
                .AsNoTracking()
                .Select(a => new
                {
                    a.Id,
                    a.Usuario
                })
                .ToListAsync();
            return Json(list);
        }

        [HttpGet("/api/administradores/{id:int}")]
        public async Task<IActionResult> GetJson(int id)
        {
            var a = await _db.Administradores.AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    x.Usuario
                })
                .FirstOrDefaultAsync();
            if (a == null) return NotFound();
            return Json(a);
        }

        // API POST Create
        [HttpPost("/api/administradores")]
        public async Task<IActionResult> CreateJson([FromBody] Administrador administrador)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Hash contraseña
            if (!string.IsNullOrWhiteSpace(administrador.ContrasenaPlano))
            {
                administrador.ContrasenaHash = HashPassword(administrador.ContrasenaPlano);
            }
            else if (string.IsNullOrWhiteSpace(administrador.ContrasenaHash))
            {
                return BadRequest("Se requiere contraseña.");
            }

            _db.Administradores.Add(administrador);
            await _db.SaveChangesAsync();
            return Json(administrador);
        }

        // Utilidad para hashear contraseña 
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password ?? string.Empty));
            return Convert.ToBase64String(bytes);
        }
        public async Task<IActionResult> Profile()
        {
            // Obtener el ID del administrador desde la sesión
            var idString = HttpContext.Session.GetString("AdminId");
            if (string.IsNullOrEmpty(idString) || !int.TryParse(idString, out int id))
                return RedirectToAction("Login", "AdminAuth"); // Redirige al login si no hay sesión

            // Buscar el administrador en la base de datos
            var admin = await _db.Administradores.AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);

            if (admin == null)
                return RedirectToAction("Login", "AdminAuth"); // Redirige al login si no existe

            // Devuelve la vista con los datos del administrador
            return View("~/Views/PortalAdmin/Profile.cshtml", admin);
        }

            private const string VIEW_PORTAL_PATH = "~/Views/PortalAdmin/";

        private const string VIEW_PORTAL = "~/Views/PortalAdmin/";

        // Menú principal (Bienvenido, Fabi...)
        public async Task<IActionResult> Menu()
        {
            var idString = HttpContext.Session.GetString("AdminId");
            if (string.IsNullOrEmpty(idString) || !int.TryParse(idString, out int id))
                return RedirectToAction("Login", "AdminAuth");

            var admin = await _db.Administradores.AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);

            if (admin == null)
                return RedirectToAction("Login", "AdminAuth");

            return View(VIEW_PORTAL + "Menu.cshtml", admin);
        }

        // using Microsoft.EntityFrameworkCore; ya lo tienes arriba

        public async Task<IActionResult> Estudiantes(string? search, string? letra)
        {
            var query = _db.Estudiantes.AsNoTracking();

            // Filtro por texto (nombre o correo)
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(e =>
                    e.Nombre.Contains(search) ||
                    e.Correo.Contains(search));
            }

            // Filtro por letra inicial del nombre
            if (!string.IsNullOrWhiteSpace(letra) && letra != "Todos")
            {
                letra = letra.ToUpper();
                query = query.Where(e =>
                    !string.IsNullOrEmpty(e.Nombre) &&
                    e.Nombre.ToUpper().StartsWith(letra));
            }

            var estudiantes = await query
                .OrderBy(e => e.Nombre)
                .ToListAsync();

            ViewBag.Search = search;
            ViewBag.Letra = string.IsNullOrWhiteSpace(letra) ? "Todos" : letra;

            return View("~/Views/PortalAdmin/Estudiantes.cshtml", estudiantes);
        }


        public IActionResult Contactos()
        {
            return View(VIEW_PORTAL + "Contactos.cshtml");
        }

        // 
        // --- API PARA OBTENER TODAS LAS PASANTÍAS (PARA EL ADMIN) ---
        [HttpGet("/api/admin/pasantias")]
        public async Task<IActionResult> GetTodasLasPasantias()
        {
            var pasantias = await _db.Pasantias
                .AsNoTracking()
                .Include(p => p.Empresa)
                .Select(p => new
                {
                    // 1. CORREGIDO: En la base de datos se llama 'Id', pero el frontend espera 'idPasantia'
                    IdPasantia = p.Id,

                    p.Titulo,
                    p.Descripcion,

                    // 2. CORREGIDO: Usamos el nombre técnico 'RequiredCareersCsv' pero se lo enviamos 
                    // al frontend como 'carrerasPermitidas' para que la tabla lo entienda.
                    CarrerasPermitidas = p.RequiredCareersCsv,

                    // 3. CORREGIDO: Usamos 'Nombre' porque así está en tu modelo Empresa.cs
                    NombreEmpresa = p.Empresa != null ? p.Empresa.Nombre : "Desconocida"
                })
                .ToListAsync();

            return Json(pasantias);
        }
        //

        // --- API PARA ELIMINAR UNA PASANTÍA (SOLO ADMIN) ---
        [HttpDelete("/api/admin/pasantias/{id}")]
        public async Task<IActionResult> DeletePasantiaAdmin(int id)
        {
            var pasantia = await _db.Pasantias.FindAsync(id);
            if (pasantia == null)
            {
                return NotFound(new { message = "Pasantía no encontrada" });
            }

            _db.Pasantias.Remove(pasantia);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Eliminado correctamente" });
        }

        // --- API PARA EDITAR UNA PASANTÍA (SOLO ADMIN) ---
        // Agregamos esto de una vez para que te funcione el botón de Editar también
        [HttpPut("/api/admin/pasantias/{id}")]
        public async Task<IActionResult> UpdatePasantiaAdmin(int id, [FromBody] PasantiaDto.CreatePasantia model)
        {
            var pasantia = await _db.Pasantias.FindAsync(id);
            if (pasantia == null) return NotFound();

            // Actualizamos los datos
            pasantia.Titulo = model.Titulo;
            pasantia.Descripcion = model.Descripcion;
            // FIX: Convert List<string> to CSV string
            pasantia.RequiredCareersCsv = model.RequiredCareersCsv != null
                ? string.Join(",", model.RequiredCareersCsv)
                : null;

            await _db.SaveChangesAsync();
            return Ok();
        }

        //

        public IActionResult Pasantias()
        {
            // Cambiamos "Pasantias.cshtml" por "pasantia.cshtml" (como se llama tu archivo real)
            return View(VIEW_PORTAL + "pasantia.cshtml");
        }
    }
}
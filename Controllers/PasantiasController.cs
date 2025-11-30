using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UAMPass.Models;
using UAMPass.Models.Dto;

namespace UAMPass.Controllers
{
    public class PasantiasController : Controller
    {
        private readonly ApplicationDbContext _db;
        public PasantiasController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("/api/pasantias")]
        public async Task<IActionResult> getPasantias()
        {
            try
            {
                var data = await _db.Pasantias.
                    Select(s => new PasantiaDto.ListPasantia // CORREGIDO: ListPasantia (Mayúscula)
                    {
                        // CORREGIDO: Propiedades en Mayúscula para coincidir con DTO
                        Titulo = s.Titulo,
                        Descripcion = s.Descripcion,
                        Empresa = s.EmpresaId,
                        // Nota: En el DTO le pusimos NombreEmpresa a una propiedad, pero aquí usabas 'empresa' como int.
                        // Ajusto para que compile con el DTO corregido que tiene 'NombreEmpresa' como string y 'Empresa' en la clase base.
                        NombreEmpresa = s.Empresa.Nombre,
                        CarrerasPermitidas = string.Join(", ", s.RequiredCareers)
                    }).
                    ToListAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [Route("api/pasantias")]
        public async Task<IActionResult> postPasantias([FromBody] PasantiaDto.CreatePasantia obj) // CORREGIDO: CreatePasantia
        {
            try
            {
                Pasantia pasantia = new Pasantia();

                // CORREGIDO: obj.Titulo y obj.Empresa (Mayúsculas)
                var data = await _db.Pasantias.
                    Where(w => w.Titulo == obj.Titulo && w.EmpresaId == obj.Empresa)
                    .FirstOrDefaultAsync();

                if (data != null)
                    throw new Exception("Pasantía duplicada para la misma empresa");

                pasantia.Titulo = obj.Titulo;
                pasantia.Descripcion = obj.Descripcion;
                // OJO: Aquí usas Session, asegúrate de que el nombre del string sea consistente ("empresaID" vs "EmpresaId")
                // En EmpresasController usaste "empresaID". Aquí lo corrijo para que coincida.
                var empresaIdStr = HttpContext.Session.GetString("empresaID");
                pasantia.EmpresaId = string.IsNullOrEmpty(empresaIdStr) ? 0 : Convert.ToInt32(empresaIdStr);

                pasantia.RequiredCareers = obj.RequiredCareersCsv;

                await _db.Pasantias.AddAsync(pasantia);
                await _db.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
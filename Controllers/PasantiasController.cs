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
        public async Task<IActionResult> getPasantias([FromQuery] int? IdEmpresa)
        {
            try
            {
                var data = await _db.Pasantias.
                     Where(w => w.EmpresaId == IdEmpresa).
                    Select(s => new PasantiaDto.ListPasantia // CORREGIDO: ListPasantia (Mayúscula)
                    {
                        IdPasantia = s.Id,
                        Titulo = s.Titulo,
                        Descripcion = s.Descripcion,
                        Empresa = s.EmpresaId,                        
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
                pasantia.EmpresaId = Convert.ToInt32(HttpContext.Session.GetString("empresaID"));
                pasantia.RequiredCareersCsv = string.Join(", ", obj.RequiredCareersCsv);

                await _db.Pasantias.AddAsync(pasantia);
                await _db.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

        [HttpDelete("/Eliminar/pasantias/{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            Console.WriteLine($"ID recibido: {id}");
            var pasantia = await _db.Pasantias.FindAsync(id);
            if (pasantia == null)
                return NotFound(new { success = false, mensaje = "La pasantía no existe." });

            _db.Pasantias.Remove(pasantia);
            await _db.SaveChangesAsync();


            return Ok(new { success = true, mensaje = "Pasantía eliminada correctamente." });
        }

        [HttpPut("/Editar/pasantias/{id}")]
        public IActionResult Editar(int id, [FromBody] PasantiaDto.Pasantias dto)
        {
            // Buscar la pasantía en la base de datos
            var pasantia = _db.Pasantias.Find(id);
            if (pasantia == null)
            {
                return NotFound("No se encontró la pasantía");
            }

            // Actualizar los campos
            pasantia.Titulo = dto.Titulo;
            pasantia.Descripcion = dto.Descripcion;
            pasantia.RequiredCareers = dto.RequiredCareersCsv;

            // Guardar cambios
            _db.SaveChanges();

            return Ok("Pasantía actualizada correctamente");
        }


    }
}



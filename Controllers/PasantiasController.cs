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
                    Select(s => new PasantiaDto.listPasantia
                    {
                        titulo = s.Titulo,
                        descripcion = s.Descripcion,
                        empresa = s.EmpresaId,
                        carrerasPermitidas = string.Join(", ", s.RequiredCareers)
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
        public async Task<IActionResult> postPasantias([FromBody]PasantiaDto.createPasantia obj)
        {
            try
            {
                Pasantia pasantia = new Pasantia();

                var data = await _db.Pasantias.
                    Where(w => w.Titulo == obj.titulo && w.EmpresaId == obj.empresa)
                    .FirstOrDefaultAsync();

                if (data != null)
                    throw new Exception("Pasantía duplicada para la misma empresa");

                pasantia.Titulo = obj.titulo;
                pasantia.Descripcion = obj.descripcion;
                pasantia.EmpresaId = obj.empresa;
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

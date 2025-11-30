using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UAMPass.Models;
using UAMPass.Models.Dto;
using static UAMPass.Models.Dto.AplicacionDto; // CORREGIDO: AplicacionDto

namespace UAMPass.Controllers
{
    public class PostulacionesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public PostulacionesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Route("api/postulaciones")]
        public async Task<IActionResult> getPostulaciones([FromQuery] AplicacionDto param) // CORREGIDO
        {
            try
            {
                var data = await _context.Aplicaciones.Where(w => (!param.IdEstudiante.HasValue || w.EstudianteId == param.IdEstudiante)
                && (!param.IdEmpresa.HasValue || w.Pasantia.Empresa.Id == param.IdEmpresa))
                    .Select(s => new ListAplicacion // CORREGIDO
                    {
                        Estudiante = s.Estudiante.Nombre,
                        Pasantia = s.Pasantia.Titulo,
                        FechaAplicacion = s.FechaAplicacion,
                        Estado = s.Status.ToString(),
                        Empresa = s.Pasantia.Empresa.Nombre // CORREGIDO: Empresa con mayúscula
                    }).ToListAsync();
                return Ok(data);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [Route("api/postulaciones")]
        public async Task<IActionResult> postPostulaciones([FromBody] CreateApplicationDto obj) // CORREGIDO
        {
            try
            {
                Aplicacion aplicacion = new Aplicacion();
                if (obj.EstudianteId == 0)
                    throw new Exception("El estudiante es obligatorio");
                if (obj.PasantiaId == 0)
                    throw new Exception("Debe seleccionar una propuesta de pasantía.");

                aplicacion.EstudianteId = obj.EstudianteId;
                aplicacion.PasantiaId = obj.PasantiaId;
                aplicacion.Comentarios = obj.Comentarios;
                aplicacion.Status = ApplicationStatus.InReview;

                await _context.Aplicaciones.AddAsync(aplicacion);
                await _context.SaveChangesAsync();

                return Ok(new { statusCode = 200, mensaje = "Postulación registrada con exito" });
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
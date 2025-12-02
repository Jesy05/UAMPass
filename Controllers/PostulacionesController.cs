using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UAMPass.Models;
using UAMPass.Models.Dto;
using static UAMPass.Models.Dto.AplicacionDto;

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
        public async Task<IActionResult> getPostulaciones([FromQuery] AplicacionDto param)
        {
            try
            {
                var data = await _context.Aplicaciones.Where(w => (!param.IdEstudiante.HasValue || w.EstudianteId == param.IdEstudiante)
                && (!param.IdEmpresa.HasValue || w.Pasantia.Empresa.Id == param.IdEmpresa))
                    .Select(s => new ListAplicacion
                    {
                        Estudiante = s.Estudiante.Nombre,
                        Pasantia = s.Pasantia.Titulo,
                        FechaAplicacion = s.FechaAplicacion,
                        Estado = s.Status.ToString(),
                        Empresa = s.Pasantia.Empresa.Nombre
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
        public async Task<IActionResult> postPostulaciones([FromBody] CreateApplication obj)
        {
            try
            {
                Aplicacion aplicacion = new Aplicacion();
                if (obj.EstudianteId == 0 || obj.EstudianteId == 0)
                    throw new Exception("El estudiante es obligatorio");
                if (obj.PasantiaId == 0 || obj.PasantiaId == 0)
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



        [HttpGet]
        public async Task<IActionResult> getPostulaciones()
        {

            try
            {
                var empresaId = Convert.ToInt32(HttpContext.Session.GetString("empresaID"));
                if (empresaId == 0)
                {
                    return Unauthorized(new { success = false, mensaje = "No se encontró la empresa en la sesión." });
                }
                var data = await _context.Aplicaciones
                    .Where(p => p.Pasantia.EmpresaId == empresaId)
                    .Select(p => new
                    {
                        id = p.Id,
                        Estudiante = p.Estudiante.Nombre,
                        cv = p.Estudiante.CvPdfPath,
                        Pasantia = p.Pasantia.Titulo,
                        FechaAplicacion = p.FechaAplicacion.ToString("dd/MM/yyyy"),
                        Estado = p.Status.ToString(),
                    }).
                        ToListAsync();
                return Ok(new { data });

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> CambiarEstado([FromBody] EstadoDto obj)
        {
            var aplicacion = await _context.Aplicaciones.FirstOrDefaultAsync(a => a.Id == obj.Id);
            if (aplicacion == null)
                return NotFound(new { success = false, mensaje = "La postulación no existe." });

            if (!Enum.TryParse<ApplicationStatus>(obj.Estado, out var nuevoEstado))
                return BadRequest(new { success = false, mensaje = "Estado inválido." });

            aplicacion.Status = nuevoEstado;
            await _context.SaveChangesAsync();

            return Ok(new { success = true, nuevoEstado = aplicacion.Status });

        }


    }
}
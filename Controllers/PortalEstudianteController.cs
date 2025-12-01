using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UAMPass.Models;
using System.Threading.Tasks;
using System.IO; // NECESARIO para manejar archivos

namespace UAMPass.Controllers
{
    public class PortalEstudianteController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment; // NECESARIO para saber la ruta de wwwroot

        // Inyectamos IWebHostEnvironment aquí
        public PortalEstudianteController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        private int? GetEstudianteIdFromSession()
        {
            var idStr = HttpContext.Session.GetString("EstudianteId");
            if (string.IsNullOrWhiteSpace(idStr)) return null;
            if (int.TryParse(idStr, out var id)) return id;
            return null;
        }

        public async Task<IActionResult> Profile()
        {
            var id = GetEstudianteIdFromSession();
            if (!id.HasValue) return RedirectToAction("Login", "EstudianteAuth");

            var estudiante = await _db.Estudiantes.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id.Value);
            if (estudiante == null) return RedirectToAction("Login", "EstudianteAuth");

            return View(estudiante);
        }

        // ==========================================
        //  NUEVO: GET - Mostrar formulario Subir CV
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> SubirCV()
        {
            var id = GetEstudianteIdFromSession();
            if (!id.HasValue) return RedirectToAction("Login", "EstudianteAuth");

            var estudiante = await _db.Estudiantes.FindAsync(id.Value);
            return View(estudiante);
        }

        // ==========================================
        //  NUEVO: POST - Procesar el archivo PDF
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubirCV(Estudiante modelo)
        {
            // 1. Validar sesión
            var id = GetEstudianteIdFromSession();
            if (!id.HasValue) return RedirectToAction("Login", "EstudianteAuth");

            // 2. Obtener el estudiante REAL de la BD para actualizarlo
            var estudianteDb = await _db.Estudiantes.FindAsync(id.Value);
            if (estudianteDb == null) return NotFound();

            // 3. Verificar si subieron un archivo
            if (modelo.ArchivoCV != null)
            {
                // Validar que sea PDF
                string extension = Path.GetExtension(modelo.ArchivoCV.FileName).ToLower();
                if (extension != ".pdf")
                {
                    ModelState.AddModelError("", "Solo se permiten archivos PDF.");
                    return View(estudianteDb);
                }

                // 4. Definir ruta: wwwroot/archivos/cvs
                string carpetaDestino = Path.Combine(_webHostEnvironment.WebRootPath, "archivos", "cvs");

                // Crear carpeta si no existe
                if (!Directory.Exists(carpetaDestino)) Directory.CreateDirectory(carpetaDestino);

                // 5. Crear nombre único para evitar que se sobrescriban archivos con el mismo nombre
                // Ejemplo: CV_23010380_NombreArchivo.pdf
                string nombreArchivoUnico = $"CV_{estudianteDb.CIF}_{Guid.NewGuid()}{extension}";
                string rutaCompleta = Path.Combine(carpetaDestino, nombreArchivoUnico);

                // 6. Guardar el archivo físicamente
                using (var fileStream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    await modelo.ArchivoCV.CopyToAsync(fileStream);
                }

                // 7. Borrar el CV viejo si existía (Limpieza opcional pero recomendada)
                if (!string.IsNullOrEmpty(estudianteDb.CvPdfPath))
                {
                    // Aquí tendrías que borrar el viejo, pero por ahora lo dejamos simple.
                }

                // 8. Guardar la RUTA RELATIVA en la base de datos
                // Guardamos "/archivos/cvs/nombre.pdf" para poder usarlo en el <a href>
                estudianteDb.CvPdfPath = $"/archivos/cvs/{nombreArchivoUnico}";

                _db.Update(estudianteDb);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Profile));
            }

            ModelState.AddModelError("", "Por favor selecciona un archivo.");
            return View(estudianteDb);
        }
    }
}
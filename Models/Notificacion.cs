using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UAMPass.Models
{
    public class Notificacion
    {
        [Key]
        public int Id { get; set; }

        // FK al estudiante
        [ForeignKey(nameof(Estudiante))]
        public int EstudianteId { get; set; }

        // Navegación
        public Estudiante Estudiante { get; set; } = null!;

        [Required, StringLength(150)]
        public string Titulo { get; set; } = string.Empty;

        public string? Mensaje { get; set; }

        // Tipo/ categoría opcional (p.ej. "Aplicacion", "Sistema", "Recordatorio")
        [StringLength(50)]
        public string? Tipo { get; set; }

        // Si la notificación ya fue leída por el usuario
        public bool Leida { get; set; } = false;

        // Fecha de creación de la notificación
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
    }
}

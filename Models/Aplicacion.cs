using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UAMPass.Models
{
    public enum ApplicationStatus
    {
        InReview,
        Interview,
        Accepted,
        Rejected,
        Completed
    }

    public class Aplicacion
    {
        [Key]
        public int Id { get; set; }

        // FK a Pasantia
        [ForeignKey(nameof(Pasantia))]
        public int PasantiaId { get; set; }
        public Pasantia Pasantia { get; set; } = null!;

        // FK a Estudiante
        [ForeignKey(nameof(Estudiante))]
        public int EstudianteId { get; set; }
        public Estudiante Estudiante { get; set; } = null!;

        public ApplicationStatus Status { get; set; } = ApplicationStatus.InReview;

        public DateTime FechaAplicacion { get; set; } = DateTime.UtcNow;

        public string? Comentarios { get; set; }
    }
}

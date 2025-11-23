using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UAMPass.Models
{
    public class Pasantia
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        public string Descripcion { get; set; } = string.Empty;

        public DateTime FechaPublicacion { get; set; } = DateTime.UtcNow;

        // Carreras solicitadas como CSV para simplificar (se puede normalizar después)
        public string? RequiredCareersCsv { get; set; }

        [NotMapped]
        public List<string> RequiredCareers
        {
            get => string.IsNullOrWhiteSpace(RequiredCareersCsv)
                      ? new List<string>()
                      : new List<string>(RequiredCareersCsv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
            set => RequiredCareersCsv = (value == null) ? null : string.Join(",", value);
        }

        // Relación con Empresa (FK)
        [ForeignKey(nameof(Empresa))]
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; } = null!;

        // Relación con Aplicaciones (1 pasantía tiene muchas aplicaciones)
        public ICollection<Aplicacion> Aplicaciones { get; set; } = new List<Aplicacion>();

        // Si la oferta está aprobada por Admin
        public bool Aprobada { get; set; } = false;
    }
}

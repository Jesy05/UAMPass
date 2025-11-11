using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UAMPass.Models
{
    public class Estudiante
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(120)]
        public string Nombre { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Correo { get; set; } = string.Empty;

        [StringLength(100)]
        public string Facultad { get; set; } = string.Empty;

        // Para simplificar el mapeo inicial guardamos carreras como CSV en la BD
        public string? CareersCsv { get; set; }

        // Propiedad de ayuda no mapeada para consumir en código como lista
        [NotMapped]
        public List<string> Carreras
        {
            get => string.IsNullOrWhiteSpace(CareersCsv)
                      ? new List<string>()
                      : new List<string>(CareersCsv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
            set => CareersCsv = (value == null) ? null : string.Join(",", value);
        }

        // Relación: un estudiante puede tener muchas aplicaciones
        public ICollection<Aplicacion> Aplicaciones { get; set; } = new List<Aplicacion>();

        // Fecha de registro opcional
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
    }
}

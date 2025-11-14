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

        [Required, EmailAddress, StringLength(150)]
        public string Correo { get; set; } = string.Empty;

        [StringLength(100)]
        public string Facultad { get; set; } = string.Empty;

        // Simplificación inicial: carreras como CSV
        public string? CareersCsv { get; set; }

        [NotMapped]
        public List<string> Carreras
        {
            get => string.IsNullOrWhiteSpace(CareersCsv)
                      ? new List<string>()
                      : new List<string>(CareersCsv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
            set => CareersCsv = (value == null) ? null : string.Join(",", value);
        }

        // Fecha de creación / registro
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        // Relación: un estudiante puede tener muchas aplicaciones (navegación)
        public ICollection<Aplicacion> Aplicaciones { get; set; } = new List<Aplicacion>();

        //  --- NUEVOS CAMPOS PARA AUTENTICACIÓN ---
        [Required, StringLength(100)]
        public string ContrasenaHash { get; set; } = string.Empty;

        [NotMapped]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string ContrasenaPlano { get; set; } = string.Empty;
    }
}

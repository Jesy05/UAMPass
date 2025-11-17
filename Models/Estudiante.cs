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

        // Carreras guardadas como CSV
        [StringLength(500)]
        public string? CareersCsv { get; set; }

        // Lista no mapeada a la BD, construida desde CSV
        [NotMapped]
        public List<string> Carreras
        {
            get
            {
                if (string.IsNullOrWhiteSpace(CareersCsv))
                    return new List<string>();

                return new List<string>(
                    CareersCsv.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                );
            }
            set
            {
                CareersCsv = (value == null || value.Count == 0)
                    ? null
                    : string.Join(",", value);
            }
        }

        [StringLength(50)]
        public string? CIF { get; set; }

        // Contraseña hasheada internamente
        [StringLength(300)]
        public string? ContrasenaHash { get; set; }

        // Contraseña en texto plano (solo en formulario, no se guarda)
        [NotMapped]
        [DataType(DataType.Password)]
        public string? ContrasenaPlano { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        // Relación con aplicaciones
        public ICollection<Aplicacion> Aplicaciones { get; set; } = new List<Aplicacion>();
    }
}

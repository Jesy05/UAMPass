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

        // En UAMPass.Models.Estudiante.cs

        [NotMapped] // Esto significa: "No trates de crear una columna en la base de datos para esto"
        public IFormFile? ArchivoCV { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        // Fecha de último ingreso
        public DateTime? UltimoLogin { get; set; }

        // Para recuperar contraseña sin enviar email (modo prueba)
        [StringLength(10)]
        public string? CodigoRecuperacion { get; set; }

        public string? ResetToken { get; set; }
        public DateTime? TokenExpiration { get; set; }

        // PERFIL DEL ESTUDIANTE

        // CV PDF (ruta en el sistema)
        [StringLength(300)]
        public string? CvPdfPath { get; set; }

        // Foto de perfil opcional
        [StringLength(300)]
        public string? FotoPerfilPath { get; set; }

        // Estado de la cuenta (por si en el futuro se bloquea una cuenta)
        [StringLength(20)]
        public string Estado { get; set; } = "Activo";

        // Relación con aplicaciones
        public ICollection<Aplicacion> Aplicaciones { get; set; } = new List<Aplicacion>();


        // Notificaciones del estudiante
        public ICollection<Notificacion> Notificaciones { get; set; } = new List<Notificacion>();

    }
}
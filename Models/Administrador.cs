using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace UAMPass.Models
{
    public class Administrador
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public required string Usuario { get; set; } // Usado para login

        [Required, StringLength(300)]
        public string? ContrasenaHash { get; set; } = string.Empty; // Usado para seguridad

        // Campo para recibir la contraseña en formularios (NO se guarda)
        [NotMapped]
        [DataType(DataType.Password)]
        public string? ContrasenaPlano { get; set; }

        // Datos del Perfil (Causaban errores CS1061 antes)
        [StringLength(120)]
        public string? Nombre { get; set; }

        [StringLength(150), EmailAddress]
        public string? Correo { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        [StringLength(20)]
        public string Estado { get; set; } = "Activo";

        // Foto de perfil opcional
        [StringLength(300)]
        public string? FotoPerfilPath { get; set; }

    }
}
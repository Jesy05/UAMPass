using System.ComponentModel.DataAnnotations;

namespace UAMPass.Models
{
    public class Usuario
    {
        [Key] // Indica que este campo es la clave primaria
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(100)]
        public string Contrasena { get; set; } = string.Empty;

        [StringLength(50)]
        public string Rol { get; set; } = "Usuario"; // Por defecto
    }
}

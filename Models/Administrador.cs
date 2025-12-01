using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UAMPass.Models
{
    public class Administrador
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El usuario es obligatorio")]
        [StringLength(50)]
        public string Usuario { get; set; } = string.Empty;

        // La contraseña encriptada que se guarda en la BD
        [Required]
        public string ContrasenaHash { get; set; } = string.Empty;

        // Campo temporal para recibir la contraseña del formulario (No se guarda en BD)
        // ESTA ES LA PROPIEDAD QUE TE FALTABA Y DABA ERROR
        [NotMapped]
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        public string ContrasenaPlano { get; set; } = string.Empty;
    }
}
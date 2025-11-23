using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 
using System; // Para evitar advertencias futuras

namespace UAMPass.Models
{
    public class Administrador
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Usuario { get; set; }

        [Required]
        public required string ContrasenaHash { get; set; }

        // Propiedad de la Contraseña en Texto Plano 
        [NotMapped]
        [DataType(DataType.Password)]
        public string? ContrasenaPlano { get; set; } 
    }
}
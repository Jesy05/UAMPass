using System.ComponentModel.DataAnnotations;

namespace UAMPass.Models
{
    public class Administrador
    {
        public int Id { get; set; }

        [Required]
        public required string Usuario { get; set; }

        [Required]
        public required string ContrasenaHash { get; set; }
    }
} // <--- Asegúrate de que esta llave de cierre esté al final.
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UAMPass.Models
{
    public class Empresa
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [EmailAddress]
        public string ContactoEmail { get; set; } = string.Empty;

        [StringLength(200)]
        public string Direccion { get; set; } = string.Empty;

        public string? SitioWeb { get; set; }

        // Relación: una empresa puede publicar muchas pasantías
        public ICollection<Pasantia> Pasantias { get; set; } = new List<Pasantia>();
    }
}

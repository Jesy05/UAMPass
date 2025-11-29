using System.ComponentModel.DataAnnotations;

namespace UAMPass.Models.Dto
{
    public class loginEmpresaDTO
    {
        [EmailAddress]
        public string ContactoEmail { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
    }
    public class createEmpresa
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public required string Nombre { get; set; }
        public required string Direccion { get; set; }
        [EmailAddress]
        public required string ContactoEmail { get; set; }
        public string? SitioWeb { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(5,ErrorMessage = "La contraseña debe de tener minimo 5 caracteres")]
        public required string ContrasenaHash { get; set; } // Added 'required' modifier to fix CS8618
    }

    public class listEmpresa
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
    }
}

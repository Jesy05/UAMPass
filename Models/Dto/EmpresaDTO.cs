using System.ComponentModel.DataAnnotations;

namespace UAMPass.Models.Dto
{
    public class LoginEmpresaDTO // Cambié a Mayúscula
    {
        [EmailAddress]
        public string ContactoEmail { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
    }

    public class CreateEmpresa // Cambié a Mayúscula
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección es obligatoria")]
        public string Direccion { get; set; } = string.Empty;

        [EmailAddress]
        [Required(ErrorMessage = "El email es obligatorio")]
        public string ContactoEmail { get; set; } = string.Empty;

        public string? SitioWeb { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(5, ErrorMessage = "La contraseña debe tener mínimo 5 caracteres")]
        public string ContrasenaHash { get; set; } = string.Empty;
    }

    public class ListEmpresa // Cambié a Mayúscula
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }
}
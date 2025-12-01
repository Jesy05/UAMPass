using System.ComponentModel.DataAnnotations;

namespace UAMPass.Models.Dto
{
    public class LoginEmpresaDTO 
    {
        [Required(ErrorMessage = "El correo es obligatorio."), EmailAddress]
        public string ContactoEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatorio.")] 
        public string Contrasena { get; set; } = string.Empty;
    }

    public class CreateEmpresa 
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        [EmailAddress]
        [Required(ErrorMessage = "El email es obligatorio")]
        public string ContactoEmail { get; set; } = string.Empty;
        public string? SitioWeb { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string ContrasenaHash { get; set; } = string.Empty;
    }

    public class ListEmpresa // Cambié a Mayúscula
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }
}
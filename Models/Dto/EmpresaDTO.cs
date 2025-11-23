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
        public string Nombre { get; set; } 
        public string Direccion { get; set; }
        [EmailAddress]
        public string ContactoEmail { get; set; }
        public string SitioWeb { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(5,ErrorMessage = "La contraseña debe de tener minimo 5 caracteres")]
        public string ContrasenaHash { get; set; }
    }
}

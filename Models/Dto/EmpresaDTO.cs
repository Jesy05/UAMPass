using System.ComponentModel.DataAnnotations;

namespace UAMPass.Models.Dto
{
    public class loginEmpresaDTO
    {
        [Required(ErrorMessage = "El correo es obligatorio."), EmailAddress]
        public string ContactoEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatorio.")] 
        public string Contrasena { get; set; } = string.Empty;
    }
    public class createEmpresa
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        [Required(ErrorMessage = "El correo es obligatorio."), EmailAddress]
        public string ContactoEmail { get; set; } = string.Empty;
        public string SitioWeb { get; set; } = string.Empty;
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string ContrasenaHash { get; set; } = string.Empty;
    }

    public class listEmpresa
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }
}

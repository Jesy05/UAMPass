using System.Collections.Generic;

namespace UAMPass.Models.Dto
{
    public class PasantiaDto
    {
        // Clase base
        public class Pasantias
        {
            public string Titulo { get; set; } = string.Empty; // Corregido mayúscula
            public string Descripcion { get; set; } = string.Empty; // Corregido mayúscula

            // Inicializamos la lista para que no sea nula
            public List<string> RequiredCareersCsv { get; set; } = new List<string>();

            public int Empresa { get; set; } // Corregido mayúscula
        }

        public class CreatePasantia : Pasantias // Corregido mayúscula
        {
            // Hereda todo de Pasantias
        }

        public class ListPasantia : Pasantias // Corregido mayúscula
        {
            public int IdPasantia { get; set; }
            public string NombreEmpresa { get; set; } = string.Empty;
            public string CarrerasPermitidas { get; set; } = string.Empty; // Corregido mayúscula
        }
    }
}
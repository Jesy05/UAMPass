using System.Collections.Generic;

namespace UAMPass.Models.Dto
{
    public class PasantiaDto
    {
        // Clase base
        public class Pasantias
        {
            public int Id { get; set; }
            public string Titulo { get; set; } = string.Empty; 
            public string Descripcion { get; set; } = string.Empty; 

            // Inicializamos la lista para que no sea nula
            public List<string> RequiredCareersCsv { get; set; } = new List<string>();

            public int Empresa { get; set; } 

        }

        public class CreatePasantia : Pasantias 
        {
            // Hereda todo de Pasantias
        }

        public class ListPasantia : Pasantias 
        {
            public int IdPasantia { get; set; }
            public string NombreEmpresa { get; set; } = string.Empty;
            public string CarrerasPermitidas { get; set; } = string.Empty;

        }

    }
}
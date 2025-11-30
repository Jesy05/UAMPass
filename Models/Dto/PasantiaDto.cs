namespace UAMPass.Models.Dto
{
    public class PasantiaDto
    {
        public class Pasantias
        {
            public int Id { get; set; }
            public string titulo { get; set; } = string.Empty;
            public string descripcion { get; set; } = string.Empty;
            public List<string> RequiredCareersCsv { get; set; } 
            public int empresa { get; set; }
        }

        public class createPasantia : Pasantias
        {

        }
        public class listPasantia:Pasantias
        {
            public int IdPasantia { get; set; }
            public string NombreEmpresa { get; set; } = string.Empty;
            public string carrerasPermitidas { get; set; } = string.Empty;
        }

    }
}

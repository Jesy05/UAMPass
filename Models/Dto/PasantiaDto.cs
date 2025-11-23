namespace UAMPass.Models.Dto
{
    public class PasantiaDto
    {
        public class Pasantias
        {
            public string titulo { get; set; }
            public string descripcion { get; set; }
            public List<string> RequiredCareersCsv { get; set; }
            public int empresa { get; set; }
        }

        public class createPasantia : Pasantias
        {

        }
        public class listPasantia:Pasantias
        {
            public string NombreEmpresa { get; set; }
            public string carrerasPermitidas { get; set; }
        }
    }
}

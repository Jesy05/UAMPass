namespace UAMPass.Models.Dto
{
    public class aplicacionDto
    {
        public int? IdEstudiante { get; set; }
        public int? IdEmpresa { get; set; }
        public class listAplicacion
        {
            public string Estudiante { get; set; }
            public string Pasantia { get; set; }
            public DateTime FechaAplicacion { get; set; }
            public string Estado { get; set; }
            public string empresa { get; set; }
        }

        public class createApplication
        {
            public int PasantiaId { get; set; }
            public int EstudianteId { get; set; }
            public string? Comentarios { get; set; }
        }
    }
}

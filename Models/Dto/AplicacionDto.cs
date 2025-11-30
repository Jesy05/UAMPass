namespace UAMPass.Models.Dto
{
    public class aplicacionDto
    {
        public int? IdEstudiante { get; set; }
        public int? IdEmpresa { get; set; }
        public class listAplicacion
        {
            public string Estudiante { get; set; } = string.Empty;
            public string Pasantia { get; set; } = string.Empty;
            public DateTime FechaAplicacion { get; set; }
            public string Estado { get; set; } = string.Empty;
            public string empresa { get; set; } = string.Empty;
        }

        public class createApplication
        {
            public int PasantiaId { get; set; }
            public int EstudianteId { get; set; }
            public string? Comentarios { get; set; }
        }

        public class EstadoDto
        {
            public int Id { get; set; }
            public string Estado { get; set; } = string.Empty;
        }

    }
}

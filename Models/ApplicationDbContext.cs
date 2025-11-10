using Microsoft.EntityFrameworkCore;

namespace UAMPass.Models
{
    public class ApplicationDbContext : DbContext
    {
        // Constructor: le pasaremos las opciones desde el Program.cs
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Aquí agregamos los modelos que serán tablas
        public DbSet<Usuario> Usuarios { get; set; }
    }
}

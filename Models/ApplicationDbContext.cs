using Microsoft.EntityFrameworkCore;

namespace UAMPass.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Pasantia> Pasantias { get; set; }
        public DbSet<Aplicacion> Aplicaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // configuraciones adicionales (índices, seed)...
        }
    }
}

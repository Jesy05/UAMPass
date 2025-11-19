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

        // NUEVO: tabla de notificaciones
        public DbSet<Notificacion> Notificaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Todas las tablas en el esquema uampass
            modelBuilder.HasDefaultSchema("uampass");

            // Enum ApplicationStatus guardado como texto
            modelBuilder
                .Entity<Aplicacion>()
                .Property(a => a.Status)
                .HasConversion<string>();

            // RELACIÓN estudiante -> notificaciones (1:N)
            modelBuilder.Entity<Notificacion>()
                .HasOne(n => n.Estudiante)
                .WithMany(e => e.Notificaciones)
                .HasForeignKey(n => n.EstudianteId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}

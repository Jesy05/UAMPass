using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

// Migración inicial que crea la tabla "Usuarios".
// Propósito: aplicar al esquema de la base de datos la creación de la tabla de usuarios
// Conexiones:
// - El DbContext (ApplicationDbContext) y las clases en Models\ (p. ej. Models\Usuario.cs) son
//   la fuente del modelo que originó esta migración.
// - Este archivo se aplica con __dotnet ef database update__ o cuando el pipeline de migraciones se ejecuta.
// NOTA: Es un archivo generado por EF Core; se recomienda no editar manualmente salvo para documentación.
namespace UAMPass.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Crea la tabla "Usuarios" con columnas y restricciones declaradas.
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    // Columna Id: entero, PK autogenerada por PostgreSQL usando la estrategia IDENTITY por defecto.
                    // Annotation indica al proveedor Npgsql que use ValueGenerationStrategy.IdentityByDefaultColumn.
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),

                    // Nombre: varchar(100) NOT NULL, longitud máxima aplicada en el esquema.
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),

                    // Correo: texto NOT NULL. Revisar en el modelo si existe validación/índice adicional.
                    Correo = table.Column<string>(type: "text", nullable: false),

                    // Contrasena: varchar(100) NOT NULL. En el modelo debe manejarse con hashing seguro.
                    Contrasena = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),

                    // Rol: varchar(50) NOT NULL. Indica el rol del usuario (p. ej. "Admin", "Empresa", "Estudiante").
                    Rol = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    // Define la clave primaria sobre la columna Id.
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revierte la migración: elimina la tabla "Usuarios".
            // EF llama a este método cuando se hace rollback de la migración.
            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
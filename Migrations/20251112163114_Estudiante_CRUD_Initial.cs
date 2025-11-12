using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UAMPass.Migrations
{
    /// <inheritdoc />
    public partial class Estudiante_CRUD_Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Empresas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    ContactoEmail = table.Column<string>(type: "text", nullable: false),
                    Direccion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SitioWeb = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Estudiantes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Correo = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Facultad = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CareersCsv = table.Column<string>(type: "text", nullable: true),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estudiantes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pasantias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Titulo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    FechaPublicacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RequiredCareersCsv = table.Column<string>(type: "text", nullable: true),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    Aprobada = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pasantias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pasantias_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Aplicaciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PasantiaId = table.Column<int>(type: "integer", nullable: false),
                    EstudianteId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    FechaAplicacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Comentarios = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aplicaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Aplicaciones_Estudiantes_EstudianteId",
                        column: x => x.EstudianteId,
                        principalTable: "Estudiantes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Aplicaciones_Pasantias_PasantiaId",
                        column: x => x.PasantiaId,
                        principalTable: "Pasantias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Aplicaciones_EstudianteId",
                table: "Aplicaciones",
                column: "EstudianteId");

            migrationBuilder.CreateIndex(
                name: "IX_Aplicaciones_PasantiaId",
                table: "Aplicaciones",
                column: "PasantiaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pasantias_EmpresaId",
                table: "Pasantias",
                column: "EmpresaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Aplicaciones");

            migrationBuilder.DropTable(
                name: "Estudiantes");

            migrationBuilder.DropTable(
                name: "Pasantias");

            migrationBuilder.DropTable(
                name: "Empresas");
        }
    }
}

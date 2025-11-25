using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UAMPass.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodigoRecuperacion",
                schema: "uampass",
                table: "Estudiantes",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CvPdfPath",
                schema: "uampass",
                table: "Estudiantes",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                schema: "uampass",
                table: "Estudiantes",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FotoPerfilPath",
                schema: "uampass",
                table: "Estudiantes",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UltimoLogin",
                schema: "uampass",
                table: "Estudiantes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Notificaciones",
                schema: "uampass",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EstudianteId = table.Column<int>(type: "integer", nullable: false),
                    Titulo = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Mensaje = table.Column<string>(type: "text", nullable: true),
                    Tipo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Leida = table.Column<bool>(type: "boolean", nullable: false),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notificaciones_Estudiantes_EstudianteId",
                        column: x => x.EstudianteId,
                        principalSchema: "uampass",
                        principalTable: "Estudiantes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_EstudianteId",
                schema: "uampass",
                table: "Notificaciones",
                column: "EstudianteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notificaciones",
                schema: "uampass");

            migrationBuilder.DropColumn(
                name: "CodigoRecuperacion",
                schema: "uampass",
                table: "Estudiantes");

            migrationBuilder.DropColumn(
                name: "CvPdfPath",
                schema: "uampass",
                table: "Estudiantes");

            migrationBuilder.DropColumn(
                name: "Estado",
                schema: "uampass",
                table: "Estudiantes");

            migrationBuilder.DropColumn(
                name: "FotoPerfilPath",
                schema: "uampass",
                table: "Estudiantes");

            migrationBuilder.DropColumn(
                name: "UltimoLogin",
                schema: "uampass",
                table: "Estudiantes");
        }
    }
}

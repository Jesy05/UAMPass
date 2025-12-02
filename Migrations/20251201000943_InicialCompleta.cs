using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UAMPass.Migrations
{
    /// <inheritdoc />
    public partial class InicialCompleta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "uampass");

            // =========================================================================
            //  ESTO SÍ SE EJECUTA (Porque es la tabla nueva que te falta)
            // =========================================================================
            migrationBuilder.CreateTable(
                name: "Administradores",
                schema: "uampass",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Usuario = table.Column<string>(type: "text", nullable: false),
                    ContrasenaHash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administradores", x => x.Id);
                });

            // =========================================================================
            //  AQUÍ COMIENZA EL COMENTARIO GRANDE (Ignoramos lo que ya existe)
            // =========================================================================
/* <--- INICIO DEL COMENTARIO

migrationBuilder.CreateTable(
    name: "Empresas",
    schema: "uampass",
    columns: table => new
    {
        Id = table.Column<int>(type: "integer", nullable: false)
            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
        Nombre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
        ContactoEmail = table.Column<string>(type: "text", nullable: false),
        ContrasenaHash = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
        Direccion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
        SitioWeb = table.Column<string>(type: "text", nullable: true)
    },
    constraints: table =>
    {
        table.PrimaryKey("PK_Empresas", x => x.Id);
    });

migrationBuilder.CreateTable(
    name: "Estudiantes",
    schema: "uampass",
    columns: table => new
    {
        Id = table.Column<int>(type: "integer", nullable: false)
            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
        Nombre = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
        Correo = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
        Facultad = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
        CareersCsv = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
        CIF = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
        ContrasenaHash = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
        FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
        UltimoLogin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
        CodigoRecuperacion = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
        ResetToken = table.Column<string>(type: "text", nullable: true),
        TokenExpiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
        CvPdfPath = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
        FotoPerfilPath = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
        Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
    },
    constraints: table =>
    {
        table.PrimaryKey("PK_Estudiantes", x => x.Id);
    });

migrationBuilder.CreateTable(
    name: "Usuarios",
    schema: "uampass",
    columns: table => new
    {
        Id = table.Column<int>(type: "integer", nullable: false)
            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
        Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
        Correo = table.Column<string>(type: "text", nullable: false),
        Contrasena = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
        Rol = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
    },
    constraints: table =>
    {
        table.PrimaryKey("PK_Usuarios", x => x.Id);
    });

migrationBuilder.CreateTable(
*/

        }
    }
}
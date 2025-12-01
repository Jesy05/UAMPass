using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UAMPass.Migrations
{
    /// <inheritdoc />
    public partial class AddResetTokenFieldsToEstudiante : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ContrasenaHash",
                schema: "uampass",
                table: "Estudiantes",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "ResetToken",
                schema: "uampass",
                table: "Estudiantes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TokenExpiration",
                schema: "uampass",
                table: "Estudiantes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContrasenaHash",
                schema: "uampass",
                table: "Empresas",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Administradores",
                schema: "uampass");

            migrationBuilder.DropColumn(
                name: "ResetToken",
                schema: "uampass",
                table: "Estudiantes");

            migrationBuilder.DropColumn(
                name: "TokenExpiration",
                schema: "uampass",
                table: "Estudiantes");

            migrationBuilder.DropColumn(
                name: "ContrasenaHash",
                schema: "uampass",
                table: "Empresas");

            migrationBuilder.AlterColumn<string>(
                name: "ContrasenaHash",
                schema: "uampass",
                table: "Estudiantes",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(300)",
                oldMaxLength: 300,
                oldNullable: true);
        }
    }
}

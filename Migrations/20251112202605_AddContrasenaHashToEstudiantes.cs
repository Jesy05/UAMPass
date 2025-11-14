using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UAMPass.Migrations
{
    /// <inheritdoc />
    public partial class AddContrasenaHashToEstudiantes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContrasenaHash",
                table: "Estudiantes",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContrasenaHash",
                table: "Estudiantes");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UAMPass.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdministradores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Usuario",
                schema: "uampass",
                table: "Administradores",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Correo",
                schema: "uampass",
                table: "Administradores",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                schema: "uampass",
                table: "Administradores",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Correo",
                schema: "uampass",
                table: "Administradores");

            migrationBuilder.DropColumn(
                name: "Nombre",
                schema: "uampass",
                table: "Administradores");

            migrationBuilder.AlterColumn<string>(
                name: "Usuario",
                schema: "uampass",
                table: "Administradores",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);
        }
    }
}

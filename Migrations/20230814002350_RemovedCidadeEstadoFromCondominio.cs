using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiMatos.Migrations
{
    public partial class RemovedCidadeEstadoFromCondominio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bairro",
                table: "Condominios");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Condominios");

            migrationBuilder.RenameColumn(
                name: "Cidade",
                table: "Condominios",
                newName: "Localidade");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Localidade",
                table: "Condominios",
                newName: "Cidade");

            migrationBuilder.AddColumn<string>(
                name: "Bairro",
                table: "Condominios",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Condominios",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}

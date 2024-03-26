using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiMatos.Migrations
{
    public partial class PermutaNaPropriedadeCadastroSiteProprietario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CadastroSite",
                table: "Proprietarios",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CadastroSite",
                table: "Proprietarios");
        }
    }
}

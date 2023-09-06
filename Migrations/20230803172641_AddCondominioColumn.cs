using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiMatos.Migrations
{
    public partial class AddCondominioColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Endereco",
                table: "Propriedades",
                newName: "Logradouro");

            migrationBuilder.AddColumn<string>(
                name: "Condominio",
                table: "Propriedades",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Condominio",
                table: "Propriedades");

            migrationBuilder.RenameColumn(
                name: "Logradouro",
                table: "Propriedades",
                newName: "Endereco");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiMatos.Migrations
{
    public partial class AddPermutaEmPropriedade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AceitaPermuta",
                table: "Propriedades",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AceitaPermuta",
                table: "Propriedades");
        }
    }
}

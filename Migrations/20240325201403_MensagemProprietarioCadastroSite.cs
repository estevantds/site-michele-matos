using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiMatos.Migrations
{
    public partial class MensagemProprietarioCadastroSite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Mensagem",
                table: "Proprietarios",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mensagem",
                table: "Proprietarios");
        }
    }
}

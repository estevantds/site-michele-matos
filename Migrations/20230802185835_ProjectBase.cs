using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiMatos.Migrations
{
    public partial class ProjectBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Proprietarios",
                columns: table => new
                {
                    ProprietarioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CPF = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    RG = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Celular = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    IsWhatsApp = table.Column<bool>(type: "bit", nullable: false),
                    OutroTelefone = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Nascimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proprietarios", x => x.ProprietarioId);
                });

            migrationBuilder.CreateTable(
                name: "Propriedades",
                columns: table => new
                {
                    PropriedadeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    QtdeQuartos = table.Column<int>(type: "int", nullable: false),
                    QtdeSuites = table.Column<int>(type: "int", nullable: false),
                    QtdeBanheiros = table.Column<int>(type: "int", nullable: false),
                    ParaVenda = table.Column<bool>(type: "bit", nullable: false),
                    ParaLocacao = table.Column<bool>(type: "bit", nullable: false),
                    PrecoLocacao = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    PrecoVenda = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    AreaTotal = table.Column<int>(type: "int", nullable: false),
                    AreaContruida = table.Column<int>(type: "int", nullable: false),
                    TemGaragem = table.Column<bool>(type: "bit", nullable: false),
                    CapacidadeGaragem = table.Column<int>(type: "int", nullable: false),
                    TemQuintal = table.Column<bool>(type: "bit", nullable: false),
                    TemPiscina = table.Column<bool>(type: "bit", nullable: false),
                    TemQuadra = table.Column<bool>(type: "bit", nullable: false),
                    TemSalaoFesta = table.Column<bool>(type: "bit", nullable: false),
                    TemAreaGourmet = table.Column<bool>(type: "bit", nullable: false),
                    TemChurrasqueira = table.Column<bool>(type: "bit", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Cidade = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Bairro = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmCondominio = table.Column<bool>(type: "bit", nullable: false),
                    Endereco = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Numero = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Complemento = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CEP = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Vendido = table.Column<bool>(type: "bit", nullable: false),
                    Locado = table.Column<bool>(type: "bit", nullable: false),
                    DataLocacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataVenda = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NomeProprietario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Codigo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CaminhoImagem = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ProprietarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Propriedades", x => x.PropriedadeId);
                    table.ForeignKey(
                        name: "FK_Propriedades_Proprietarios_ProprietarioId",
                        column: x => x.ProprietarioId,
                        principalTable: "Proprietarios",
                        principalColumn: "ProprietarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Propriedades_ProprietarioId",
                table: "Propriedades",
                column: "ProprietarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Propriedades");

            migrationBuilder.DropTable(
                name: "Proprietarios");
        }
    }
}

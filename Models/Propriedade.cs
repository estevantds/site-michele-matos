using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MiMatos.Models
{
    public class Propriedade
    {
        public int PropriedadeId { get; set; }

        [DisplayName("Título")]
        [MaxLength(100)]
        [Required(ErrorMessage = "Campo Obrigatório.")]
        public string Titulo { get; set; }

        [DisplayName("Descrição")]
        [MaxLength(1024)]
        [Required(ErrorMessage = "Campo Obrigatório.")]
        public string Descricao { get; set; }

        [DisplayName("Tipo")]
        [MaxLength(50)]
        [Required(ErrorMessage = "Campo Obrigatório.")]
        public string Tipo { get; set; }

        [DisplayName("Número de Quartos")]
        [Required(ErrorMessage = "Campo Obrigatório.")]
        [Range(0, 25)]
        public int QtdeQuartos { get; set; }

        [DisplayName("Número de Suítes")]
        [Required(ErrorMessage = "Campo Obrigatório.")]
        [Range(0, 25)]
        public int QtdeSuites { get; set; }

        [DisplayName("Número de Banheiros")]
        [Required(ErrorMessage = "Campo Obrigatório.")]
        [Range(0, 25)]
        public int QtdeBanheiros { get; set; }

        [DisplayName("Disponível para Venda")]
        public bool ParaVenda { get; set; }

        [DisplayName("Disponível para Locação")]
        public bool ParaLocacao { get; set; }

        [DisplayName("Preço para Locação")]
        [Column(TypeName = "decimal(12,0)")]
        [Range(0.00, 999999999)]
        public decimal PrecoLocacao { get; set; }

        [DisplayName("Preço para Venda")]
        [Column(TypeName = "decimal(12,0)")]
        [Range(0.00,999999999)]
        public decimal PrecoVenda { get; set; }

        [DisplayName("Área Total")]
        [Required(ErrorMessage = "Campo Obrigatório.")]
        public int AreaTotal { get; set; }

        [DisplayName("Área Construída")]
        [Required(ErrorMessage = "Campo Obrigatório.")]
        public int AreaContruida { get; set; }

        [DisplayName("Possui Garagem?")]
        public bool TemGaragem { get; set; }

        [DisplayName("Garagem para quantos carros?")]
        [Range(0, 25)]
        public int CapacidadeGaragem { get; set; }

        [DisplayName("Tem Quintal?")]
        public bool TemQuintal { get; set; }

        [DisplayName("Tem Piscina?")]
        public bool TemPiscina { get; set; }

        [DisplayName("Tem Quadra de Esportes?")]
        public bool TemQuadra { get; set; }

        [DisplayName("Tem Salão de Festa?")]
        public bool TemSalaoFesta { get; set; }

        [DisplayName("Tem Área Gourmet?")]
        public bool TemAreaGourmet { get; set; }

        [DisplayName("Tem Churrasqueira?")]
        public bool TemChurrasqueira { get; set; }

        [DisplayName("Estado")]
        [Required(ErrorMessage = "Campo Obrigatório.")]
        [MaxLength(20)]
        public string Estado { get; set; }

        [DisplayName("Cidade")]
        [Required(ErrorMessage = "Campo Obrigatório.")]
        [MaxLength(50)]
        public string Cidade { get; set; }

        [DisplayName("Bairro")]
        [Required(ErrorMessage = "Campo Obrigatório.")]
        [MaxLength(100)]
        public string Bairro { get; set; }

        [DisplayName("Em Condomínio")]
        public bool EmCondominio { get; set; }

        [DisplayName("Condomínio")]
        [MaxLength(50)]
        public string Condominio { get; set; }

        [DisplayName("Logradouro")]
        [Required(ErrorMessage = "Campo Obrigatório.")]
        [MaxLength(200)]
        public string Logradouro { get; set; }

        [DisplayName("Número")]
        [Required(ErrorMessage = "Campo Obrigatório.")]
        [MaxLength(10)]
        public string Numero { get; set; }

        [DisplayName("Complemento")]
        [MaxLength(20)]
        public string Complemento { get; set; }

        [DisplayName("CEP")]
        [MaxLength(8)]
        public string CEP { get; set; }

        [DisplayName("Data de Criação")]
        public DateTime CriadoEm { get; set; }

        [DisplayName("Última Edição")]
        public DateTime AtualizadoEm { get; set; }

        [DisplayName("Foi Vendido?")]
        public bool Vendido { get; set; }

        [DisplayName("Está Alugado?")]
        public bool Locado { get; set; }

        [DisplayName("Data da Locação")]
        public DateTime DataLocacao { get; set; }

        [DisplayName("Data da Venda")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime DataVenda { get; set; }

        [DisplayName("Nome do Proprietário")]
        [MaxLength(50)]
        public string NomeProprietario { get; set; }

        [DisplayName("Código")]
        [MaxLength(10)]
        public string Codigo { get; set; } = "";

        [DisplayName("Caminho da Imagem")]
        [MaxLength(200)]
        public string CaminhoImagem { get; set; } = "";

        [DisplayName("Destaque")]
        public bool Destaque { get; set; }

        public int ProprietarioId { get; set; }
        public Proprietario Proprietario { get; set; }

        public int LocalidadeId { get; set; }

        [NotMapped]
        public List<Proprietario> Proprietarios { get; set; }

        [NotMapped]
        public List<Tipo> Tipos { get; set; }

        [NotMapped]
        public List<string> Localidades { get; set; }

        [NotMapped]
        public List<string> CaminhosImagens { get; set; } = new List<string>();
    }
}

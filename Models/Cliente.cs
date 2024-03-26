using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiMatos.Models
{
    public class Cliente
    {
        public int ClienteId { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [MaxLength(50)]
        public string Nome { get; set; }

        [Display(Name = "Celular")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [MaxLength(14)]
        public string Celular { get; set; }

        [Display(Name = "É WhatsApp?")]
        public bool IsWhatsApp { get; set; }

        [Display(Name = "Outro Telefone")]
        [MaxLength(14)]
        public string OutroTelefone { get; set; }

        [Display(Name = "E-mail")]
        [MaxLength(50)]
        public string Email { get; set; }

        [Display(Name = "Cadastro do Site")]
        public bool CadastroSite { get; set; }

        [Display(Name = "Visualizado")]
        public bool Visualizado { get; set; }

        [DisplayName("Mensagem")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [MaxLength(1024)]
        public string Mensagem { get; set; }

        [Display(Name = "Data de Nascimento")]
        public DateTime Nascimento { get; set; }

        [Display(Name = "Criado em")]
        public DateTime CriadoEm { get; set; }

        [Display(Name = "Atualizado em")]
        public DateTime AtualizadoEm { get; set; }

        [NotMapped]
        public string TextoNascimento { get; set; }
    }
}

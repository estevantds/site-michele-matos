using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace MiMatos.Models
{
    public class Proprietario
    {
        public int ProprietarioId { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [MaxLength(50)]
        public string Nome { get; set; }

        [Display(Name = "CPF")]
        [MaxLength(11)]
        public string CPF { get; set; }

        [Display(Name = "RG")]
        [MaxLength(10)]
        public string RG { get; set; }

        [Display(Name = "Celular")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [MaxLength(11)]
        public string Celular { get; set; }

        [Display(Name = "É WhatsApp?")]
        public bool IsWhatsApp { get; set; }

        [Display(Name = "Outro Telefone")]
        [MaxLength(11)]
        public string OutroTelefone { get; set; }

        [Display(Name = "E-mail")]
        [MaxLength(50)]
        public string Email { get; set; }

        [Display(Name = "Data de Nascimento")]
        public DateTime Nascimento { get; set; }

        [Display(Name = "Criado em")]
        public DateTime CriadoEm { get; set; }

        [Display(Name = "Atualizado em")]
        public DateTime AtualizadoEm { get; set; }

        public ICollection<Propriedade> Propriedades { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace MiMatos.Models
{
    public class Imagem
    {
        public int ImagemId { get; set; }

        [Display(Name = "Caminho da Imagem")]
        [MaxLength(255)]
        public string Caminho { get; set; }

        [Display(Name = "É Destaque?")]
        public bool Destaque { get; set; }

        [Display(Name = "Criado em")]
        public DateTime CriadoEm { get; set; }

        public int PropriedadeId { get; set; }
        public Propriedade Propriedade { get; set; }
    }
}

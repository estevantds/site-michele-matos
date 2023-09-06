using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MiMatos.Models
{
    public class Bairro
    {
        public int BairroId { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [MaxLength(50)]
        public string Nome { get; set; }

        public int CidadeId { get; set; }

        public Cidade Cidade { get; set; }

        [NotMapped]
        public List<SelectListItem> Cidades { get; set; }

        [NotMapped]
        [Display(Name = "Cidade")]
        public string NomeCidade { get; set; }

        [NotMapped]
        public string SiglaEstado { get; set; }
    }
}

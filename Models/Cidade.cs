using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MiMatos.Models
{
    public class Cidade
    {
        public int CidadeId { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [MaxLength(50)]
        public string Nome { get; set; }

        public int EstadoId { get; set; }

        public Estado Estado { get; set; }

        public ICollection<Bairro> Bairros { get; set; }

        [NotMapped]
        public List<SelectListItem> Estados { get; set; }

        [NotMapped]
        public string NomeEstado { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MiMatos.Models
{
    public class Estado
    {
        [Key]
        public int EstadoId { get; set; }

        [Display(Name = "Nome")]
        [MaxLength(20)]
        public string Nome { get; set; }

        [Display(Name = "Sigla")]
        [StringLength(2)]
        public string Sigla { get; set; }

        public ICollection<Cidade> Cidades { get; set; }
    }
}

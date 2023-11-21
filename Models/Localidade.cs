using System.ComponentModel.DataAnnotations;

namespace MiMatos.Models
{
    public class Localidade
    {
        [Key]
        public int LocalidadeId { get; set; }
        public string Nome { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
    }
}

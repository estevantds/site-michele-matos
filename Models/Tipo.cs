using System.ComponentModel.DataAnnotations;

namespace MiMatos.Models
{
    public class Tipo
    {
        public int TipoId { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        [MaxLength(20)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        [MaxLength(2)]
        public string Prefixo { get; set; }
    }
}

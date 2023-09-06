using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MiMatos.Models
{
    public class Condominio
    {
        public int CondominioId { get; set; }

        [DisplayName("Nome")]
        [Required(ErrorMessage = "Campo Obrigatório.")]
        [MaxLength(50)]
        public string Nome { get; set; }

        [DisplayName("Logradouro")]
        [Required(ErrorMessage = "Campo Obrigatório.")]
        [MaxLength(200)]
        public string Logradouro { get; set; }

        [DisplayName("Número")]
        [Required(ErrorMessage = "Campo Obrigatório.")]
        [MaxLength(10)] 
        public string Numero { get; set; }

        [DisplayName("Localidade")]
        [Required(ErrorMessage = "Campo Obrigatório.")]
        [MaxLength(50)]
        public string Localidade { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime AtualizadoEm { get; set; }

        [NotMapped]
        public List<SelectListItem> Localidades { get; set; }
    }
}

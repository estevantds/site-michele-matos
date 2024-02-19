using MiMatos.Models;
using ReflectionIT.Mvc.Paging;
using System.ComponentModel;

namespace MiMatos.ViewModels
{
    public class ImoveisViewModel
    {
        public PagingList<Propriedade> Propriedades { get; set; }
        public List<Tipo> Tipos { get; set; }
        public List<Localidade> Localidades { get; set; }

        [DisplayName("Em Condomínio")]
        public bool EmCondominio { get; set; }

        [DisplayName("Tem Quintal")]
        public bool TemQuintal { get; set; }

        [DisplayName("Tem Piscina")]
        public bool TemPiscina { get; set; }

        [DisplayName("Tem Quadra")]
        public bool TemQuadra { get; set; }

        [DisplayName("Tem Salão de Festa")]
        public bool TemSalao { get; set; }

        [DisplayName("Tem Área Gourmet")]
        public bool TemAreaGourmet { get; set; }

        [DisplayName("Tem Churrasqueira")]
        public bool TemChurrasqueira { get; set; }

        [DisplayName("Com Suíte")]
        public bool ComSuite { get; set; }
        public string Tipo { get; set; }
        public string Localidade { get; set; }
        public string Finalidade { get; set; }
        public string TituloBusca { get; set; }
        public string QtdeQuartos { get; set; }

        public int MaxValue { get; set; }
        public int MinValue { get; set; }

        public int MaxValueLoc { get; set; }
        public int MinValueLoc { get; set; }
    }
}

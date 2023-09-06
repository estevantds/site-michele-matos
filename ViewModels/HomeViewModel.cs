using MiMatos.Models;

namespace MiMatos.ViewModels
{
    public class HomeViewModel
    {
        public List<Propriedade> Propriedades { get; set; }
        public List<Tipo> Tipos { get; set; }
        public List<Estado> Estados { get; set; }
        public List<Cidade> Cidades { get; set; }
        public List<Bairro> Bairros { get; set; }
        public List<Condominio> Condominios { get; set; }
    }
}

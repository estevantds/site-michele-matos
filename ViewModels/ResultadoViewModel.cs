using MiMatos.Models;

namespace MiMatos.ViewModels
{
    public class ResultadoViewModel
    {
        public List<Propriedade> Resultado { get; set; }
        public List<Propriedade> Destaques { get; set; }
        public string Titulo { get; set; }
        public ImoveisViewModel ImoveisVM { get; set; }
    }
}

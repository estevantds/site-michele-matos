using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiMatos.Repositories.Interfaces;
using MiMatos.ViewModels;
using ReflectionIT.Mvc.Paging;

namespace MiMatos.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPropriedadeRepository _propriedadeRepository;
        private readonly ITipoRepository _tipoRepository;
        private readonly ILocationRepository _locationRepository;

        public HomeController(IPropriedadeRepository propriedadeRepository, ITipoRepository tipoRepository, ILocationRepository locationRepository)
        {
            _propriedadeRepository = propriedadeRepository;
            _tipoRepository = tipoRepository;
            _locationRepository = locationRepository;
        }

        public IActionResult Index()
        {
            var homeVM = new HomeViewModel();

            homeVM.Propriedades = _propriedadeRepository.PropriedadesDisponiveisRecemAdicionadas().ToList();
            homeVM.Tipos = _tipoRepository.Tipos.ToList();
            homeVM.Localidades = _locationRepository.Localidades.ToList();

            return View(homeVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void Index(HomeViewModel homeVM)
        {
            var resultado = _propriedadeRepository.Propriedades.ToList();
            var imoveisVM = new ImoveisViewModel();
            var localidadeCadastrada = true;

            if (homeVM.Finalidade == "Comprar")
                resultado = resultado.Where(i => i.ParaVenda && !i.ParaLocacao).ToList();
            else if (homeVM.Finalidade == "Alugar")
                resultado = resultado.Where(i => i.ParaLocacao && !i.ParaVenda).ToList();

            if (!String.IsNullOrEmpty(homeVM.Localidade))
            {
                var localidadeId = _locationRepository.Localidades?.FirstOrDefault(l => l.Nome == homeVM.Localidade).LocalidadeId;
                if (localidadeId != null)
                    resultado = resultado.Where(i => i.LocalidadeId == localidadeId).ToList();
                else
                    localidadeCadastrada = false;
            }

            Resultado(resultado);
        }

        public IActionResult Resultado(List<Models.Propriedade> resultado)
        {
            return View(resultado);
        }
    }
}

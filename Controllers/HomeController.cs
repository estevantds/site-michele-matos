﻿using Microsoft.AspNetCore.Mvc;
using MiMatos.Repositories.Interfaces;
using MiMatos.ViewModels;

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
            homeVM.Estados = _locationRepository.Estados.ToList();
            homeVM.Cidades = _locationRepository.Cidades.ToList();
            homeVM.Bairros = _locationRepository.Bairros.ToList();
            homeVM.Condominios = _locationRepository.Condominios.ToList();

            return View(homeVM);
        }
    }
}

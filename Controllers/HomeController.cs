using Microsoft.AspNetCore.Mvc;
using MiMatos.Context;
using MiMatos.Models;
using MiMatos.Repositories.Interfaces;
using MiMatos.ViewModels;

namespace MiMatos.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPropriedadeRepository _propriedadeRepository;
        private readonly ITipoRepository _tipoRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly AppDbContext _context;

        public HomeController(IPropriedadeRepository propriedadeRepository, ITipoRepository tipoRepository, ILocationRepository locationRepository, AppDbContext context)
        {
            _propriedadeRepository = propriedadeRepository;
            _tipoRepository = tipoRepository;
            _locationRepository = locationRepository;
            _context = context;
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
        public IActionResult Index(HomeViewModel homeVM)
        {
            int tipoId = 0;
            int localidadeId = 0;
            bool paraCompra = false;
            bool paraLocacao = false;

            if (homeVM.Tipo != "Tipo")
                tipoId = _context.Tipos.FirstOrDefault(i => i.Nome == homeVM.Tipo).TipoId;

            if (!String.IsNullOrEmpty(homeVM.Localidade))
                localidadeId = _context.Localidades.FirstOrDefault(l => l.Nome == homeVM.Localidade).LocalidadeId;

            if (homeVM.Finalidade == "Finalidade")
            {
                paraCompra = true;
                paraLocacao = true;
            }
            else
            {
                if (homeVM.Finalidade == "Comprar")
                    paraCompra = true;
                else if (homeVM.Finalidade == "Alugar")
                    paraLocacao = true;
                else
                {
                    paraCompra = true;
                    paraLocacao = true;
                }
            }

            var resultado = Resultado(paraCompra, paraLocacao, tipoId, localidadeId);

            return View("Resultado", resultado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Filtro(ResultadoViewModel resultadoVM)
        {
            var imoveisVM = resultadoVM.ImoveisVM;
            var resultado = _context.Propriedades.ToList();
            var destaques = _context.Propriedades.Where(i => i.Destaque).ToList();
            var tipos = _context.Tipos.ToList();
            var localidades = _context.Localidades.ToList();
            var localidadeId = 0;

            if (!string.IsNullOrEmpty(imoveisVM.Localidade))
            {
                localidadeId = _context.Localidades.FirstOrDefault(i => i.Nome == imoveisVM.Localidade).LocalidadeId;

                if (imoveisVM.EmCondominio)
                    resultado = resultado.Where(i => i.LocalidadeId == localidadeId && i.EmCondominio).ToList();
                else
                    resultado = resultado.Where(i => i.LocalidadeId == localidadeId).ToList();

                if (resultado.Count() <= 0)
                    resultadoVM.Titulo = "Nenhum imóvel corresponde à busca.";
            }

            if (imoveisVM.Finalidade == "Venda")
                resultado = resultado.Where(i => i.ParaVenda && !i.ParaLocacao && i.PrecoVenda >= imoveisVM.MinValue && i.PrecoVenda <= imoveisVM.MaxValue).ToList();
            else if (imoveisVM.Finalidade == "Locação")
                resultado = resultado.Where(i => i.ParaLocacao && !i.ParaVenda && i.PrecoLocacao >= imoveisVM.MinValueLoc && i.PrecoLocacao <= imoveisVM.MaxValueLoc).ToList();
            else
                resultado = resultado.Where(i => i.ParaVenda && i.PrecoVenda >= imoveisVM.MinValue && i.PrecoVenda <= imoveisVM.MaxValue ||
                                                 i.ParaLocacao && i.PrecoLocacao >= imoveisVM.MinValueLoc && i.PrecoLocacao <= imoveisVM.MaxValueLoc).ToList();

            if (imoveisVM.Tipo == "Todos")
            {
                if (localidadeId != 0)
                    resultadoVM.Titulo = $"todos os imóveis para {imoveisVM.Finalidade} em {imoveisVM.Localidade}";
                else
                    resultadoVM.Titulo = $"todos os imóveis para {imoveisVM.Finalidade}";
            }
            else
            {
                resultado = resultado.Where(i => i.Tipo == imoveisVM.Tipo).ToList();

                if (localidadeId != 0 && string.IsNullOrEmpty(imoveisVM.TituloBusca))
                    resultadoVM.Titulo = $"{imoveisVM.Tipo} para {imoveisVM.Finalidade} em {imoveisVM.Localidade}";
                else if (string.IsNullOrEmpty(imoveisVM.TituloBusca))
                    resultadoVM.Titulo = $"{imoveisVM.Tipo} para {imoveisVM.Finalidade}";
            }

            if (imoveisVM.Finalidade == "Venda" || imoveisVM.Finalidade == "Venda ou Locação")
                resultado = resultado.Where(i => i.PrecoVenda >= imoveisVM.MinValue && i.PrecoVenda <= imoveisVM.MaxValue).ToList();

            if (imoveisVM.Finalidade == "Locação" || imoveisVM.Finalidade == "Venda ou Locação")
                resultado = resultado.Where(i => i.PrecoLocacao >= imoveisVM.MinValueLoc && i.PrecoLocacao <= imoveisVM.MaxValueLoc).ToList();

            if (imoveisVM.QtdeQuartos != "Indiferente")
            {
                if (imoveisVM.QtdeQuartos != "4+")
                    resultado = resultado.Where(i => i.QtdeQuartos == Int32.Parse(imoveisVM.QtdeQuartos)).ToList();
                else
                    resultado = resultado.Where(i => i.QtdeQuartos >= 4).ToList();
            }

            if (imoveisVM.ComSuite)
                resultado = resultado.Where(i => i.QtdeSuites > 0).ToList();

            if (imoveisVM.TemQuintal)
                resultado = resultado.Where(i => i.TemQuintal).ToList();

            if (imoveisVM.TemPiscina)
                resultado = resultado.Where(i => i.TemPiscina).ToList();

            if (imoveisVM.TemQuadra)
                resultado = resultado.Where(i => i.TemQuadra).ToList();

            if (imoveisVM.TemSalao)
                resultado = resultado.Where(i => i.TemSalaoFesta).ToList();

            if (imoveisVM.TemAreaGourmet)
                resultado = resultado.Where(i => i.TemAreaGourmet).ToList();

            if (imoveisVM.TemChurrasqueira)
                resultado = resultado.Where(i => i.TemChurrasqueira).ToList();

            resultadoVM.ImoveisVM.Tipos = tipos;
            resultadoVM.ImoveisVM.Localidades = localidades;
            resultadoVM.Resultado = SetImagens(resultado);
            resultadoVM.Destaques = SetImagens(destaques);

            return View("Resultado", resultadoVM);
        }

        public IActionResult Limpar()
        {
            var resultados = new ResultadoViewModel();
            resultados.ImoveisVM = new ImoveisViewModel();
            var resultado = _context.Propriedades.ToList();
            var destaques = _context.Propriedades.Where(i => i.Destaque).ToList();
            resultados.ImoveisVM.Tipos = _context.Tipos.ToList();
            resultados.ImoveisVM.Localidades = _context.Localidades.ToList();

            resultados.Resultado = SetImagens(resultado);
            resultados.Destaques = SetImagens(destaques);
            resultados.Titulo = "todos os imóveis";

            return View("Resultado", resultados);
        }

        private ResultadoViewModel Resultado(bool paraCompra, bool paraLocacao, int tipoId, int localidadeId)
        {
            var propriedades = _context.Propriedades.ToList();
            var destaques = _context.Propriedades.Where(i => i.Destaque).ToList();

            var resultadoVM = new ResultadoViewModel();
            resultadoVM.ImoveisVM = new ImoveisViewModel();
            resultadoVM.ImoveisVM.Tipos = _context.Tipos.ToList();
            resultadoVM.ImoveisVM.Localidades = _context.Localidades.ToList();

            resultadoVM.Titulo = "";

            string tipoNome = "";
            
            if (tipoId != 0) 
            {
                tipoNome = _context.Tipos.FirstOrDefault(t => t.TipoId == tipoId).Nome;
                propriedades = propriedades.Where(i => i.Tipo == tipoNome).ToList();

                resultadoVM.Titulo += tipoNome;
            }
            else
            {
                resultadoVM.Titulo += "todos os imóveis";
            }

            if (paraCompra == paraLocacao)
            {
                propriedades = propriedades.Where(i => i.ParaVenda == paraCompra || i.ParaLocacao == paraLocacao).ToList();
                resultadoVM.Titulo += " para comprar ou alugar";
            }
            else
            {
                propriedades = propriedades.Where(i => i.ParaVenda == paraCompra && i.ParaLocacao == paraLocacao).ToList();
                if (paraCompra)
                    resultadoVM.Titulo += " para comprar";

                if (paraLocacao)
                {
                    resultadoVM.Titulo += " para alugar";
                }
            }


            if (localidadeId != 0)
            {
                propriedades = propriedades.Where(i => i.LocalidadeId == localidadeId).ToList();

                var localidade = _context.Localidades.FirstOrDefault(l => l.LocalidadeId == localidadeId).Nome;
                resultadoVM.Titulo += $" em {localidade}";
            }

            if (propriedades.Count == 0 && paraCompra && tipoId != 0)
                propriedades = propriedades.Where(i => i.ParaVenda && i.Tipo == tipoNome).ToList();
            else if (propriedades.Count == 0 && paraCompra && tipoId == 0)
                propriedades = propriedades.Where(i => i.ParaVenda).ToList();
            else if (propriedades.Count == 0 && paraLocacao && tipoId != 0)
                propriedades = propriedades.Where(i => i.ParaVenda && i.Tipo == tipoNome).ToList();
            else if (propriedades.Count == 0 && paraLocacao && tipoId == 0)
                propriedades = propriedades.Where(i => i.ParaVenda).ToList();

            resultadoVM.Resultado = SetImagens(propriedades);
            resultadoVM.Destaques = SetImagens(destaques);

            return resultadoVM;
        }

        private List<Propriedade> SetImagens (List<Propriedade> propriedades)
        {
            foreach (var propriedade in propriedades)
            {
                var imagens = _context.Imagens.Where(i => i.PropriedadeId == propriedade.PropriedadeId).ToList();

                if (imagens.Count() > 0)
                {
                    var imagensDestaques = imagens.Where(i => i.Destaque);
                    if (imagensDestaques.Count() > 0)
                    {
                        foreach (var imagem in imagensDestaques)
                        {
                            if (imagem.Destaque)
                            {
                                propriedade.CaminhoImagem = imagem.Caminho;
                            }
                        }
                    }
                    else
                    {
                        propriedade.CaminhoImagem = imagens.FirstOrDefault().Caminho;
                    }

                    foreach (var imagem in imagens)
                    {
                        if (imagem.Caminho != propriedade.CaminhoImagem)
                        {
                            if (propriedade.CaminhosImagens.Count() < 6)
                            {
                                propriedade.CaminhosImagens.Add(imagem.Caminho);
                            }
                        }
                    }
                }
                else
                {
                    propriedade.CaminhoImagem = "/images/preparando-imovel.png";
                }
            }

            return propriedades;
        }
    }
}

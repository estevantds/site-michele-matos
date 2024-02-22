using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiMatos.Context;
using MiMatos.Models;
using MiMatos.ViewModels;
using ReflectionIT.Mvc.Paging;

namespace MiMatos.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminPropriedadesController : Controller
    {
        private string _serverPath;
        private readonly AppDbContext _context;

        public AdminPropriedadesController(IWebHostEnvironment webHostEnvironment, AppDbContext context)
        {
            _serverPath = webHostEnvironment.WebRootPath;
            _context = context;
        }

        public async Task<IActionResult> Index(string filter, int pageindex = 1, string sort = "CriadoEm")
        {
            var imoveisVM = new ImoveisViewModel();
            var resultado = _context.Propriedades.AsNoTracking().AsQueryable();
            var imagens = _context.Imagens.ToList();
            var tipos = _context.Tipos.ToList();
            var localidades = _context.Localidades.ToList();


            if (!string.IsNullOrWhiteSpace(filter))
                resultado = resultado.Where(p => p.Bairro.Contains(filter));

            imoveisVM.Propriedades = await PagingList.CreateAsync(resultado, 5, pageindex, sort, "Titulo");
            
            foreach (var item in imoveisVM.Propriedades)
            {
                var itemImagens = imagens.Where(i => i.PropriedadeId == item.PropriedadeId);
                if (itemImagens.Count() > 0)
                {
                    if (itemImagens.Any(i => i.Destaque))
                        item.CaminhoImagem = itemImagens.FirstOrDefault(i => i.Destaque).Caminho;
                    else
                        item.CaminhoImagem = itemImagens.FirstOrDefault().Caminho;
                }
            }

            imoveisVM.Propriedades.RouteValue = new RouteValueDictionary
            {
                { "filter", filter }
            };

            imoveisVM.Tipos = tipos;
            imoveisVM.Localidades = localidades;

            return View(imoveisVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ImoveisViewModel imoveisVM, int pageindex = 1, string sort = "CriadoEm")
        {
            var resultado = _context.Propriedades.AsNoTracking().AsQueryable();
            var tipos = _context.Tipos.ToList();
            var localidades = _context.Localidades.ToList();
            var localidadeId = 0;

            if (!string.IsNullOrEmpty(imoveisVM.Localidade))
            {
                localidadeId = _context.Localidades.FirstOrDefault(i => i.Nome == imoveisVM.Localidade).LocalidadeId;

                if (imoveisVM.EmCondominio)
                    resultado = resultado.Where(i => i.LocalidadeId == localidadeId && i.EmCondominio);
                else
                    resultado = resultado.Where(i => i.LocalidadeId == localidadeId);

                if (resultado.Count() <= 0)
                    imoveisVM.TituloBusca = "Nenhum imóvel corresponde à busca.";
            }

            if (imoveisVM.Finalidade == "Venda")
                resultado = resultado.Where(i => i.ParaVenda && !i.ParaLocacao && i.PrecoVenda >= imoveisVM.MinValue && i.PrecoVenda <= imoveisVM.MaxValue);
            else if (imoveisVM.Finalidade == "Locação")
                resultado = resultado.Where(i => i.ParaLocacao && !i.ParaVenda && i.PrecoLocacao >= imoveisVM.MinValueLoc && i.PrecoLocacao <= imoveisVM.MaxValueLoc);
            else
                resultado = resultado.Where(i => i.ParaVenda && i.PrecoVenda >= imoveisVM.MinValue && i.PrecoVenda <= imoveisVM.MaxValue || 
                                                 i.ParaLocacao && i.PrecoLocacao >= imoveisVM.MinValueLoc && i.PrecoLocacao <= imoveisVM.MaxValueLoc);

            if (imoveisVM.Tipo == "Todos")
            {
                if (localidadeId != 0)
                    imoveisVM.TituloBusca = "Resultado de qualquer imóvel para " + imoveisVM.Finalidade + " em " + imoveisVM.Localidade;
                else
                    imoveisVM.TituloBusca = "Resultado de qualquer imóvel para " + imoveisVM.Finalidade + " em todas as localidades";
            }
            else
            {
                resultado = resultado.Where(i => i.Tipo == imoveisVM.Tipo);

                if (localidadeId != 0 && string.IsNullOrEmpty(imoveisVM.TituloBusca))
                    imoveisVM.TituloBusca = "Resultado de " + imoveisVM.Tipo + " para " + imoveisVM.Finalidade + " em " + imoveisVM.Localidade;
                else if (string.IsNullOrEmpty(imoveisVM.TituloBusca))
                    imoveisVM.TituloBusca = "Resultado de " + imoveisVM.Tipo + " para " + imoveisVM.Finalidade + " em todas as localidades";
            }

            if (imoveisVM.Finalidade == "Venda" || imoveisVM.Finalidade == "Venda ou Locação")
                resultado = resultado.Where(i => i.PrecoVenda >= imoveisVM.MinValue && i.PrecoVenda <= imoveisVM.MaxValue);

            if(imoveisVM.Finalidade == "Locação" || imoveisVM.Finalidade == "Venda ou Locação")
                resultado = resultado.Where(i => i.PrecoLocacao >= imoveisVM.MinValueLoc && i.PrecoLocacao <= imoveisVM.MaxValueLoc);

            if(imoveisVM.QtdeQuartos != "Indiferente")
            {
                if (imoveisVM.QtdeQuartos != "4+")
                    resultado = resultado.Where(i => i.QtdeQuartos == Int32.Parse(imoveisVM.QtdeQuartos));
                else
                    resultado = resultado.Where(i => i.QtdeQuartos >= 4);
            }

            if (imoveisVM.ComSuite)
                resultado = resultado.Where(i => i.QtdeSuites > 0);

            if (imoveisVM.TemQuintal)
                resultado = resultado.Where(i => i.TemQuintal);

            if (imoveisVM.TemPiscina)
                resultado = resultado.Where(i => i.TemPiscina);

            if (imoveisVM.TemQuadra)
                resultado = resultado.Where(i => i.TemQuadra);

            if (imoveisVM.TemSalao)
                resultado = resultado.Where(i => i.TemSalaoFesta);

            if (imoveisVM.TemAreaGourmet)
                resultado = resultado.Where(i => i.TemAreaGourmet);

            if (imoveisVM.TemChurrasqueira)
                resultado = resultado.Where(i => i.TemChurrasqueira);

            if (resultado.Count() == 0)
                imoveisVM.TituloBusca = "Nenhum imóvel encontrado. Revise os filtros.";

            imoveisVM.Tipos = tipos;
            imoveisVM.Localidades = localidades;

            imoveisVM.Propriedades = await PagingList.CreateAsync(resultado, 5, pageindex, sort, "Titulo");

            return View(imoveisVM);
        }

        public async Task<IActionResult> ListByCondominio(int? id, string filter, int pageindex = 1, string sort = "CriadoEm")
        {
            if (id == null)
                return NotFound();

            var condominio = _context.Condominios.FirstOrDefault(c => c.CondominioId == id).Nome;
            var resultado = _context.Propriedades.AsNoTracking().AsQueryable().Where(p => p.Condominio == condominio);

            if (resultado.Count() == 0)
                return RedirectToRoute(new { action = "NenhumImovelCondominio", controller = "AdminPropriedades", id });

            if (!string.IsNullOrWhiteSpace(filter))
                resultado = resultado.Where(p => p.Bairro.Contains(filter));

            var propriedades = await PagingList.CreateAsync(resultado, 5, pageindex, sort, "Nome");

            propriedades.RouteValue = new RouteValueDictionary
            {
                { "filter", filter }
            };

            return View(propriedades);
        }

        public async Task<IActionResult> ListByProprietario(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var propriedades = await _context.Propriedades.Where(p => p.ProprietarioId == id).ToListAsync();

            if(propriedades.Count() == 0)
            {
                return RedirectToRoute(new { action = "NenhumImovelProprietario", controller = "AdminPropriedades", id });
            }

            return View(propriedades);
        }

        public IActionResult Create()
        {
            var proprietarios = _context.Proprietarios.ToList();
            var tipos = _context.Tipos.ToList();
            var condominios = _context.Condominios.ToList();
            var bairros = _context.Bairros.ToList().OrderBy(b => b.Cidade.Nome);
            var cidades = _context.Cidades.ToList();
            var estados = _context.Estados.ToList();

            var model = new Propriedade();
            model.Tipos = new List<Tipo>();
            model.Proprietarios = new List<Proprietario>();
            model.Localidades = new List<string>();

            if (proprietarios != null)
            {
                foreach (var item in proprietarios)
                {
                    model.Proprietarios.Add(item);
                }
            }

            if (tipos != null)
            {
                foreach (var item in tipos)
                {
                    model.Tipos.Add(item);
                }
            }

            if (bairros != null)
            {
                foreach (var item in bairros)
                {
                    var cidade = cidades.FirstOrDefault(c => c.CidadeId == item.CidadeId);
                    var estado = estados.FirstOrDefault(e => e.EstadoId == cidade.EstadoId);

                    model.Localidades.Add(item.Nome + ", " + cidade.Nome + ", " + estado.Nome);
                }

                foreach (var item in condominios)
                {
                    model.Localidades.Add(item.Nome + ", " + item.Localidade);
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Propriedade propriedade)
        {
            propriedade.CriadoEm = DateTime.Now;
            propriedade.AtualizadoEm = DateTime.Now;

            propriedade.ProprietarioId = _context.Proprietarios.FirstOrDefault(o => o.Nome == propriedade.NomeProprietario).ProprietarioId;

            if (propriedade.EmCondominio)
            {
                var localidade = propriedade.Bairro.Split(", ");

                propriedade.Condominio = localidade[0];
                propriedade.Bairro = localidade[1];
                propriedade.Cidade = localidade[2];
                propriedade.Estado = localidade[3];

                _context.Add(propriedade);
            }
            else
            {
                var localidade = propriedade.Bairro.Split(", ");

                propriedade.Bairro = localidade[0];
                propriedade.Cidade = localidade[1];
                propriedade.Estado = localidade[2];

                _context.Add(propriedade);
            }

            _context.SaveChanges();

            propriedade.Codigo = SetCode(propriedade.PropriedadeId);

            _context.Update(propriedade);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var condominios = _context.Condominios.ToList();
            var bairros = _context.Bairros.ToList().OrderBy(b => b.Cidade.Nome);
            var cidades = _context.Cidades.ToList();
            var estados = _context.Estados.ToList();

            if (id == null)
            {
                return NotFound();
            }

            var propriedade = await _context.Propriedades.FindAsync(id);

            if (propriedade == null)
            {
                return NotFound();
            }

            var proprietarios = _context.Proprietarios.ToList();

            propriedade.Proprietarios = new List<Proprietario>();

            propriedade.Localidades = new List<string>();

            foreach (var item in bairros)
            {
                var cidade = cidades.FirstOrDefault(c => c.CidadeId == item.CidadeId);
                var estado = estados.FirstOrDefault(e => e.EstadoId == cidade.EstadoId);

                propriedade.Localidades.Add(item.Nome + ", " + cidade.Nome + ", " + estado.Nome);
            }
            foreach (var item in condominios)
            {
                propriedade.Localidades.Add(item.Nome + ", " + item.Localidade);
            }

            if (proprietarios != null)
            {
                foreach (var item in proprietarios)
                {
                    propriedade.Proprietarios.Add(item);
                }
            }

            return View(propriedade);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Propriedade propriedade)
        {
            propriedade.ProprietarioId = _context.Proprietarios.FirstOrDefault(p => p.Nome == propriedade.NomeProprietario).ProprietarioId;
            
            if (propriedade.PropriedadeId != id)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    if (propriedade.EmCondominio)
                    {
                        var localidade = propriedade.Bairro.Split(", ");

                        propriedade.Condominio = localidade[0];
                        propriedade.Bairro = localidade[1];
                        propriedade.Cidade = localidade[2];
                        propriedade.Estado = localidade[3];

                        _context.Add(propriedade);
                    }
                    else
                    {
                        var localidade = propriedade.Bairro.Split(", ");

                        propriedade.Bairro = localidade[0];
                        propriedade.Cidade = localidade[1];
                        propriedade.Estado = localidade[2];

                        _context.Add(propriedade);
                    }

                    propriedade.AtualizadoEm = DateTime.Now;

                    _context.Update(propriedade);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PropriedadeExists(propriedade.PropriedadeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var propriedade = await _context.Propriedades.FirstOrDefaultAsync(m => m.PropriedadeId == id);
            if (propriedade == null)
            {
                return NotFound();
            }

            return View(propriedade);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var propriedade = await _context.Propriedades.FindAsync(id);

            if (propriedade != null)
            {
                var imagens = _context.Imagens.Where(i => i.PropriedadeId == id);

                foreach (var imagem in imagens)
                {
                    if (System.IO.File.Exists(_serverPath + imagem.Caminho))
                    {
                        System.IO.File.Delete(_serverPath + imagem.Caminho);
                    }
                }

                _context.Propriedades.Remove(propriedade);
                await _context.SaveChangesAsync();


            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult NenhumImovelCondominio(int id)
        {
            var condominio = _context.Condominios.Find(id);
            return View(condominio);
        }

        public IActionResult NenhumImovelProprietario(int id)
        {
            var proprietario = _context.Proprietarios.Find(id);
            return View(proprietario);
        }

        private bool PropriedadeExists(int id)
        {
            return _context.Propriedades.Any(e => e.PropriedadeId == id);
        }

        private string SetCode(int propriedadeId)
        {
            var propriedade = _context.Propriedades.Find(propriedadeId);
            var tipos = _context.Tipos.ToList();

            string numeroCodigo;
            string prefixo = tipos.FirstOrDefault(t => t.Nome == propriedade.Tipo).Prefixo;

            switch (propriedadeId.ToString().Length)
            {
                case 1:
                    numeroCodigo = "000";
                    break;
                case 2:
                    numeroCodigo = "00";
                    break;
                case 3:
                    numeroCodigo = "0";
                    break;
                default:
                    numeroCodigo = "";
                    break;
            }

            return prefixo + numeroCodigo + propriedade.PropriedadeId.ToString();
        }
    }
}

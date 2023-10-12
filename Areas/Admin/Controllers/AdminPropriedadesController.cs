using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiMatos.Context;
using MiMatos.Models;

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

        public IActionResult Index()
        {
            var propriedades = _context.Propriedades.ToList();
            var imagens = _context.Imagens.ToList();
            var tipos = _context.Tipos.ToList();

            foreach (var item in propriedades)
            {
                var itemImagens = imagens.Where(i => i.PropriedadeId == item.PropriedadeId);
                if (itemImagens.Count() > 0)
                {
                    if (itemImagens.Any(i => i.Destaque))
                    {
                        item.CaminhoImagem = itemImagens.FirstOrDefault(i => i.Destaque).Caminho;
                    }
                    else
                    {
                        item.CaminhoImagem = itemImagens.FirstOrDefault().Caminho;
                    }
                }
            }
            return View(propriedades);
        }

        public IActionResult ListByCondominio(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var condominio = _context.Condominios.FirstOrDefault(c => c.CondominioId == id).Nome;
            var propriedades = _context.Propriedades.Where(p => p.Condominio == condominio);

            if (propriedades.Count() == 0)
            {
                return RedirectToRoute(new { action = "NenhumImovelCondominio", controller = "AdminPropriedades", id });
            }

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
            model.Tipos = new List<SelectListItem>();
            model.Proprietarios = new List<SelectListItem>();
            model.Localidades = new List<SelectListItem>();

            if (proprietarios != null)
            {
                model.Proprietarios.Add(new SelectListItem { Text = "", Value = "", Selected = true });
                foreach (var item in proprietarios)
                {
                    model.Proprietarios.Add(new SelectListItem { Text = item.Nome, Value = item.Nome });
                }
            }

            if (tipos != null)
            {
                model.Tipos.Add(new SelectListItem { Text = "", Value = "", Selected = true });
                foreach (var item in tipos)
                {
                    model.Tipos.Add(new SelectListItem { Text = item.Nome, Value = item.Nome });
                }
            }

            if (bairros != null)
            {
                model.Localidades.Add(new SelectListItem { Text = "", Value = "", Selected = true });
                foreach (var item in bairros)
                {
                    var cidade = cidades.FirstOrDefault(c => c.CidadeId == item.CidadeId);
                    var estado = estados.FirstOrDefault(e => e.EstadoId == cidade.EstadoId);

                    model.Localidades.Add(new SelectListItem
                    {
                        Text = item.Nome + ", " + cidade.Nome + ", " + estado.Nome,
                        Value = item.Nome + ", " + cidade.Nome + ", " + estado.Nome
                    });
                }

                foreach (var item in condominios)
                {
                    model.Localidades.Add(new SelectListItem
                    {
                        Text = item.Nome + ", " + item.Localidade,
                        Value = item.Nome + ", " + item.Localidade
                    });
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

            propriedade.Proprietarios = new List<SelectListItem>();

            if (proprietarios != null)
            {
                propriedade.Proprietarios.Add(new SelectListItem { Text = propriedade.NomeProprietario, Value = propriedade.NomeProprietario, Selected = true });
                foreach (var item in proprietarios)
                {
                    if (item.Nome != propriedade.NomeProprietario)
                    {
                        propriedade.Proprietarios.Add(new SelectListItem { Text = item.Nome, Value = item.Nome });
                    }
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

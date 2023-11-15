using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiMatos.Context;
using MiMatos.Models;

namespace MiMatos.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminBairrosController : Controller
    {
        private readonly AppDbContext _context;

        public AdminBairrosController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var bairros = _context.Bairros.ToList();

            foreach (var bairro in bairros)
            {
                var cidade = _context.Cidades.FirstOrDefault(c => c.CidadeId == bairro.CidadeId);
                var estado = _context.Estados.FirstOrDefault(e => e.EstadoId == cidade.EstadoId);
                bairro.NomeCidade = cidade.Nome;
                bairro.SiglaEstado = estado.Sigla;
                    
            }

            return View(bairros.OrderBy(b => b.NomeCidade));
        }

        public IActionResult ListByCidade(int? id)
        {
            var bairros = _context.Bairros.Where(b => b.CidadeId == id).ToList();

            if(bairros.Count < 1)
            {
                return RedirectToAction("Create", "AdminBairros");
            }

            foreach (var bairro in bairros)
            {
                var cidade = _context.Cidades.FirstOrDefault(c => c.CidadeId == bairro.CidadeId);
                var estado = _context.Estados.FirstOrDefault(e => e.EstadoId == cidade.EstadoId);
                bairro.NomeCidade = cidade.Nome;
                bairro.SiglaEstado = estado.Sigla;

            }

            return View(bairros.OrderBy(b => b.Nome));
        }

        public IActionResult Create()
        {
            var cidades = _context.Cidades.OrderBy(c => c.Nome).ToList();
            cidades.OrderBy(c => c.Estado.Sigla);

            var model = new Bairro();
            model.Cidades = new List<Cidade>();

            if (cidades != null)
            {
                foreach (var item in cidades)
                {
                    model.Cidades.Add(item);
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Bairro bairro)
        {
            var cidade = _context.Cidades.FirstOrDefault(e => e.Nome == bairro.NomeCidade);
            bairro.Cidade = cidade;
            bairro.CidadeId = cidade.CidadeId;

            var bairroExists = _context.Bairros.Any(b => b.Nome == bairro.Nome);

            if (ModelState.IsValid)
            {
                if (bairroExists)
                {
                    var bairrosExistentes = _context.Bairros.Where(b => b.Nome == bairro.Nome).ToList();
                    
                    var naMesmaCidade = bairrosExistentes.Count() > 0 && bairrosExistentes.Any(be => be.CidadeId == cidade.CidadeId);
                    
                    if (naMesmaCidade)
                    {
                        ModelState.AddModelError("Nome inválido", "Já existe um 'Bairro' com esse nome nessa Cidade.");
                        return View(bairro);
                    }
                }
                _context.Add(bairro);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "AdminBairros");
            }
            return View();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bairro = await _context.Bairros.FindAsync(id);
            var cidades = _context.Cidades.OrderBy(c => c.Nome).ToList();
            cidades.OrderBy(c => c.Estado.Sigla);

            var cidade = _context.Cidades.FirstOrDefault(e => e.CidadeId == bairro.CidadeId);

            if (bairro != null)
            {
                bairro.NomeCidade = cidade.Nome;

                bairro.Cidades = new List<Cidade>();

                // bairro.Cidades.Add(new SelectListItem { Text = cidade.Nome, Value = cidade.Nome });
                foreach (var item in cidades)
                {
                    bairro.Cidades.Add(item);
                }
            }
            else
            {
                return NotFound();
            }
            return View(bairro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Bairro bairro)
        {
            if (id != bairro.BairroId)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bairro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BairroExists(bairro.BairroId))
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
            return View(bairro);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bairro = await _context.Bairros.FirstOrDefaultAsync(m => m.BairroId == id);
            if (bairro == null)
            {
                return NotFound();
            }

            return View(bairro);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var bairro = await _context.Bairros.FindAsync(id);
            if (bairro != null)
            {
                _context.Bairros.Remove(bairro);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool BairroExists(int id)
        {
            return _context.Bairros.Any(e => e.BairroId == id);
        }
    }
}

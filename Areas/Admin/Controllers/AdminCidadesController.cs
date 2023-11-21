using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiMatos.Context;
using MiMatos.Models;

namespace MiMatos.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminCidadesController : Controller
    {
        private readonly AppDbContext _context;

        public AdminCidadesController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cidades = _context.Cidades;
            var estados = _context.Estados.ToList();

            foreach (var item in cidades)
            {
                item.Estado = estados.FirstOrDefault(e => e.EstadoId == item.EstadoId);
            }

            return View(cidades);
        }

        public IActionResult Create()
        {
            var estados = _context.Estados.ToList();
            var model = new Cidade();
            model.Estados = new List<SelectListItem>();

            if(estados != null)
            {
                model.Estados.Add(new SelectListItem { Text = "", Value = "", Selected = true });
                foreach (var item in estados)
                {
                    model.Estados.Add(new SelectListItem { Text = item.Sigla, Value = item.Nome });
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cidade cidade)
        {
            var estado = _context.Estados.FirstOrDefault(e => e.Nome == cidade.NomeEstado);
            cidade.EstadoId = estado.EstadoId;
            cidade.NomeEstado = estado.Nome;

            if (ModelState.IsValid)
            {
                if (_context.Cidades.Any(c => c.Nome == cidade.Nome && c.Estado == cidade.Estado))
                {
                    ModelState.AddModelError("Nome inválido", "Já existe uma 'Cidade' com esse nome nesse Estado.");
                    return View(cidade);
                }
                _context.Add(cidade);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "AdminCidades");
            }
            return View();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cidade = await _context.Cidades.FindAsync(id);
            var estados = _context.Estados;
            cidade.NomeEstado = _context.Estados.FirstOrDefault(e => e.EstadoId == cidade.EstadoId).Nome;

            cidade.Estados = new List<SelectListItem>();
            if(cidade != null)
            {
                
                foreach (var item in estados)
                {
                    if (item.Nome != cidade.NomeEstado)
                    {
                        cidade.Estados.Add(new SelectListItem { Text = item.Sigla, Value = item.Nome });
                    }
                    else
                    {
                        cidade.Estados.Add(new SelectListItem { Text = item.Sigla, Value = item.Nome, Selected = true });
                    }
                }
            }
            else
            {
                return NotFound();
            }
            return View(cidade);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cidade cidade)
        {
            if (id != cidade.CidadeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cidade);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CidadeExists(cidade.CidadeId))
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
            return View(cidade);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cidade = await _context.Cidades.FirstOrDefaultAsync(m => m.CidadeId == id);
            cidade.Estado = _context.Estados.FirstOrDefault(e => e.EstadoId == cidade.EstadoId);
            if (cidade == null)
            {
                return NotFound();
            }

            return View(cidade);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var cidade = await _context.Cidades.FindAsync(id);
            var localidades = new List<Localidade>();
            if (cidade != null)
            {
                localidades = _context.Localidades.Where(l => l.Cidade == cidade.Nome).ToList();
                _context.Cidades.Remove(cidade);
            }
            await _context.SaveChangesAsync();

            RemoveLocalidades(localidades);

            return RedirectToAction(nameof(Index));
        }

        private bool CidadeExists(int id)
        {
            return _context.Cidades.Any(e => e.CidadeId == id);
        }

        private void RemoveLocalidades(List<Localidade> localidades)
        {
            _context.RemoveRange(localidades);
            _context.SaveChanges();
        }
    }
}

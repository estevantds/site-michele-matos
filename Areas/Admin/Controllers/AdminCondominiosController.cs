using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiMatos.Context;
using MiMatos.Models;

namespace MiMatos.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminCondominiosController : Controller
    {
        private readonly AppDbContext _context;

        public AdminCondominiosController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var condominios = _context.Condominios.ToList();
            return View(condominios);
        }

        public IActionResult Create()
        {
            var bairros = _context.Bairros.OrderBy(b => b.Nome).ToList();
            var cidades = _context.Cidades.ToList();
            var estados = _context.Estados.ToList();

            var condominio = new Condominio();
            condominio.Localidades = new List<SelectListItem>();

            if (bairros != null)
            {
                foreach (var item in bairros)
                {
                    var cidade = cidades.FirstOrDefault(c => c.CidadeId == item.CidadeId);
                    var estado = estados.FirstOrDefault(e => e.EstadoId == cidade.EstadoId);

                    condominio.Localidades.Add(new SelectListItem 
                    { 
                        Text = item.Nome + ", " + cidade.Nome + ", " + estado.Nome,
                        Value = item.Nome + ", " + cidade.Nome + ", " + estado.Nome
                    });
                }
            }

            return View(condominio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Condominio condominio)
        {
            condominio.CriadoEm = DateTime.Now;
            condominio.AtualizadoEm = DateTime.Now;

            if (ModelState.IsValid)
            {
                if(_context.Condominios.Any(c => c.Nome == condominio.Nome))
                {
                    ModelState.AddModelError("Inválido", "Já existe um condomínio com este nome.");
                    return View(condominio);
                }

                _context.Condominios.Add(condominio);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(condominio);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var cidades = _context.Cidades.ToList();
            var estados = _context.Estados.ToList();

            if (id == null)
            {
                return NotFound();
            }

            var condominio = await _context.Condominios.FindAsync(id);
            if (condominio == null)
            {
                return NotFound();
            }

            var bairros = _context.Bairros.ToList();

            condominio.Localidades = new List<SelectListItem>();

            if (bairros != null)
            {
                condominio.Localidades.Add(new SelectListItem { Text = condominio.Localidade, Value = condominio.Localidade, Selected = true });
                foreach (var bairro in bairros)
                {
                    var cidade = cidades.FirstOrDefault(c => c.CidadeId == bairro.CidadeId);
                    var estado = estados.FirstOrDefault(e => e.EstadoId == cidade.EstadoId);

                    var localidade = bairro.Nome + ", " + cidade.Nome + ", " + estado.Nome;

                    if (localidade != condominio.Localidade)
                    {
                        condominio.Localidades.Add(new SelectListItem 
                        { 
                            Text = localidade, 
                            Value = localidade
                        });
                    }
                }
            }

            return View(condominio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Condominio condominio)
        {
            if (id != condominio.CondominioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    condominio.AtualizadoEm = DateTime.Now;
                    _context.Update(condominio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CondominioExists(condominio.CondominioId))
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
            return View(condominio);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var condominio = await _context.Condominios.FirstOrDefaultAsync(c => c.CondominioId == id);
            if (condominio == null)
            {
                return NotFound();
            }

            return View(condominio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var condominio = await _context.Condominios.FindAsync(id);
            if (condominio != null)
            {
                _context.Condominios.Remove(condominio);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CondominioExists(int id)
        {
            return _context.Condominios.Any(e => e.CondominioId == id);
        }
    }
}

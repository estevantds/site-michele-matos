using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiMatos.Context;
using MiMatos.Models;
using ReflectionIT.Mvc.Paging;

namespace MiMatos.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminProprietariosController : Controller
    {
        private readonly AppDbContext _context;

        public AdminProprietariosController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string filter, int pageindex = 1, string sort = "Nome")
        {
            var resultado = _context.Proprietarios.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                resultado = resultado.Where(p => p.Nome.Contains(filter));
            }

            var proprietarios = await PagingList.CreateAsync(resultado, 5, pageindex, sort, "Nome");

            proprietarios.RouteValue = new RouteValueDictionary
            {
                { "filter", filter }
            };

            return View(proprietarios);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Proprietario proprietario)
        {
            if (ModelState.IsValid)
            {
                proprietario.CriadoEm = DateTime.Now;
                proprietario.AtualizadoEm = DateTime.Now;

                _context.Add(proprietario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(proprietario);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proprietario = await _context.Proprietarios.FindAsync(id);
            if (proprietario == null)
            {
                return NotFound();
            }
            proprietario.Nascimento = proprietario.Nascimento.Date;
            return View(proprietario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Proprietario proprietario)
        {
            if (id != proprietario.ProprietarioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    proprietario.AtualizadoEm = DateTime.Now;
                    _context.Update(proprietario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProprietarioExists(proprietario.ProprietarioId))
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
            return View(proprietario);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proprietario = await _context.Proprietarios
                .FirstOrDefaultAsync(m => m.ProprietarioId == id);
            if (proprietario == null)
            {
                return NotFound();
            }

            return View(proprietario);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var proprietario = await _context.Proprietarios.FindAsync(id);
            if (proprietario != null)
            {
                _context.Proprietarios.Remove(proprietario);
            } 
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProprietarioExists(int id)
        {
            return _context.Proprietarios.Any(e => e.ProprietarioId == id);
        }
    }
}

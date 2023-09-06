using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiMatos.Context;
using MiMatos.Models;

namespace MiMatos.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminTiposController : Controller
    {
        private readonly AppDbContext _context;

        public AdminTiposController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var tipos = _context.Tipos.ToList();
            return View(tipos);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tipo tipo)
        {
            if (ModelState.IsValid)
            {
                if(_context.Tipos.Any(t => t.Nome == tipo.Nome))
                {
                    ModelState.AddModelError("Nome inválido", "Já existe um 'Tipo' com esse nome.");
                    return View(tipo);
                }
                if (_context.Tipos.Any(t => t.Prefixo == tipo.Prefixo))
                {
                    ModelState.AddModelError("Prefixo inválido", "Já existe um 'Tipo' com esse prefixo.");
                    return View(tipo);
                }

                _context.Add(tipo);
                await _context.SaveChangesAsync();
                return RedirectToAction ("Index");
            }
            return View(tipo);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipo = await _context.Tipos.FindAsync(id);
            if (tipo == null)
            {
                return NotFound();
            }
            return View(tipo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Tipo tipo)
        {
            if (id != tipo.TipoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (_context.Tipos.Any(t => t.Nome == tipo.Nome && t.TipoId != tipo.TipoId))
                {
                    ModelState.AddModelError("Nome inválido", "Já existe um 'Tipo' com esse nome.");
                    return View(tipo);
                }
                if (_context.Tipos.Any(t => t.Prefixo == tipo.Prefixo && t.TipoId != tipo.TipoId))
                {
                    ModelState.AddModelError("Prefixo inválido", "Já existe um 'Tipo' com esse prefixo.");
                    return View(tipo);
                }

                try
                {
                    _context.Update(tipo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoExists(tipo.TipoId))
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
            return View(tipo);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipo = await _context.Tipos
                .FirstOrDefaultAsync(m => m.TipoId == id);
            if (tipo == null)
            {
                return NotFound();
            }

            return View(tipo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var tipo = await _context.Tipos.FindAsync(id);
            _context.Tipos.Remove(tipo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoExists(int id)
        {
            return _context.Tipos.Any(e => e.TipoId == id);
        }
    }
}

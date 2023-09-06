using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiMatos.Context;
using MiMatos.Models;

namespace MiMatos.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminEstadosController : Controller
    {
        private readonly AppDbContext _context;

        public AdminEstadosController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var estados = _context.Estados;
            return View(estados);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Estado estado)
        {
            if (ModelState.IsValid)
            {
                if (_context.Estados.Any(e => e.Nome == estado.Nome))
                {
                    ModelState.AddModelError("Nome inválido", "Já existe um 'Estado' com esse nome.");
                    return View(estado);
                }
                if (_context.Estados.Any(e => e.Sigla == estado.Sigla))
                {
                    ModelState.AddModelError("Nome inválido", "Já existe um 'Estado' com essa Sigla.");
                    return View(estado);
                }

                _context.Add(estado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estado = await _context.Estados.FindAsync(id);
            if (estado == null)
            {
                return NotFound();
            }
            return View(estado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Estado estado)
        {
            if (id != estado.EstadoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstadoExists(estado.EstadoId))
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
            return View(estado);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estado = await _context.Estados.FirstOrDefaultAsync(m => m.EstadoId == id);
            if (estado == null)
            {
                return NotFound();
            }

            return View(estado);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var estado = await _context.Estados.FindAsync(id);
            if(estado != null)
            {
                _context.Estados.Remove(estado);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool EstadoExists(int id)
        {
            return _context.Estados.Any(e => e.EstadoId == id);
        }
    }
}

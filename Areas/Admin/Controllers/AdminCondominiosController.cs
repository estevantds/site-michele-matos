﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiMatos.Context;
using MiMatos.Models;

namespace MiMatos.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
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
            var todasLocalidades = _context.Localidades.ToList();
            var localidadesApresentaveis = todasLocalidades.Where(tl => tl.Nome.Split(",").Length == 3).ToList();

            var condominio = new Condominio();
            condominio.Localidades = new List<Localidade>();

            if (localidadesApresentaveis != null)
            {
                foreach (var item in localidadesApresentaveis)
                {
                    condominio.Localidades.Add(item);
                }
            }

            return View(condominio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Condominio condominio)
        {
            var localidade = _context.Localidades.FirstOrDefault(l => l.Nome == condominio.Localidade);

            condominio.CriadoEm = DateTime.Now;
            condominio.AtualizadoEm = DateTime.Now;

            if (ModelState.IsValid)
            {
                if (_context.Condominios.Any(c => c.Nome == condominio.Nome) && localidade.Nome != null)
                {
                    ModelState.AddModelError("Inválido", "Já existe um condomínio com este nome nesta localidade.");

                    var todasLocalidades = _context.Localidades.ToList();
                    var localidadesApresentaveis = todasLocalidades.Where(tl => tl.Nome.Split(",").Length == 3).ToList();

                    condominio.Localidades = localidadesApresentaveis;

                    return View(condominio);
                }

                _context.Condominios.Add(condominio);
                _context.SaveChanges();

                localidade.Nome = condominio.Nome + ", " + condominio.Localidade;

                SaveLocalidade(localidade);

                return RedirectToAction("Index");
            }

            return View(condominio);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var condominio = await _context.Condominios.FindAsync(id);
            condominio.Localidades = new List<Localidade>();

            if (condominio == null)
            {
                return NotFound();
            }

            var todasLocalidades = _context.Localidades.ToList();

            var localidadesApresentaveis = todasLocalidades.Where(tl => tl.Nome.Split(",").Length == 3).ToList();

            if (localidadesApresentaveis != null)
            {
                foreach (var item in localidadesApresentaveis)
                {
                    condominio.Localidades.Add(item);
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
            var localidadeParaExcluir = condominio.Nome + ", " + condominio.Localidade;

            if (condominio != null)
            {
                _context.Condominios.Remove(condominio);
            }
            await _context.SaveChangesAsync();

            DeleteLocalidade(localidadeParaExcluir);

            return RedirectToAction(nameof(Index));
        }

        private bool CondominioExists(int id)
        {
            return _context.Condominios.Any(e => e.CondominioId == id);
        }

        private void SaveLocalidade(Localidade localidade)
        {
            try
            {
                localidade.LocalidadeId = 0;

                _context.Add(localidade);
                _context.SaveChanges();
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        private void DeleteLocalidade(string localidade)
        {
            var localidadeParaExcluir = _context.Localidades.First(l => l.Nome == localidade);

            _context.Remove(localidadeParaExcluir);

            _context.SaveChanges();
        }
    }
}

﻿using Microsoft.AspNetCore.Mvc;
using MiMatos.Context;
using MiMatos.Models;

namespace MiMatos.Controllers
{
    public class ClienteController : Controller
    {
        private readonly AppDbContext _context;

        public ClienteController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            cliente.CadastroSite = true;
            cliente.CriadoEm = DateTime.Now;
            cliente.AtualizadoEm = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Clientes.Add(cliente);

                await _context.SaveChangesAsync();
                return RedirectToAction("Success");
            }
            return View(cliente);
        }

        public IActionResult CreateProp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProp(Proprietario proprietario)
        {
            proprietario.CadastroSite = true;
            proprietario.CriadoEm = DateTime.Now;
            proprietario.AtualizadoEm = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Proprietarios.Add(proprietario);

                await _context.SaveChangesAsync();
                return RedirectToAction("Success");
            }
            return View(proprietario);
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}

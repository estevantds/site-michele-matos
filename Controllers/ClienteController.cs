using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Success()
        {
            return View();
        }
    }
}

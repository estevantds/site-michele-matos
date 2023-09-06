using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiMatos.Context;
using MiMatos.ViewModels;

namespace MiMatos.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var clientesNovos = _context.Clientes.Where(c => c.Visualizado == false).ToList();

            var clientesNovosVM = new ClienteViewModel
            {
                TemNovo = clientesNovos.Count > 0,
                Total = clientesNovos.Count,
            };

            return View(clientesNovosVM);
        }
    }
}

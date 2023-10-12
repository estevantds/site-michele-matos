using Microsoft.AspNetCore.Mvc;
using MiMatos.Repositories.Interfaces;

namespace MiMatos.Controllers
{
    public class PropriedadeController : Controller
    {
        private readonly IPropriedadeRepository _propriedadeRepository;

        public PropriedadeController(IPropriedadeRepository propriedadeRepository)
        {
            _propriedadeRepository = propriedadeRepository;
        }

        public IActionResult Propriedade(int? id)
        {
            var propriedade = _propriedadeRepository.GetPropriedadeById(id);
            return View(propriedade);
        }
    }
}

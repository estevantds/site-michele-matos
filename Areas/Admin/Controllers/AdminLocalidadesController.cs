using Microsoft.AspNetCore.Mvc;

namespace MiMatos.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminLocalidadesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

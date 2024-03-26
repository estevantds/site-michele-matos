using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MiMatos.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminLocalidadesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace Gotcha.Web.Areas.Player.Controllers
{
    [Area("Player")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["IsAlive"] = true;
            return View();
        }
    }
}

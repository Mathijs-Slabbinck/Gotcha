using Microsoft.AspNetCore.Mvc;

namespace Gotcha.Web.Areas.User.Controllers
{
    [Area("User")]
    public class GamesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

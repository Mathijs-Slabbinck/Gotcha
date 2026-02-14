using Microsoft.AspNetCore.Mvc;

namespace Gotcha.Web.Areas.User.Controllers
{
    [Area("User")]
    public class SettingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

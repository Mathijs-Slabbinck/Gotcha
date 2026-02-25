using Gotcha.Web.Areas.Player.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Gotcha.Web.Areas.Player.Controllers
{
    [Area("Player")]
    public class SettingsController : Controller
    {
        public IActionResult Index()
        {
            ViewData["IsAlive"] = true;
            ViewData["IsAdmin"] = true;

            PlayerSettingsViewModel playerSettingsViewModel = new PlayerSettingsViewModel
            {
                Username = "TheLegend27",
                ProfileImgSource = null
            };

            return View(playerSettingsViewModel);
        }
    }
}

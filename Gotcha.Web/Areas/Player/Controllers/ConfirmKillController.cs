using Gotcha.Core.Enums;
using Gotcha.Web.Areas.Player.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Gotcha.Web.Areas.Player.Controllers
{
    [Area("Player")]
    public class ConfirmKillController : Controller
    {
        public IActionResult Index()
        {
            ViewData["IsAlive"] = true;
            ViewData["IsAdmin"] = true;

            ConfirmKillViewModel confirmKillViewModel = new ConfirmKillViewModel
            {
                ShowPlayerImages = true,
                ShowGender = true,
                TargetName = "Jane Doe",
                TargetUsername = "TheLegend27",
                TargetGender = Genders.Female,
                Weapon = "Sock",
                IsAssassinMode = true,
                ShowHunter = true,
                HunterName = "Willy Wonka",
                HunterUsername = "Chocolate_Man",
                HunterGender = Genders.Male
            };

            return View(confirmKillViewModel);
        }
    }
}

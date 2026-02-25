using Microsoft.AspNetCore.Mvc;
using Gotcha.Core.Enums;
using Gotcha.Web.Areas.User.ViewModels;

namespace Gotcha.Web.Areas.User.Controllers
{
    [Area("User")]
    public class StoreController : Controller
    {
        public IActionResult Index()
        {
            var model = new StoreViewModel
            {
                // Mock: some features unlocked, some not
                AssassinUnlocked = true,
                ChaosUnlocked = false,
                TimedKillsUnlocked = true,
                CustomKillMethodsUnlocked = false,

                // Mock: user has Medium lobby size (100 players)
                CurrentLobbySize = MaxLobbySize.Medium,

                // Mock: user is on Standard plan
                CurrentPlan = Plan.Standard
            };

            return View(model);
        }
    }
}

using Gotcha.Core.Enums;
using Gotcha.Web.Areas.Player.ViewModels;
using Gotcha.Web.Areas.Player.ViewModels.BaseViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Gotcha.Web.Areas.Player.Controllers
{
    [Area("Player")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["IsAlive"] = true;
            ViewData["IsAdmin"] = true;

            List<KillBaseViewModel> kills = new List<KillBaseViewModel>()
            {
                new KillBaseViewModel
                {
                    VictimFullName = "Willy Wonka",
                    VictimUsername = "Chocolate_Man",
                    TimeStamp = DateTime.UtcNow,
                    Weapon = "Book"
                 },

                new KillBaseViewModel
                {
                    VictimFullName = "Wally Wanker",
                    VictimUsername = "Ben Dover",
                    TimeStamp = DateTime.UtcNow,
                    Weapon = "Table"
                }
            };

            HomeViewModel homeViewModel = new HomeViewModel
            {
                TargetName = "Jane Doe",
                TargetUsername = "TheLegend27",
                TargetGender = Genders.Female,
                PlayerGender = Genders.Male,
                Weapon = "Sock",
                AssignmentExpirationDate = DateTime.UtcNow.AddDays(5),
                StartDate = DateTime.UtcNow.AddDays(-18),
                EndDate = DateTime.UtcNow.AddDays(-2),
                WinnerName = "John Doe",
                WinnerOtherName = "TheLegend28",
                KillerName = "Willy Wonka",
                KillerOtherName = "Chocolate_Man",
                KilledOnDate = DateTime.UtcNow.AddDays(-6),
                Kills = kills,

            };

            return View(homeViewModel);
        }
    }
}

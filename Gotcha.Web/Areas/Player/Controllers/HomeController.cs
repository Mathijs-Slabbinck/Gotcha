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
                CustomRules = new List<string> { "No hiding in classrooms", "Safe zones: cafeteria during lunch", "Kills must have a witness" },
                ShowPlayerImages = true,
                ShowGender = true,
                IsAssassin = true,
                ShowHunter = true,
                HunterName = "Alice Brown",
                HunterOtherName = "ShadowHunter",
                HunterGender = Genders.Female,
                IsChaos = true,
                ChaosTimerMinHours = 6,
                ChaosTimerMaxHours = 12,
                IsTimed = true,
                TargetTimeOutHours = 24,
                ShowLivingPlayerCount = true,
                ShowLivingPlayerNames = true,
                IsSpectator = false,
                Players = new List<PlayerBaseViewModel>
                {
                    new PlayerBaseViewModel { Name = "Jane Doe", OtherName = "TheLegend27", IsAlive = true },
                    new PlayerBaseViewModel { Name = "John Smith", OtherName = "SneakySnake", IsAlive = true },
                    new PlayerBaseViewModel { Name = "Alice Brown", OtherName = "ShadowHunter", IsAlive = false },
                    new PlayerBaseViewModel { Name = "Bob Wilson", OtherName = "NerfKing", IsAlive = true },
                    new PlayerBaseViewModel { Name = "Charlie Green", OtherName = "StealthMode", IsAlive = false },
                },
            };

            return View(homeViewModel);
        }
    }
}

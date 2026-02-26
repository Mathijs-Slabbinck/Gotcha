using Gotcha.Web.Areas.User.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Gotcha.Web.Areas.User.Controllers
{
    [Area("User")]
    public class GamesController : Controller
    {
        public IActionResult Index()
        {
            List<GameItemViewModel> pendingGames = new List<GameItemViewModel>()
            {
                new GameItemViewModel
                {
                    GameId = Guid.NewGuid(),
                    Name = "Campus Clash",
                    CreatedDate = DateTime.UtcNow.AddDays(-2),
                    PlayerCount = 5
                },
                new GameItemViewModel
                {
                    GameId = Guid.NewGuid(),
                    Name = "Neighborhood Nerf War",
                    CreatedDate = DateTime.UtcNow.AddDays(-7),
                    PlayerCount = 3
                }
            };

            List<GameItemViewModel> activeGames = new List<GameItemViewModel>()
            {
                new GameItemViewModel
                {
                    GameId = Guid.NewGuid(),
                    Name = "Summer Showdown",
                    StartDate = DateTime.UtcNow.AddDays(-10),
                    PlayerCount = 12,
                    IsAlive = true
                },
                new GameItemViewModel
                {
                    GameId = Guid.NewGuid(),
                    Name = "Office Battle Royale",
                    StartDate = DateTime.UtcNow.AddDays(-3),
                    PlayerCount = 8,
                    IsAlive = false
                }
            };

            List<GameItemViewModel> endedGames = new List<GameItemViewModel>()
            {
                new GameItemViewModel
                {
                    GameId = Guid.NewGuid(),
                    Name = "Spring Frenzy",
                    StartDate = DateTime.UtcNow.AddDays(-45),
                    EndDate = DateTime.UtcNow.AddDays(-20),
                    PlayerCount = 15,
                    WinnerName = "TheLegend27"
                },
                new GameItemViewModel
                {
                    GameId = Guid.NewGuid(),
                    Name = "Winter Wars",
                    StartDate = DateTime.UtcNow.AddDays(-90),
                    EndDate = DateTime.UtcNow.AddDays(-60),
                    PlayerCount = 20,
                    WinnerName = "John Doe"
                }
            };

            GamesViewModel gamesViewModel = new GamesViewModel
            {
                PendingGames = pendingGames,
                ActiveGames = activeGames,
                EndedGames = endedGames
            };

            return View(gamesViewModel);
        }

        public IActionResult Create()
        {
            NewGameViewModel viewModel = new NewGameViewModel
            {
                Name = "",
                CustomRules = null,
                ShowPlayerImages = true,
                ShowGender = false,
                EnforcePlayerImages = false,
                ShowRealNames = true,
                ShowUsernames = false,
                ShowLivingPlayerCount = true,
                ShowLivingPlayerNames = false,
                ShowLivingPlayerNamesToDeath = false,
                IsAssassin = false,
                ShowHunter = false,
                IsChaos = false,
                ChaosTimerMinHours = 12,
                ChaosTimerMaxHours = 48,
                IsTimed = false,
                TargetTimeOutHours = 24,
                CustomKillMethods = false,
                KillMethods = null,
                AssassinModeUnlocked = true,
                ChaosModeUnlocked = true,
                TimedKillsUnlocked = true,
                InviteLink = "https://gotcha.app/join/abc123-mock-link"
            };

            return View(viewModel);
        }
    }
}

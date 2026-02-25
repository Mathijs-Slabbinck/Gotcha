using Gotcha.Web.Areas.User.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Gotcha.Web.Areas.User.Controllers
{
    [Area("User")]
    public class GamesController : Controller
    {
        public IActionResult Index()
        {
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
                ActiveGames = activeGames,
                EndedGames = endedGames
            };

            return View(gamesViewModel);
        }
    }
}

using Gotcha.Core.Data;
using Gotcha.Core.Entities.Models;
using Gotcha.Core.Services;
using Gotcha.Web.Areas.User.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gotcha.Web.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class GamesController : Controller
    {
        private readonly UserManager<GotchaUser> _userManager;
        private readonly GotchaDbContext _context;
        private readonly GameService _gameService;

        public GamesController(UserManager<GotchaUser> userManager, GotchaDbContext context, GameService gameService)
        {
            _userManager = userManager;
            _context = context;
            _gameService = gameService;
        }

        public async Task<IActionResult> Index()
        {
            GotchaUser? user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            var players = await _context.Players
                .Include(p => p.Game)
                    .ThenInclude(g => g.Players)
                        .ThenInclude(p2 => p2.User)
                .Include(p => p.Game)
                    .ThenInclude(g => g.Kills)
                .Where(p => p.UserId == user.Id)
                .ToListAsync();

            var pendingGames = new List<GameItemViewModel>();
            var activeGames = new List<GameItemViewModel>();
            var endedGames = new List<GameItemViewModel>();

            foreach (var player in players)
            {
                var game = player.Game;

                // Find winner name if game is finished
                string? winnerName = null;
                if (game.IsFinished && game.WinnerId.HasValue)
                {
                    var winner = game.Players.FirstOrDefault(p => p.Id == game.WinnerId.Value);
                    if (winner != null)
                    {
                        winnerName = $"{winner.User.FirstName} {winner.User.LastName}";
                    }
                }

                var item = new GameItemViewModel
                {
                    GameId = game.Id,
                    Name = game.Name,
                    CreatedDate = game.CreationDate,
                    StartDate = game.StartDate ?? DateTime.MinValue,
                    EndDate = game.EndDate,
                    PlayerCount = game.Players.Count,
                    WinnerName = winnerName,
                    IsAlive = player.IsAlive
                };

                if (!game.HasStarted && !game.IsFinished)
                {
                    pendingGames.Add(item);
                }
                else if (game.HasStarted && !game.IsFinished)
                {
                    activeGames.Add(item);
                }
                else if (game.IsFinished)
                {
                    endedGames.Add(item);
                }
            }

            var model = new GamesViewModel
            {
                PendingGames = pendingGames,
                ActiveGames = activeGames,
                EndedGames = endedGames
            };

            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            GotchaUser? user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            VipSettings? vip = await _context.VipSettings
                .FirstOrDefaultAsync(v => EF.Property<Guid>(v, "UserId") == user.Id);

            var viewModel = new NewGameViewModel
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
                AssassinModeUnlocked = vip?.AssassinModeUnlocked ?? false,
                ChaosModeUnlocked = vip?.ChaosModeUnlocked ?? false,
                TimedKillsUnlocked = vip?.TimedKillsUnlocked ?? false,
                InviteLink = ""
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewGameViewModel model)
        {
            GotchaUser? user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            // Load VipSettings for JoinPlayer (needs it for MaxLobbySize)
            var fullUser = await _context.Users
                .Include(u => u.VipSettings)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            if (fullUser == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            var rules = new Rules
            {
                ShowPlayerImages = model.ShowPlayerImages,
                ShowGender = model.ShowGender,
                EnforcePlayerImages = model.EnforcePlayerImages,
                ShowRealNames = model.ShowRealNames,
                ShowUsernames = model.ShowUsernames,
                ShowLivingPlayerCount = model.ShowLivingPlayerCount,
                ShowLivingPlayerNames = model.ShowLivingPlayerNames,
                ShowLivingPlayerNamesToDeath = model.ShowLivingPlayerNamesToDeath,
                IsAssassin = model.IsAssassin,
                ShowHunter = model.ShowHunter,
                IsChaos = model.IsChaos,
                ChaosTimerMin = TimeSpan.FromHours(model.ChaosTimerMinHours),
                ChaosTimerMax = TimeSpan.FromHours(model.ChaosTimerMaxHours),
                IsTimed = model.IsTimed,
                TargetTimeOut = TimeSpan.FromHours(model.TargetTimeOutHours),
                CustomKillMethods = model.CustomKillMethods,
                CustomRules = model.CustomRules,
                KillMethods = model.KillMethods
            };

            var game = new Game
            {
                Name = string.IsNullOrWhiteSpace(model.Name) ? "New game" : model.Name,
                Rules = rules
            };

            var player = new Gotcha.Core.Entities.Models.Player
            {
                UserId = fullUser.Id,
                User = fullUser,
                GameId = game.Id,
                Game = game
            };

            _gameService.JoinPlayer(game, player);

            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}

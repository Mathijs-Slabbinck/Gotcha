using Gotcha.Core.Data;
using Gotcha.Core.Entities.Models;
using Gotcha.Core.Services;
using Gotcha.Web.Areas.Player.ViewModels;
using Gotcha.Web.Areas.Player.ViewModels.BaseViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gotcha.Web.Areas.Player.Controllers
{
    [Area("Player")]
    [Authorize]
    public class AdminController : Controller
    {
        private readonly UserManager<GotchaUser> _userManager;
        private readonly GotchaDbContext _context;
        private readonly GameService _gameService;

        public AdminController(UserManager<GotchaUser> userManager, GotchaDbContext context, GameService gameService)
        {
            _userManager = userManager;
            _context = context;
            _gameService = gameService;
        }

        public async Task<IActionResult> Index(Guid id)
        {
            GotchaUser? user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            var player = await _context.Players
                .Include(p => p.User)
                .Include(p => p.Game)
                    .ThenInclude(g => g.Rules)
                .Include(p => p.Game)
                    .ThenInclude(g => g.Players)
                        .ThenInclude(p2 => p2.User)
                .Include(p => p.Game)
                    .ThenInclude(g => g.Kills)
                        .ThenInclude(k => k.Killer)
                .Include(p => p.Game)
                    .ThenInclude(g => g.Kills)
                        .ThenInclude(k => k.Victim)
                .FirstOrDefaultAsync(p => p.UserId == user.Id && p.GameId == id);

            if (player == null)
            {
                return RedirectToAction("Index", "Games", new { area = "User" });
            }

            var game = player.Game;

            // Verify this player is an admin
            if (!game.AdminIds.Contains(player.Id))
            {
                return RedirectToAction("Index", "Home", new { id });
            }

            ViewData["IsAlive"] = player.IsAlive;
            ViewData["IsAdmin"] = true;
            ViewData["GameId"] = id;

            // Get VipSettings for the game creator
            var creatorVip = await _context.VipSettings
                .FirstOrDefaultAsync(v => EF.Property<Guid>(v, "UserId") == game.CreatorId);

            var rules = game.Rules;

            var adminPlayers = game.Players.Select(p => new AdminPlayerViewModel
            {
                PlayerId = p.Id,
                Name = $"{p.User.FirstName} {p.User.LastName}",
                Username = p.UserName ?? p.User.UserName,
                HasImage = p.ProfileImageSource != null,
                IsAdmin = game.AdminIds.Contains(p.Id),
                IsSpectator = p.IsSpectator
            }).ToList();

            // Pending kills = kills that are not yet validated (IsValid = false)
            var pendingKills = game.Kills
                .Where(k => !k.IsValid)
                .Select(k => new AdminKillViewModel
                {
                    KillId = k.Id,
                    KillerName = k.Killer.UserName ?? $"{k.Killer.User.FirstName} {k.Killer.User.LastName}",
                    VictimName = k.Victim.UserName ?? $"{k.Victim.User.FirstName} {k.Victim.User.LastName}",
                    Weapon = k.Weapon,
                    Moment = k.Moment
                }).ToList();

            var model = new AdminViewModel
            {
                HasStarted = game.HasStarted,
                GameName = game.Name,
                InviteLink = $"gotcha://join/{game.Id}",
                PlayerCount = game.Players.Count,
                MaxPlayers = game.MaxPlayers,
                Players = adminPlayers,
                PendingKills = pendingKills,

                ShowPlayerImages = rules.ShowPlayerImages,
                ShowGender = rules.ShowGender,
                EnforcePlayerImages = rules.EnforcePlayerImages,
                ShowRealNames = rules.ShowRealNames,
                ShowUsernames = rules.ShowUsernames,
                ShowLivingPlayerCount = rules.ShowLivingPlayerCount,
                ShowLivingPlayerNames = rules.ShowLivingPlayerNames,
                ShowLivingPlayerNamesToDeath = rules.ShowLivingPlayerNamesToDeath,
                CustomKillMethods = rules.CustomKillMethods,
                CustomRules = rules.CustomRules?.ToList(),
                KillMethods = rules.KillMethods?.ToList(),

                AssassinModeUnlocked = creatorVip?.AssassinModeUnlocked ?? false,
                IsAssassin = rules.IsAssassin,
                ShowHunter = rules.ShowHunter,
                ChaosModeUnlocked = creatorVip?.ChaosModeUnlocked ?? false,
                IsChaos = rules.IsChaos,
                ChaosTimerMinHours = (int)rules.ChaosTimerMin.TotalHours,
                ChaosTimerMaxHours = (int)rules.ChaosTimerMax.TotalHours,
                TimedKillsUnlocked = creatorVip?.TimedKillsUnlocked ?? false,
                IsTimed = rules.IsTimed,
                TargetTimeOutHours = (int)rules.TargetTimeOut.TotalHours
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartGame(Guid id)
        {
            var game = await LoadFullGame(id);
            if (game == null)
            {
                return RedirectToAction("Index", "Games", new { area = "User" });
            }

            GotchaUser? user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            // Verify admin
            var player = game.Players.FirstOrDefault(p => p.UserId == user.Id);
            if (player == null || !game.AdminIds.Contains(player.Id))
            {
                return RedirectToAction("Index", "Home", new { id });
            }

            _gameService.StartGame(game);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemovePlayer(Guid id, Guid playerId)
        {
            var game = await LoadFullGame(id);
            if (game == null)
            {
                return RedirectToAction("Index", "Games", new { area = "User" });
            }

            GotchaUser? user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            // Verify admin
            var adminPlayer = game.Players.FirstOrDefault(p => p.UserId == user.Id);
            if (adminPlayer == null || !game.AdminIds.Contains(adminPlayer.Id))
            {
                return RedirectToAction("Index", "Home", new { id });
            }

            var playerToRemove = game.Players.FirstOrDefault(p => p.Id == playerId);
            if (playerToRemove != null)
            {
                game.Players.Remove(playerToRemove);
                game.AdminIds.Remove(playerId);
                _context.Players.Remove(playerToRemove);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleAdmin(Guid id, Guid playerId)
        {
            var game = await LoadFullGame(id);
            if (game == null)
            {
                return RedirectToAction("Index", "Games", new { area = "User" });
            }

            GotchaUser? user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            // Verify admin
            var adminPlayer = game.Players.FirstOrDefault(p => p.UserId == user.Id);
            if (adminPlayer == null || !game.AdminIds.Contains(adminPlayer.Id))
            {
                return RedirectToAction("Index", "Home", new { id });
            }

            if (game.AdminIds.Contains(playerId))
            {
                game.AdminIds.Remove(playerId);
            }
            else
            {
                game.AdminIds.Add(playerId);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ValidateKill(Guid id, Guid killId)
        {
            var game = await LoadFullGame(id);
            if (game == null)
            {
                return RedirectToAction("Index", "Games", new { area = "User" });
            }

            GotchaUser? user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            // Verify admin
            var adminPlayer = game.Players.FirstOrDefault(p => p.UserId == user.Id);
            if (adminPlayer == null || !game.AdminIds.Contains(adminPlayer.Id))
            {
                return RedirectToAction("Index", "Home", new { id });
            }

            var kill = game.Kills.FirstOrDefault(k => k.Id == killId);
            if (kill == null)
            {
                return RedirectToAction("Index", new { id });
            }

            var killer = game.Players.FirstOrDefault(p => p.Id == kill.KillerId);
            var victim = game.Players.FirstOrDefault(p => p.Id == kill.VictimId);
            if (killer == null || victim == null)
            {
                return RedirectToAction("Index", new { id });
            }

            _gameService.HandleValidKill(game, killer, victim, kill.Weapon);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectKill(Guid id, Guid killId)
        {
            var game = await LoadFullGame(id);
            if (game == null)
            {
                return RedirectToAction("Index", "Games", new { area = "User" });
            }

            GotchaUser? user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            // Verify admin
            var adminPlayer = game.Players.FirstOrDefault(p => p.UserId == user.Id);
            if (adminPlayer == null || !game.AdminIds.Contains(adminPlayer.Id))
            {
                return RedirectToAction("Index", "Home", new { id });
            }

            var kill = game.Kills.FirstOrDefault(k => k.Id == killId);
            if (kill == null)
            {
                return RedirectToAction("Index", new { id });
            }

            var killer = game.Players.FirstOrDefault(p => p.Id == kill.KillerId);
            var victim = game.Players.FirstOrDefault(p => p.Id == kill.VictimId);
            if (killer == null || victim == null)
            {
                return RedirectToAction("Index", new { id });
            }

            _gameService.HandleInValidKill(game, killer, victim, weapon: kill.Weapon);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSettings(Guid id, AdminViewModel model)
        {
            GotchaUser? user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            var player = await _context.Players
                .Include(p => p.Game)
                    .ThenInclude(g => g.Rules)
                .FirstOrDefaultAsync(p => p.UserId == user.Id && p.GameId == id);

            if (player == null)
            {
                return RedirectToAction("Index", "Games", new { area = "User" });
            }

            var game = player.Game;

            // Verify admin
            if (!game.AdminIds.Contains(player.Id))
            {
                return RedirectToAction("Index", "Home", new { id });
            }

            var rules = game.Rules;

            rules.ShowPlayerImages = model.ShowPlayerImages;
            rules.ShowGender = model.ShowGender;
            rules.EnforcePlayerImages = model.EnforcePlayerImages;
            rules.ShowRealNames = model.ShowRealNames;
            rules.ShowUsernames = model.ShowUsernames;
            rules.ShowLivingPlayerCount = model.ShowLivingPlayerCount;
            rules.ShowLivingPlayerNames = model.ShowLivingPlayerNames;
            rules.ShowLivingPlayerNamesToDeath = model.ShowLivingPlayerNamesToDeath;
            rules.IsAssassin = model.IsAssassin;
            rules.ShowHunter = model.ShowHunter;
            rules.IsChaos = model.IsChaos;
            rules.ChaosTimerMin = TimeSpan.FromHours(model.ChaosTimerMinHours);
            rules.ChaosTimerMax = TimeSpan.FromHours(model.ChaosTimerMaxHours);
            rules.IsTimed = model.IsTimed;
            rules.TargetTimeOut = TimeSpan.FromHours(model.TargetTimeOutHours);
            rules.CustomKillMethods = model.CustomKillMethods;
            rules.CustomRules = model.CustomRules;
            rules.KillMethods = model.KillMethods;

            if (!string.IsNullOrWhiteSpace(model.GameName))
            {
                game.Name = model.GameName;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { id });
        }

        private async Task<Game?> LoadFullGame(Guid gameId)
        {
            return await _context.Games
                .Include(g => g.Rules)
                .Include(g => g.Players)
                    .ThenInclude(p => p.User)
                .Include(g => g.Players)
                    .ThenInclude(p => p.TargetAssignments)
                        .ThenInclude(ta => ta.Target)
                .Include(g => g.Players)
                    .ThenInclude(p => p.TargetAssignments)
                        .ThenInclude(ta => ta.Kill)
                .Include(g => g.Kills)
                    .ThenInclude(k => k.Killer)
                .Include(g => g.Kills)
                    .ThenInclude(k => k.Victim)
                .FirstOrDefaultAsync(g => g.Id == gameId);
        }
    }
}

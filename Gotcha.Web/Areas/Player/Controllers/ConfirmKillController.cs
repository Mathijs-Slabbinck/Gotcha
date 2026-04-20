using Gotcha.Core.Data;
using Gotcha.Core.Entities.Models;
using Gotcha.Core.Enums;
using Gotcha.Core.Services;
using Gotcha.Web.Areas.Player.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gotcha.Web.Areas.Player.Controllers
{
    [Area("Player")]
    [Authorize]
    public class ConfirmKillController : Controller
    {
        private readonly UserManager<GotchaUser> _userManager;
        private readonly GotchaDbContext _context;
        private readonly GameService _gameService;

        public ConfirmKillController(UserManager<GotchaUser> userManager, GotchaDbContext context, GameService gameService)
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
                .Include(p => p.Game)
                    .ThenInclude(g => g.Rules)
                .Include(p => p.TargetAssignments)
                    .ThenInclude(ta => ta.Target)
                        .ThenInclude(t => t.User)
                .FirstOrDefaultAsync(p => p.UserId == user.Id && p.GameId == id);

            if (player == null)
            {
                return RedirectToAction("Index", "Games", new { area = "User" });
            }

            var rules = player.Game.Rules;

            ViewData["IsAlive"] = player.IsAlive;
            ViewData["IsAdmin"] = player.Game.AdminIds.Contains(player.Id);
            ViewData["GameId"] = id;

            // Current target assignment
            var currentAssignment = player.TargetAssignments
                .FirstOrDefault(ta => ta.AssignmentStatus == AssignmentStatus.Ongoing);

            // Who is hunting this player
            var hunterAssignment = await _context.TargetAssignments
                .Include(ta => ta.Hunter)
                    .ThenInclude(h => h.User)
                .FirstOrDefaultAsync(ta => ta.TargetId == player.Id && ta.AssignmentStatus == AssignmentStatus.Ongoing);

            var model = new ConfirmKillViewModel
            {
                ShowPlayerImages = rules.ShowPlayerImages,
                ShowGender = rules.ShowGender,
                TargetName = currentAssignment != null
                    ? $"{currentAssignment.Target.User.FirstName} {currentAssignment.Target.User.LastName}"
                    : null,
                TargetUsername = currentAssignment?.Target.UserName ?? currentAssignment?.Target.User.UserName,
                TargetGender = currentAssignment?.Target.User.Gender,
                TargetProfileImgSource = currentAssignment?.Target.ProfileImageSource,
                Weapon = currentAssignment?.Weapon,
                IsAssassinMode = rules.IsAssassin,
                ShowHunter = rules.ShowHunter,
                HunterName = hunterAssignment != null
                    ? $"{hunterAssignment.Hunter.User.FirstName} {hunterAssignment.Hunter.User.LastName}"
                    : null,
                HunterUsername = hunterAssignment?.Hunter.UserName ?? hunterAssignment?.Hunter.User.UserName,
                HunterGender = hunterAssignment?.Hunter.User.Gender,
                HunterProfileImgSource = hunterAssignment?.Hunter.ProfileImageSource
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmKill(Guid id)
        {
            GotchaUser? user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            var player = await LoadFullPlayer(user.Id, id);
            if (player == null)
            {
                return RedirectToAction("Index", "Games", new { area = "User" });
            }

            var game = player.Game;

            // Find the target from the ongoing assignment
            var currentAssignment = player.TargetAssignments
                .FirstOrDefault(ta => ta.AssignmentStatus == AssignmentStatus.Ongoing);

            if (currentAssignment == null)
            {
                return RedirectToAction("Index", new { id });
            }

            var victim = game.Players.FirstOrDefault(p => p.Id == currentAssignment.TargetId);
            if (victim == null)
            {
                return RedirectToAction("Index", new { id });
            }

            _gameService.HandleValidKill(game, player, victim, currentAssignment.Weapon);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectKill(Guid id)
        {
            GotchaUser? user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            var player = await LoadFullPlayer(user.Id, id);
            if (player == null)
            {
                return RedirectToAction("Index", "Games", new { area = "User" });
            }

            var game = player.Game;

            // Find the target from the ongoing assignment
            var currentAssignment = player.TargetAssignments
                .FirstOrDefault(ta => ta.AssignmentStatus == AssignmentStatus.Ongoing);

            if (currentAssignment == null)
            {
                return RedirectToAction("Index", new { id });
            }

            var victim = game.Players.FirstOrDefault(p => p.Id == currentAssignment.TargetId);
            if (victim == null)
            {
                return RedirectToAction("Index", new { id });
            }

            _gameService.HandleInValidKill(game, player, victim, weapon: currentAssignment.Weapon);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmHunterKill(Guid id)
        {
            GotchaUser? user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            var player = await LoadFullPlayer(user.Id, id);
            if (player == null)
            {
                return RedirectToAction("Index", "Games", new { area = "User" });
            }

            var game = player.Game;

            // In assassin mode, the player kills their hunter
            var hunterAssignment = game.Players
                .SelectMany(p => p.TargetAssignments)
                .FirstOrDefault(ta => ta.TargetId == player.Id && ta.AssignmentStatus == AssignmentStatus.Ongoing);

            if (hunterAssignment == null)
            {
                return RedirectToAction("Index", new { id });
            }

            var hunter = game.Players.FirstOrDefault(p => p.Id == hunterAssignment.HunterId);
            if (hunter == null)
            {
                return RedirectToAction("Index", new { id });
            }

            // Player kills their hunter (player is the killer, hunter is the victim)
            _gameService.HandleValidKill(game, player, hunter, hunterAssignment.Weapon);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home", new { id });
        }

        private async Task<Gotcha.Core.Entities.Models.Player?> LoadFullPlayer(Guid userId, Guid gameId)
        {
            return await _context.Players
                .Include(p => p.User)
                .Include(p => p.Game)
                    .ThenInclude(g => g.Rules)
                .Include(p => p.Game)
                    .ThenInclude(g => g.Players)
                        .ThenInclude(p2 => p2.User)
                .Include(p => p.Game)
                    .ThenInclude(g => g.Players)
                        .ThenInclude(p2 => p2.TargetAssignments)
                            .ThenInclude(ta => ta.Target)
                .Include(p => p.Game)
                    .ThenInclude(g => g.Players)
                        .ThenInclude(p2 => p2.TargetAssignments)
                            .ThenInclude(ta => ta.Kill)
                .Include(p => p.Game)
                    .ThenInclude(g => g.Kills)
                .Include(p => p.TargetAssignments)
                    .ThenInclude(ta => ta.Target)
                .FirstOrDefaultAsync(p => p.UserId == userId && p.GameId == gameId);
        }
    }
}

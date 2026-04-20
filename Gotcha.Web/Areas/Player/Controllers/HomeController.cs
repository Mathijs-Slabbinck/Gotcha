using Gotcha.Core.Data;
using Gotcha.Core.Entities.Models;
using Gotcha.Core.Enums;
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
    public class HomeController : Controller
    {
        private readonly UserManager<GotchaUser> _userManager;
        private readonly GotchaDbContext _context;

        public HomeController(UserManager<GotchaUser> userManager, GotchaDbContext context)
        {
            _userManager = userManager;
            _context = context;
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
                            .ThenInclude(killer => killer.User)
                .Include(p => p.Game)
                    .ThenInclude(g => g.Kills)
                        .ThenInclude(k => k.Victim)
                            .ThenInclude(victim => victim.User)
                .Include(p => p.TargetAssignments)
                    .ThenInclude(ta => ta.Target)
                        .ThenInclude(t => t.User)
                .FirstOrDefaultAsync(p => p.UserId == user.Id && p.GameId == id);

            if (player == null)
            {
                return RedirectToAction("Index", "Games", new { area = "User" });
            }

            var game = player.Game;
            var rules = game.Rules;

            ViewData["IsAlive"] = player.IsAlive;
            ViewData["IsAdmin"] = game.AdminIds.Contains(player.Id);

            // Find current target assignment
            var currentAssignment = player.TargetAssignments
                .FirstOrDefault(ta => ta.AssignmentStatus == AssignmentStatus.Ongoing);

            // Find who is hunting this player
            var hunterAssignment = await _context.TargetAssignments
                .Include(ta => ta.Hunter)
                    .ThenInclude(h => h.User)
                .FirstOrDefaultAsync(ta => ta.TargetId == player.Id && ta.AssignmentStatus == AssignmentStatus.Ongoing);

            // Find if this player was killed
            var deathKill = game.Kills
                .FirstOrDefault(k => k.VictimId == player.Id && k.IsValid);

            // Winner info
            string? winnerName = null;
            string? winnerOtherName = null;
            if (game.IsFinished && game.WinnerId.HasValue)
            {
                var winner = game.Players.FirstOrDefault(p => p.Id == game.WinnerId.Value);
                if (winner != null)
                {
                    winnerName = $"{winner.User.FirstName} {winner.User.LastName}";
                    winnerOtherName = winner.UserName ?? winner.User.UserName;
                }
            }

            // Kills list (kills this player made)
            var kills = game.Kills
                .Where(k => k.KillerId == player.Id && k.IsValid)
                .OrderByDescending(k => k.Moment)
                .Select(k => new KillBaseViewModel
                {
                    VictimFullName = $"{k.Victim.User.FirstName} {k.Victim.User.LastName}",
                    VictimUsername = k.Victim.UserName ?? k.Victim.User.UserName,
                    Weapon = k.Weapon,
                    TimeStamp = k.Moment
                }).ToList();

            // Players list
            var players = game.Players.Select(p => new PlayerBaseViewModel
            {
                Name = $"{p.User.FirstName} {p.User.LastName}",
                OtherName = p.UserName ?? p.User.UserName,
                IsAlive = p.IsAlive
            }).ToList();

            var homeViewModel = new HomeViewModel
            {
                TargetName = currentAssignment != null
                    ? $"{currentAssignment.Target.User.FirstName} {currentAssignment.Target.User.LastName}"
                    : null,
                TargetUsername = currentAssignment?.Target.UserName ?? currentAssignment?.Target.User.UserName,
                TargetGender = currentAssignment?.Target.User.Gender,
                TargetProfileImgSource = currentAssignment?.Target.ProfileImageSource,
                PlayerGender = player.User.Gender,
                PlayerProfileImgSource = player.ProfileImageSource,
                Weapon = currentAssignment?.Weapon,
                AssignmentExpirationDate = currentAssignment?.AssignmentExpirationDate,
                StartDate = game.StartDate ?? DateTime.MinValue,
                EndDate = game.EndDate,
                WinnerName = winnerName,
                WinnerOtherName = winnerOtherName,
                KilledOnDate = deathKill?.Moment,
                KillerName = deathKill != null
                    ? $"{deathKill.Killer.User.FirstName} {deathKill.Killer.User.LastName}"
                    : null,
                KillerOtherName = deathKill != null
                    ? (deathKill.Killer.UserName ?? deathKill.Killer.User.UserName)
                    : null,
                Kills = kills,
                CustomRules = rules.CustomRules?.ToList(),
                ShowPlayerImages = rules.ShowPlayerImages,
                ShowGender = rules.ShowGender,
                IsAssassin = rules.IsAssassin,
                ShowHunter = rules.ShowHunter,
                HunterName = hunterAssignment != null
                    ? $"{hunterAssignment.Hunter.User.FirstName} {hunterAssignment.Hunter.User.LastName}"
                    : null,
                HunterOtherName = hunterAssignment != null
                    ? (hunterAssignment.Hunter.UserName ?? hunterAssignment.Hunter.User.UserName)
                    : null,
                HunterGender = hunterAssignment?.Hunter.User.Gender,
                HunterProfileImgSource = hunterAssignment?.Hunter.ProfileImageSource,
                IsChaos = rules.IsChaos,
                ChaosTimerMinHours = (int)rules.ChaosTimerMin.TotalHours,
                ChaosTimerMaxHours = (int)rules.ChaosTimerMax.TotalHours,
                IsTimed = rules.IsTimed,
                TargetTimeOutHours = (int)rules.TargetTimeOut.TotalHours,
                ShowLivingPlayerCount = rules.ShowLivingPlayerCount,
                ShowLivingPlayerNames = rules.ShowLivingPlayerNames,
                IsSpectator = player.IsSpectator,
                Players = players
            };

            return View(homeViewModel);
        }
    }
}

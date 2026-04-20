using Gotcha.Core.Data;
using Gotcha.Core.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gotcha.Web.Areas.User.Controllers
{
    [Area("User")]
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

        public async Task<IActionResult> Index()
        {
            GotchaUser? user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            var fullUser = await _context.Users
                .Include(u => u.PlayerAccounts)
                    .ThenInclude(p => p.Game)
                        .ThenInclude(g => g.Kills)
                .Include(u => u.ProfileImage)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            if (fullUser == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            int gamesPlayed = fullUser.PlayerAccounts.Count;
            int gamesWon = fullUser.PlayerAccounts.Count(p => p.Game.WinnerId == p.Id);

            var playerIds = fullUser.PlayerAccounts.Select(p => p.Id).ToHashSet();
            int totalKills = fullUser.PlayerAccounts
                .SelectMany(p => p.Game.Kills)
                .Count(k => playerIds.Contains(k.KillerId) && k.IsValid);

            int biggestStreak = 0;
            foreach (Gotcha.Core.Entities.Models.Player player in fullUser.PlayerAccounts)
            {
                int killCount = player.Game.Kills
                    .Count(k => k.KillerId == player.Id && k.IsValid);

                biggestStreak = Math.Max(biggestStreak, killCount);
            }

            // Build profile image source
            string profileImageSource = "~/images/male-placeholder.png";
            if (fullUser.ProfileImage != null)
            {
                string base64 = Convert.ToBase64String(fullUser.ProfileImage.ImageData);
                profileImageSource = $"data:{fullUser.ProfileImage.MimeType};base64,{base64}";
            }

            ViewBag.UserName = fullUser.UserName;
            ViewBag.GamesPlayed = gamesPlayed;
            ViewBag.GamesWon = gamesWon;
            ViewBag.TotalKills = totalKills;
            ViewBag.BiggestKillStreak = biggestStreak;
            ViewBag.AccountCreated = fullUser.AccountCreationDate.ToString("dd-MM-yyyy");
            ViewBag.ProfileImageSource = profileImageSource;

            return View();
        }
    }
}

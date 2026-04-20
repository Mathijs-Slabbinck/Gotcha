using Gotcha.Core.Data;
using Gotcha.Core.Entities.Models;
using Gotcha.Web.Areas.Player.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gotcha.Web.Areas.Player.Controllers
{
    [Area("Player")]
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly UserManager<GotchaUser> _userManager;
        private readonly GotchaDbContext _context;

        public SettingsController(UserManager<GotchaUser> userManager, GotchaDbContext context)
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
                .Include(p => p.Game)
                .FirstOrDefaultAsync(p => p.UserId == user.Id && p.GameId == id);

            if (player == null)
            {
                return RedirectToAction("Index", "Games", new { area = "User" });
            }

            ViewData["IsAlive"] = player.IsAlive;
            ViewData["IsAdmin"] = player.Game.AdminIds.Contains(player.Id);
            ViewData["GameId"] = id;

            var model = new PlayerSettingsViewModel
            {
                Username = player.UserName ?? user.UserName,
                ProfileImgSource = player.ProfileImageSource
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Guid id, PlayerSettingsViewModel model)
        {
            GotchaUser? user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            var player = await _context.Players
                .Include(p => p.Game)
                .FirstOrDefaultAsync(p => p.UserId == user.Id && p.GameId == id);

            if (player == null)
            {
                return RedirectToAction("Index", "Games", new { area = "User" });
            }

            player.UserName = model.Username;
            player.ProfileImageSource = model.ProfileImgSource;

            await _context.SaveChangesAsync();

            ViewData["IsAlive"] = player.IsAlive;
            ViewData["IsAdmin"] = player.Game.AdminIds.Contains(player.Id);
            ViewData["GameId"] = id;

            TempData["SuccessMessage"] = "Settings saved successfully.";
            return RedirectToAction("Index", new { id });
        }
    }
}

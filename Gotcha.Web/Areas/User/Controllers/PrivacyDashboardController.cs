using System.Text;
using System.Text.Json;
using Gotcha.Core.Entities.Models;
using Gotcha.Core.Services.AccountPrivacy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gotcha.Web.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class PrivacyDashboardController : Controller
    {
        private readonly UserManager<GotchaUser> _userManager;
        private readonly SignInManager<GotchaUser> _signInManager;
        private readonly AccountPrivacyService _accountPrivacy;

        public PrivacyDashboardController(
            UserManager<GotchaUser> userManager,
            SignInManager<GotchaUser> signInManager,
            AccountPrivacyService accountPrivacy)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _accountPrivacy = accountPrivacy;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> DownloadData()
        {
            GotchaUser? user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            UserDataExport? export = await _accountPrivacy.BuildExportAsync(user.Id);

            if (export == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            JsonSerializerOptions jsonOptions = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(export, jsonOptions);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            string fileName = $"gotcha-data-{DateTime.UtcNow:yyyyMMdd}.json";

            return File(bytes, "application/json", fileName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccount()
        {
            GotchaUser? user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            await _accountPrivacy.SoftDeleteAndAnonymizeAsync(user.Id);
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}

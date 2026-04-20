using Gotcha.Core.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gotcha.Web.Controllers
{
    public class AccountsController : Controller
    {
        private readonly SignInManager<GotchaUser> _signInManager;
        private readonly UserManager<GotchaUser> _userManager;

        public AccountsController(SignInManager<GotchaUser> signInManager, UserManager<GotchaUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(Guid userId, string token)
        {
            GotchaUser? user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return View("EmailConfirmationFailed");
            }

            IdentityResult result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return View("EmailConfirmed");
            }

            return View("EmailConfirmationFailed");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmGuardianConsent(Guid userId, string token)
        {
            GotchaUser? user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null || user.GuardianConsentToken != token)
            {
                return View("GuardianConsentFailed");
            }

            user.HasGuardianConsent = true;
            user.GuardianConsentDate = DateTime.UtcNow;
            user.GuardianConsentToken = null;
            await _userManager.UpdateAsync(user);

            ViewBag.UserName = user.FirstName;
            return View("GuardianConsentConfirmed");
        }
    }
}

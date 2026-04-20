using Gotcha.Core.Entities.Models;
using Gotcha.Core.Interfaces;
using Gotcha.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gotcha.Web.Controllers
{
    public class ResetPasswordController : Controller
    {
        private readonly UserManager<GotchaUser> _userManager;
        private readonly IEmailService _emailService;

        public ResetPasswordController(UserManager<GotchaUser> userManager, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        // Step 1: show "enter your email" form
        public IActionResult Index()
        {
            return View();
        }

        // Step 2: send reset link
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendResetLink(ForgotPasswordViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", viewModel);
            }

            GotchaUser? user = await _userManager.FindByEmailAsync(viewModel.Email);

            // Always redirect to CheckEmail — don't reveal whether the email exists
            if (user != null && !user.IsDeleted)
            {
                string token = await _userManager.GeneratePasswordResetTokenAsync(user);

                string? resetUrl = Url.Action(
                    "Reset",
                    "ResetPassword",
                    new { userId = user.Id, token = token },
                    Request.Scheme
                );

                await _emailService.SendEmailAsync(
                    user.Email,
                    "Reset your Gotcha password",
                    $"<h2>Password Reset</h2><p>Click the link below to reset your password:</p><p><a href=\"{resetUrl}\">Reset Password</a></p><p>If you did not request this, you can safely ignore this email.</p>"
                );
            }

            return RedirectToAction("CheckEmail");
        }

        // Step 2b: confirmation page
        public IActionResult CheckEmail()
        {
            return View();
        }

        // Step 3: show "enter new password" form
        [HttpGet]
        public IActionResult Reset(Guid userId, string token)
        {
            if (userId == Guid.Empty || string.IsNullOrWhiteSpace(token))
            {
                return RedirectToAction("Index", "Home");
            }

            ResetPasswordViewModel viewModel = new ResetPasswordViewModel
            {
                UserId = userId,
                Token = token,
                Password = "",
                RepeatPassword = "",
            };

            return View(viewModel);
        }

        // Step 4: reset the password
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reset(ResetPasswordViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            GotchaUser? user = await _userManager.FindByIdAsync(viewModel.UserId.ToString());

            if (user == null)
            {
                return View("ResetFailed");
            }

            IdentityResult result = await _userManager.ResetPasswordAsync(user, viewModel.Token, viewModel.Password);

            if (result.Succeeded)
            {
                return View("ResetSuccess");
            }

            // Show Identity errors (expired token, password too weak, etc.)
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(viewModel);
        }
    }
}

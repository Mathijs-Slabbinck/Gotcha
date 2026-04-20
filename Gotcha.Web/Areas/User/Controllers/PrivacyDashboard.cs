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
    public class PrivacyDashboardController : Controller
    {
        private readonly UserManager<GotchaUser> _userManager;
        private readonly SignInManager<GotchaUser> _signInManager;
        private readonly GotchaDbContext _dbContext;

        public PrivacyDashboardController(
            UserManager<GotchaUser> userManager,
            SignInManager<GotchaUser> signInManager,
            GotchaDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
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

            // Delete profile image if it exists
            ProfileImage? profileImage = await _dbContext.ProfileImages
                .FirstOrDefaultAsync(pi => EF.Property<Guid?>(pi, "UserId") == user.Id);

            if (profileImage != null)
            {
                _dbContext.ProfileImages.Remove(profileImage);
            }

            // Anonymize personal data
            user.FirstName = "Deleted";
            user.LastName = "User";
            user.Email = $"deleted_{user.Id}@gotcha.local";
            user.NormalizedEmail = user.Email.ToUpperInvariant();
            user.UserName = $"deleted_{user.Id}";
            user.NormalizedUserName = user.UserName.ToUpperInvariant();
            user.ProfileImageSource = null;
            user.GuardianEmail = null;
            user.GuardianConsentToken = null;
            user.PhoneNumber = null;
            user.IsDeleted = true;

            await _userManager.UpdateAsync(user);
            await _dbContext.SaveChangesAsync();

            // Sign out and redirect to home
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}

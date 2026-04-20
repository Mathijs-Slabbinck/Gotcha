using Gotcha.Core.Entities.Models;
using Gotcha.Core.Services.Repository;
using Gotcha.Core.Services.ResultModel;
using Gotcha.Web.Models;
using Gotcha.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gotcha.Web.Controllers;

public class HomeController : Controller
{
    private readonly SignInManager<GotchaUser> _signInManager;
    private readonly UserRepoService _userRepoService;
    private readonly UserManager<GotchaUser> _userManager;

    public HomeController(SignInManager<GotchaUser> signInManager, UserRepoService userRepoService, UserManager<GotchaUser> userManager)
    {
        _signInManager = signInManager;
        _userRepoService = userRepoService;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(HomeViewModel homeViewModel)
    {
        if (!ModelState.IsValid)
            return View("Index", homeViewModel);

        string email = homeViewModel.Email.Trim();
        string password = homeViewModel.Password;
        bool rememberMe = homeViewModel.RememberMe;

        // we use our own _userRepoService.GetByEmailAsync() instead of _userManager.FindByEmailAsync() for testability and logging purposes
        ResultModel<GotchaUser> userResult = await _userRepoService.GetByEmailAsync(email);

        if (!userResult.Success)
        {
            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View("Index", homeViewModel);
        }

        GotchaUser user = userResult.Data!;

        if (user.IsDeleted)
        {
            ModelState.AddModelError(string.Empty, "This account has been deleted.");
            return View("Index", homeViewModel);
        }

        // we use _userManager.CheckPasswordAsync() since the passwords are hashed, not exact string values. Doing this via our own service would be too much work.
        if (await _userManager.CheckPasswordAsync(user, password))
        {
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError(string.Empty, "Please confirm your email before logging in. Check your inbox for the confirmation link.");
                return View("Index", homeViewModel);
            }

            if (user.NeedsGuardianConsent())
            {
                ModelState.AddModelError(string.Empty, "Your parent or guardian has not yet given consent. Please ask them to check their email.");
                return View("Index", homeViewModel);
            }

            await _signInManager.SignInAsync(user, rememberMe);
            return RedirectToAction("Index", "Home", new { area = "User" });
        }

        ModelState.AddModelError(string.Empty, "Invalid email or password.");
        return View("Index", homeViewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

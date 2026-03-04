using Microsoft.AspNetCore.Mvc;

namespace Gotcha.Web.Controllers
{
    public class AccountsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            // TODO: call SignInManager.SignOutAsync() once Identity is fully wired up
            return RedirectToAction("Index", "Home");
        }
    }
}

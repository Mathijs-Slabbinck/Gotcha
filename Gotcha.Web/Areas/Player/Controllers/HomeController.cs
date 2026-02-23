using Gotcha.Core.Enums;
using Gotcha.Web.Areas.Player.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Gotcha.Web.Areas.Player.Controllers
{
    [Area("Player")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["IsAlive"] = true;
            ViewData["IsAdmin"] = true;

            HomeViewModel homeViewModel = new HomeViewModel
            {
                FullName = "Jane Doe",
                Username = "TheLegend27",
                Gender = Genders.Female,
                Weapon = "Sock",
                AssignmentExpirationDate = DateTime.UtcNow.AddDays(5)
            };

            return View(homeViewModel);
        }
    }
}

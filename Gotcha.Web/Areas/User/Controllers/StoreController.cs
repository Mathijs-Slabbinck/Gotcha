using Microsoft.AspNetCore.Mvc;

namespace Gotcha.Web.Areas.User.Controllers
{
    [Area("User")]
    public class StoreController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

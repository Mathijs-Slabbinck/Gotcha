using Microsoft.AspNetCore.Mvc;

namespace Gotcha.Web.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.PreFillReason = TempData["ContactReason"] as string;
            ViewBag.PreFillMessage = TempData["ContactMessage"] as string;

            return View();
        }
    }
}

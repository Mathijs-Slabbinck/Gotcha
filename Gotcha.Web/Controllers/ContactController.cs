using Microsoft.AspNetCore.Mvc;

namespace Gotcha.Web.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace Gotcha.Web.Controllers
{
    public class InfoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

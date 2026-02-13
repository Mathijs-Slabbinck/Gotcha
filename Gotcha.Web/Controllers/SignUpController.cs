using Microsoft.AspNetCore.Mvc;

namespace Gotcha.Web.Controllers
{
    public class SignUpController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

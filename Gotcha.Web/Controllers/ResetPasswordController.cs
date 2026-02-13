using Microsoft.AspNetCore.Mvc;

namespace Gotcha.Web.Controllers
{
    public class ResetPasswordController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

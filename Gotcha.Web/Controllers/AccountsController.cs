using Microsoft.AspNetCore.Mvc;

namespace Gotcha.Web.Controllers
{
    public class AccountsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

using Gotcha.Core.Services.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Gotcha.Web.Controllers
{
    public class SignUpController : Controller
    {
        private readonly UserRepoService _userRepoService;

        public SignUpController(UserRepoService userRepoService)
        {
            _userRepoService = userRepoService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

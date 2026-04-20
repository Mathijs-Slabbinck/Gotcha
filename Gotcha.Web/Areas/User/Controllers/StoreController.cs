using Gotcha.Core.Data;
using Gotcha.Core.Entities.Models;
using Gotcha.Core.Enums;
using Gotcha.Core.Services.Payment;
using Gotcha.Web.Areas.User.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Gotcha.Web.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class StoreController : Controller
    {
        private readonly UserManager<GotchaUser> _userManager;
        private readonly GotchaDbContext _context;
        private readonly IPayPalService _payPalService;
        private readonly PayPalSettings _payPalSettings;

        public StoreController(UserManager<GotchaUser> userManager, GotchaDbContext context, IPayPalService payPalService, IOptions<PayPalSettings> payPalSettings)
        {
            _userManager = userManager;
            _context = context;
            _payPalService = payPalService;
            _payPalSettings = payPalSettings.Value;
        }

        public async Task<IActionResult> Index()
        {
            GotchaUser? user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            VipSettings? vip = await _context.VipSettings
                .FirstOrDefaultAsync(v => EF.Property<Guid>(v, "UserId") == user.Id);

            if (vip == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            var model = new StoreViewModel
            {
                AssassinUnlocked = vip.AssassinModeUnlocked,
                ChaosUnlocked = vip.ChaosModeUnlocked,
                TimedKillsUnlocked = vip.TimedKillsUnlocked,
                CustomKillMethodsUnlocked = vip.CustomKillMethodsUnlocked,
                CurrentLobbySize = vip.MaxLobbySize,
                CurrentPlan = vip.UserPlan,
                PayPalClientId = _payPalSettings.ClientId
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            if (!Enum.TryParse<StoreItem>(request.Feature, out StoreItem item))
            {
                return BadRequest(new { error = "Invalid feature." });
            }

            string? orderId = await _payPalService.CreateOrder(item);
            if (orderId == null)
            {
                return StatusCode(500, new { error = "Failed to create PayPal order." });
            }

            return Json(new { orderId });
        }

        [HttpPost]
        public async Task<IActionResult> CaptureOrder([FromBody] CaptureOrderRequest request)
        {
            if (!Enum.TryParse<StoreItem>(request.Feature, out StoreItem item))
            {
                return BadRequest(new { error = "Invalid feature." });
            }

            bool captured = await _payPalService.CaptureOrder(request.OrderId);
            if (!captured)
            {
                return StatusCode(500, new { error = "Failed to capture PayPal payment." });
            }

            // Payment succeeded — update VipSettings
            GotchaUser? user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            VipSettings? vip = await _context.VipSettings
                .FirstOrDefaultAsync(v => EF.Property<Guid>(v, "UserId") == user.Id);

            if (vip == null)
            {
                return StatusCode(500, new { error = "User VipSettings not found." });
            }

            ApplyPurchase(vip, item);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        private void ApplyPurchase(VipSettings vip, StoreItem item)
        {
            switch (item)
            {
                case StoreItem.AssassinMode:
                    vip.AssassinModeUnlocked = true;
                    break;

                case StoreItem.ChaosMode:
                    vip.ChaosModeUnlocked = true;
                    break;

                case StoreItem.TimedKills:
                    vip.TimedKillsUnlocked = true;
                    break;

                case StoreItem.CustomKillMethods:
                    vip.CustomKillMethodsUnlocked = true;
                    break;

                case StoreItem.Lobby100:
                    if (vip.MaxLobbySize < MaxLobbySize.Medium)
                        vip.MaxLobbySize = MaxLobbySize.Medium;
                    break;

                case StoreItem.Lobby150:
                    if (vip.MaxLobbySize < MaxLobbySize.MediumLarge)
                        vip.MaxLobbySize = MaxLobbySize.MediumLarge;
                    break;

                case StoreItem.Lobby500:
                    if (vip.MaxLobbySize < MaxLobbySize.Large)
                        vip.MaxLobbySize = MaxLobbySize.Large;
                    break;

                case StoreItem.Lobby10000:
                    if (vip.MaxLobbySize < MaxLobbySize.Max)
                        vip.MaxLobbySize = MaxLobbySize.Max;
                    break;
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Gotcha.Web.Areas.Player.ViewModels;
using Gotcha.Web.Areas.Player.ViewModels.BaseViewModels;

namespace Gotcha.Web.Areas.Player.Controllers
{
    [Area("Player")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            ViewData["IsAlive"] = true;
            ViewData["IsAdmin"] = true;

            var model = new AdminViewModel
            {
                HasStarted = false,
                GameName = "Summer Gotcha 2026",
                InviteLink = "https://example.com/invite/abc123",
                PlayerCount = 5,
                MaxPlayers = 20,

                // Game settings
                ShowPlayerImages = true,
                EnforcePlayerImages = false,
                ShowRealNames = true,
                ShowUsernames = true,
                ShowLivingPlayerCount = true,
                ShowLivingPlayerNames = false,
                ShowLivingPlayerNamesToDeath = false,
                CustomKillMethods = true,
                CustomRules = new List<string> { "No kills on school grounds", "No kills during class hours" },
                KillMethods = new List<string> { "Nerf Gun", "Water Balloon", "Tag" },

                // VIP-gated settings
                AssassinModeUnlocked = true,
                IsAssassin = false,
                ChaosModeUnlocked = true,
                IsChaos = false,
                ChaosTimerMinHours = 6,
                ChaosTimerMaxHours = 12,
                TimedKillsUnlocked = true,
                IsTimed = false,
                TargetTimeOutHours = 24,

                // Mock players
                Players = new List<AdminPlayerViewModel>
                {
                    new AdminPlayerViewModel
                    {
                        PlayerId = Guid.NewGuid(),
                        Name = "John Doe",
                        Username = "TheLegend27",
                        HasImage = true
                    },
                    new AdminPlayerViewModel
                    {
                        PlayerId = Guid.NewGuid(),
                        Name = "Jane Smith",
                        Username = "ShadowHunter",
                        HasImage = false
                    },
                    new AdminPlayerViewModel
                    {
                        PlayerId = Guid.NewGuid(),
                        Name = "Bob Wilson",
                        Username = "SilentBob",
                        HasImage = true
                    },
                    new AdminPlayerViewModel
                    {
                        PlayerId = Guid.NewGuid(),
                        Name = "Alice Brown",
                        Username = "AliceInChains",
                        HasImage = false
                    },
                    new AdminPlayerViewModel
                    {
                        PlayerId = Guid.NewGuid(),
                        Name = "Charlie Davis",
                        Username = "CharlieD",
                        HasImage = true
                    }
                },

                // Mock pending kills
                PendingKills = new List<AdminKillViewModel>
                {
                    new AdminKillViewModel
                    {
                        KillId = Guid.NewGuid(),
                        KillerName = "TheLegend27",
                        VictimName = "ShadowHunter",
                        Weapon = "Nerf Gun",
                        Moment = DateTime.UtcNow.AddMinutes(-15)
                    },
                    new AdminKillViewModel
                    {
                        KillId = Guid.NewGuid(),
                        KillerName = "SilentBob",
                        VictimName = "AliceInChains",
                        Weapon = "Water Balloon",
                        Moment = DateTime.UtcNow.AddHours(-2)
                    }
                }
            };

            return View(model);
        }
    }
}

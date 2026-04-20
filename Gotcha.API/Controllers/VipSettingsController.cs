using Gotcha.API.Dtos.VipSettings;
using Gotcha.Core.Data;
using Gotcha.Core.Enums;
using Gotcha.Shared.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gotcha.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VipSettingsController : ControllerBase
    {
        private readonly GotchaDbContext _context;

        public VipSettingsController(GotchaDbContext context)
        {
            _context = context;
        }

        // GET api/vipsettings/{userId}
        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.VipSettings)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Create default VipSettings if none exist
            if (user.VipSettings == null)
            {
                user.VipSettings = new Core.Entities.Models.VipSettings();
                await _context.SaveChangesAsync();
            }

            var vip = user.VipSettings;
            var dto = new VipSettingsResponseDto
            {
                Id = vip.Id,
                AssassinModeUnlocked = vip.AssassinModeUnlocked,
                ChaosModeUnlocked = vip.ChaosModeUnlocked,
                TimedKillsUnlocked = vip.TimedKillsUnlocked,
                CustomKillMethodsUnlocked = vip.CustomKillMethodsUnlocked,
                MaxLobbySize = vip.MaxLobbySize.ToString(),
                UserPlan = vip.UserPlan.ToString()
            };

            return Ok(dto);
        }

        // PATCH api/vipsettings/{userId}
        [HttpPatch("{userId:guid}")]
        public async Task<IActionResult> Patch(Guid userId, [FromBody] UpdateVipSettingsDto dto)
        {
            var user = await _context.Users
                .Include(u => u.VipSettings)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (user.VipSettings == null)
            {
                user.VipSettings = new Core.Entities.Models.VipSettings();
            }

            var vip = user.VipSettings;

            if (dto.AssassinModeUnlocked.HasValue) vip.AssassinModeUnlocked = dto.AssassinModeUnlocked.Value;
            if (dto.ChaosModeUnlocked.HasValue) vip.ChaosModeUnlocked = dto.ChaosModeUnlocked.Value;
            if (dto.TimedKillsUnlocked.HasValue) vip.TimedKillsUnlocked = dto.TimedKillsUnlocked.Value;
            if (dto.CustomKillMethodsUnlocked.HasValue) vip.CustomKillMethodsUnlocked = dto.CustomKillMethodsUnlocked.Value;

            if (dto.MaxLobbySize != null && Enum.TryParse<MaxLobbySize>(dto.MaxLobbySize, out var lobbySize))
            {
                vip.MaxLobbySize = lobbySize;
            }

            if (dto.UserPlan != null && Enum.TryParse<Plan>(dto.UserPlan, out var plan))
            {
                vip.UserPlan = plan;
            }

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}

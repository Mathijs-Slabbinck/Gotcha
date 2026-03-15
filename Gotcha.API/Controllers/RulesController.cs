using Gotcha.API.Dtos.Rules;
using Gotcha.Core.Services.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Gotcha.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RulesController : ControllerBase
    {
        private readonly RulesRepoService _rulesRepo;

        public RulesController(RulesRepoService rulesRepo)
        {
            _rulesRepo = rulesRepo;
        }

        // GET api/rules/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _rulesRepo.GetByIdAsync(id);

            if (!result.Success)
            {
                return NotFound();
            }

            var rules = result.Data!;
            var dto = new RulesResponseDto
            {
                Id = rules.Id,
                IsAssassin = rules.IsAssassin,
                ShowHunter = rules.ShowHunter,
                ShowPlayerImages = rules.ShowPlayerImages,
                ShowGender = rules.ShowGender,
                EnforcePlayerImages = rules.EnforcePlayerImages,
                ShowRealNames = rules.ShowRealNames,
                ShowUsernames = rules.ShowUsernames,
                ShowLivingPlayerCount = rules.ShowLivingPlayerCount,
                ShowLivingPlayerNames = rules.ShowLivingPlayerNames,
                ShowLivingPlayerNamesToDeath = rules.ShowLivingPlayerNamesToDeath,
                IsTimed = rules.IsTimed,
                TargetTimeOut = rules.TargetTimeOut,
                CustomRules = rules.CustomRules?.ToList(),
                CustomKillMethods = rules.CustomKillMethods,
                KillMethods = rules.KillMethods?.ToList(),
                IsChaos = rules.IsChaos,
                ChaosTimerMin = rules.ChaosTimerMin,
                ChaosTimerMax = rules.ChaosTimerMax,
                KillConfirmationTimer = rules.KillConfirmationTimer
            };

            return Ok(dto);
        }
    }
}

using Gotcha.API.Dtos.Kills;
using Gotcha.Core.Services.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Gotcha.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KillsController : ControllerBase
    {
        private readonly KillRepoService _killRepo;

        public KillsController(KillRepoService killRepo)
        {
            _killRepo = killRepo;
        }

        // GET api/kills
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _killRepo.GetAllAsync();

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            var dtos = result.Data!.Select(k => new KillResponseDto
            {
                Id = k.Id,
                GameId = k.GameId,
                KillerId = k.KillerId,
                VictimId = k.VictimId,
                Moment = k.Moment,
                Weapon = k.Weapon,
                Reason = k.Reason,
                IsValid = k.IsValid,
                KillMessage = k.KillMessage,
                TimeSinceAssignedTarget = k.TimeSinceAssignedTarget
            }).ToList();

            return Ok(dtos);
        }
    }
}

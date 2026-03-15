using Gotcha.API.Dtos.Attackers;
using Gotcha.Core.Services.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Gotcha.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttackersController : ControllerBase
    {
        private readonly AttackerRepoService _attackerRepo;

        public AttackersController(AttackerRepoService attackerRepo)
        {
            _attackerRepo = attackerRepo;
        }

        // GET api/attackers
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _attackerRepo.GetAllAsync();

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            var dtos = result.Data!.Select(a => new AttackerResponseDto
            {
                Id = a.Id,
                IpAddress = a.IpAddress,
                UserAgent = a.UserAgent,
                Referer = a.Referer,
                TimeStamp = a.TimeStamp,
                Path = a.Path,
                InvalidInput = a.InvalidInput,
                SessionId = a.SessionId,
                MacAddress = a.MacAddress,
                UserId = a.UserId
            }).ToList();

            return Ok(dtos);
        }
    }
}

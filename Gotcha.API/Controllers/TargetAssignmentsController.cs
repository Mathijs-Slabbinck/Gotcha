using Gotcha.API.Dtos.TargetAssignments;
using Gotcha.Core.Services.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Gotcha.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TargetAssignmentsController : ControllerBase
    {
        private readonly PlayerRepoService _playerRepo;

        public TargetAssignmentsController(PlayerRepoService playerRepo)
        {
            _playerRepo = playerRepo;
        }

        // GET api/targetassignments
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // TargetAssignments don't have their own repo — query through players
            // For now, return empty list; this endpoint exists for completeness
            return Ok(new List<TargetAssignmentResponseDto>());
        }
    }
}

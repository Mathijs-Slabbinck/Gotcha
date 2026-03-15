using Gotcha.API.Dtos.Logs;
using Gotcha.Core.Entities.Logging.Models;
using Gotcha.Core.Enums;
using Gotcha.Core.Services.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Gotcha.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly LogRepoService _logRepo;

        public LogsController(LogRepoService logRepo)
        {
            _logRepo = logRepo;
        }

        // POST api/logs
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLogDto dto)
        {
            if (!Enum.TryParse<LogTypes>(dto.LogType, out var logType))
            {
                return BadRequest($"Invalid LogType: {dto.LogType}");
            }

            if (!Enum.TryParse<LogSubTypes>(dto.LogSubType, out var logSubType))
            {
                return BadRequest($"Invalid LogSubType: {dto.LogSubType}");
            }

            var log = new Log
            {
                LogGroupId = dto.LogGroupId ?? Guid.NewGuid(),
                LogType = logType,
                LogSubType = logSubType,
                Message = dto.Message,
                ExtraInfo = dto.ExtraInfo
            };

            var result = await _logRepo.AddAsync(log);

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            return CreatedAtAction(null, new { id = result.Data!.Id }, null);
        }
    }
}

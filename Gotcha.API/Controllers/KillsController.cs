using Gotcha.API.Dtos.Games;
using Gotcha.API.Dtos.Kills;
using Gotcha.Core.Data;
using Gotcha.Core.Exceptions;
using Gotcha.Core.Services;
using Gotcha.Core.Services.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gotcha.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KillsController : ControllerBase
    {
        private readonly KillRepoService _killRepo;
        private readonly GotchaDbContext _context;
        private readonly GameService _gameService;

        public KillsController(KillRepoService killRepo, GotchaDbContext context, GameService gameService)
        {
            _killRepo = killRepo;
            _context = context;
            _gameService = gameService;
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

        // POST api/kills/{id}/validate
        [HttpPost("{id:guid}/validate")]
        public async Task<IActionResult> Validate(Guid id, [FromBody] AdminActionDto dto)
        {
            var game = await LoadGameByKillId(id);

            if (game == null)
            {
                return NotFound("Kill not found.");
            }

            var adminPlayer = game.Players.FirstOrDefault(p => p.Id == dto.AdminPlayerId);

            if (adminPlayer == null || !game.AdminIds.Contains(adminPlayer.Id))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You don't have permission to perform this action on the game.");
            }

            var kill = game.Kills.FirstOrDefault(k => k.Id == id);

            if (kill == null)
            {
                return NotFound("Kill not found.");
            }

            var killer = game.Players.FirstOrDefault(p => p.Id == kill.KillerId);
            var victim = game.Players.FirstOrDefault(p => p.Id == kill.VictimId);

            if (killer == null || victim == null)
            {
                return BadRequest("Killer or victim not found in game.");
            }

            try
            {
                _gameService.HandleValidKill(game, killer, victim, kill.Weapon);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (GotchaException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/kills/{id}/reject
        [HttpPost("{id:guid}/reject")]
        public async Task<IActionResult> Reject(Guid id, [FromBody] AdminActionDto dto)
        {
            var game = await LoadGameByKillId(id);

            if (game == null)
            {
                return NotFound("Kill not found.");
            }

            var adminPlayer = game.Players.FirstOrDefault(p => p.Id == dto.AdminPlayerId);

            if (adminPlayer == null || !game.AdminIds.Contains(adminPlayer.Id))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You don't have permission to perform this action on the game.");
            }

            var kill = game.Kills.FirstOrDefault(k => k.Id == id);

            if (kill == null)
            {
                return NotFound("Kill not found.");
            }

            var killer = game.Players.FirstOrDefault(p => p.Id == kill.KillerId);
            var victim = game.Players.FirstOrDefault(p => p.Id == kill.VictimId);

            if (killer == null || victim == null)
            {
                return BadRequest("Killer or victim not found in game.");
            }

            try
            {
                _gameService.HandleInValidKill(game, killer, victim, weapon: kill.Weapon);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (GotchaException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #region Private Helpers

        private async Task<Core.Entities.Models.Game?> LoadGameByKillId(Guid killId)
        {
            return await _context.Games
                .Where(g => g.Kills.Any(k => k.Id == killId))
                .Include(g => g.Rules)
                .Include(g => g.Players)
                    .ThenInclude(p => p.User)
                .Include(g => g.Players)
                    .ThenInclude(p => p.TargetAssignments)
                        .ThenInclude(ta => ta.Target)
                .Include(g => g.Players)
                    .ThenInclude(p => p.TargetAssignments)
                        .ThenInclude(ta => ta.Kill)
                .Include(g => g.Kills)
                    .ThenInclude(k => k.Killer)
                .Include(g => g.Kills)
                    .ThenInclude(k => k.Victim)
                .FirstOrDefaultAsync();
        }

        #endregion
    }
}

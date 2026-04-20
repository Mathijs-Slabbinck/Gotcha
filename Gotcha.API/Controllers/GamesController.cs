using Gotcha.API.Dtos.Games;
using Gotcha.Core.Data;
using Gotcha.Core.Entities.Models;
using Gotcha.Core.Exceptions;
using Gotcha.Core.Services;
using Gotcha.Core.Services.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gotcha.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly GameRepoService _gameRepo;
        private readonly GotchaDbContext _context;
        private readonly GameService _gameService;

        public GamesController(GameRepoService gameRepo, GotchaDbContext context, GameService gameService)
        {
            _gameRepo = gameRepo;
            _context = context;
            _gameService = gameService;
        }

        // GET api/games
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _gameRepo.GetAllAsync();

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            var dtos = result.Data!.Select(g => new GameResponseDto
            {
                Id = g.Id,
                Name = g.Name,
                CreationDate = g.CreationDate,
                StartDate = g.StartDate,
                EndDate = g.EndDate,
                HasStarted = g.HasStarted,
                IsFinished = g.IsFinished,
                WinnerId = g.WinnerId,
                CreatorId = g.CreatorId,
                AdminIds = g.AdminIds.ToList(),
                MaxPlayers = g.MaxPlayers
            }).ToList();

            return Ok(dtos);
        }

        // GET api/games/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _gameRepo.GetByIdAsync(id);

            if (!result.Success)
            {
                return NotFound("Game not found.");
            }

            var game = result.Data!;
            var dto = new GameResponseDto
            {
                Id = game.Id,
                Name = game.Name,
                CreationDate = game.CreationDate,
                StartDate = game.StartDate,
                EndDate = game.EndDate,
                HasStarted = game.HasStarted,
                IsFinished = game.IsFinished,
                WinnerId = game.WinnerId,
                CreatorId = game.CreatorId,
                AdminIds = game.AdminIds.ToList(),
                MaxPlayers = game.MaxPlayers
            };

            return Ok(dto);
        }

        // POST api/games
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateGameDto dto)
        {
            var rules = new Rules();
            var game = new Game
            {
                Name = dto.Name,
                Rules = rules,
                CreatorId = dto.CreatorId,
                MaxPlayers = dto.MaxPlayers
            };

            var result = await _gameRepo.AddAsync(game);

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            var responseDto = new GameResponseDto
            {
                Id = result.Data!.Id,
                Name = result.Data.Name,
                CreationDate = result.Data.CreationDate,
                MaxPlayers = result.Data.MaxPlayers,
                CreatorId = result.Data.CreatorId
            };

            return CreatedAtAction(nameof(Get), new { id = responseDto.Id }, responseDto);
        }

        // POST api/games/{id}/start
        [HttpPost("{id:guid}/start")]
        public async Task<IActionResult> Start(Guid id, [FromBody] AdminActionDto dto)
        {
            var game = await LoadFullGame(id);

            if (game == null)
            {
                return NotFound("Game not found.");
            }

            var adminPlayer = game.Players.FirstOrDefault(p => p.Id == dto.AdminPlayerId);

            if (adminPlayer == null || !game.AdminIds.Contains(adminPlayer.Id))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You don't have permission to perform this action on the game.");
            }

            try
            {
                _gameService.StartGame(game);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (GotchaException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/games/{id}/end
        [HttpPost("{id:guid}/end")]
        public async Task<IActionResult> End(Guid id, [FromBody] AdminActionDto dto)
        {
            var game = await LoadFullGame(id);

            if (game == null)
            {
                return NotFound("Game not found.");
            }

            var adminPlayer = game.Players.FirstOrDefault(p => p.Id == dto.AdminPlayerId);

            if (adminPlayer == null || !game.AdminIds.Contains(adminPlayer.Id))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You don't have permission to perform this action on the game.");
            }

            if (game.IsFinished)
            {
                return BadRequest("Game is already finished.");
            }

            game.IsFinished = true;
            game.EndDate = DateTime.UtcNow;

            // Set winner to last living player if there is one
            var livingPlayers = game.GetLivingPlayers().ToList();
            if (livingPlayers.Count == 1)
            {
                game.Winner = livingPlayers[0];
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        // PATCH api/games/{id}/settings
        [HttpPatch("{id:guid}/settings")]
        public async Task<IActionResult> UpdateSettings(Guid id, [FromBody] UpdateGameSettingsDto dto)
        {
            var game = await _context.Games
                .Include(g => g.Rules)
                .Include(g => g.Players)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null)
            {
                return NotFound("Game not found.");
            }

            var adminPlayer = game.Players.FirstOrDefault(p => p.Id == dto.AdminPlayerId);

            if (adminPlayer == null || !game.AdminIds.Contains(adminPlayer.Id))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You don't have permission to perform this action on the game.");
            }

            var rules = game.Rules;

            rules.ShowPlayerImages = dto.ShowPlayerImages;
            rules.ShowGender = dto.ShowGender;
            rules.EnforcePlayerImages = dto.EnforcePlayerImages;
            rules.ShowRealNames = dto.ShowRealNames;
            rules.ShowUsernames = dto.ShowUsernames;
            rules.ShowLivingPlayerCount = dto.ShowLivingPlayerCount;
            rules.ShowLivingPlayerNames = dto.ShowLivingPlayerNames;
            rules.ShowLivingPlayerNamesToDeath = dto.ShowLivingPlayerNamesToDeath;
            rules.IsAssassin = dto.IsAssassin;
            rules.ShowHunter = dto.ShowHunter;
            rules.IsChaos = dto.IsChaos;
            if (dto.ChaosTimerMinHours.HasValue) rules.ChaosTimerMin = TimeSpan.FromHours(dto.ChaosTimerMinHours.Value);
            if (dto.ChaosTimerMaxHours.HasValue) rules.ChaosTimerMax = TimeSpan.FromHours(dto.ChaosTimerMaxHours.Value);
            rules.IsTimed = dto.IsTimed;
            if (dto.TargetTimeOutHours.HasValue) rules.TargetTimeOut = TimeSpan.FromHours(dto.TargetTimeOutHours.Value);
            rules.CustomKillMethods = dto.CustomKillMethods;

            if (dto.KillMethods != null)
            {
                rules.KillMethods = dto.KillMethods
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(m => m.Trim())
                    .ToList();
            }

            if (dto.CustomRulesText != null)
            {
                rules.CustomRules = dto.CustomRulesText
                    .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                    .Select(r => r.Trim())
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(dto.GameName))
            {
                game.Name = dto.GameName;
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        #region Private Helpers

        private async Task<Game?> LoadFullGame(Guid gameId)
        {
            return await _context.Games
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
                .FirstOrDefaultAsync(g => g.Id == gameId);
        }

        #endregion
    }
}

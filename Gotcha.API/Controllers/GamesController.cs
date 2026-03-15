using Gotcha.API.Dtos.Games;
using Gotcha.Core.Entities.Models;
using Gotcha.Core.Services.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Gotcha.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly GameRepoService _gameRepo;

        public GamesController(GameRepoService gameRepo)
        {
            _gameRepo = gameRepo;
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
                return NotFound();
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
    }
}

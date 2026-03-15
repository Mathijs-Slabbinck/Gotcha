using Gotcha.API.Dtos.GotchaUsers;
using Gotcha.Core.Data;
using Gotcha.Core.Services.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gotcha.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GotchaUsersController : ControllerBase
    {
        private readonly UserRepoService _userRepo;
        private readonly GotchaDbContext _context;

        public GotchaUsersController(UserRepoService userRepo, GotchaDbContext context)
        {
            _userRepo = userRepo;
            _context = context;
        }

        // GET api/gotchausers
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _userRepo.GetAllAsync();

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            var dtos = result.Data!.Select(u => new GotchaUserResponseDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                UserName = u.UserName,
                Email = u.Email,
                ProfileImageSource = u.ProfileImageSource,
                Gender = u.Gender.ToString(),
                BirthDate = u.BirthDate,
                AccountCreationDate = u.AccountCreationDate,
                GuardianEmail = u.GuardianEmail,
                HasGuardianConsent = u.HasGuardianConsent,
                GuardianConsentDate = u.GuardianConsentDate
            }).ToList();

            return Ok(dtos);
        }

        // GET api/gotchausers/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _userRepo.GetByIdAsync(id);

            if (!result.Success)
            {
                return NotFound();
            }

            var user = result.Data!;
            var dto = new GotchaUserResponseDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                ProfileImageSource = user.ProfileImageSource,
                Gender = user.Gender.ToString(),
                BirthDate = user.BirthDate,
                AccountCreationDate = user.AccountCreationDate,
                GuardianEmail = user.GuardianEmail,
                HasGuardianConsent = user.HasGuardianConsent,
                GuardianConsentDate = user.GuardianConsentDate
            };

            return Ok(dto);
        }

        // PUT api/gotchausers/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateGotchaUserDto dto)
        {
            var result = await _userRepo.GetByIdAsync(id);

            if (!result.Success)
            {
                return NotFound();
            }

            var user = result.Data!;

            if (dto.FirstName != null) user.FirstName = dto.FirstName;
            if (dto.LastName != null) user.LastName = dto.LastName;
            if (dto.UserName != null) user.UserName = dto.UserName;
            if (dto.Email != null) user.Email = dto.Email;
            if (dto.ProfileImageSource != null) user.ProfileImageSource = dto.ProfileImageSource;

            var updateResult = await _userRepo.UpdateAsync(user);

            if (!updateResult.Success)
            {
                return BadRequest(updateResult.Errors);
            }

            return Ok();
        }

        // GET api/gotchausers/{id}/profile
        [HttpGet("{id:guid}/profile")]
        public async Task<IActionResult> GetProfile(Guid id)
        {
            var user = await _context.GotchaUsers
                .Include(u => u.PlayerAccounts)
                    .ThenInclude(p => p.Game)
                        .ThenInclude(g => g.Kills)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            int gamesPlayed = user.PlayerAccounts.Count;
            int gamesWon = user.PlayerAccounts.Count(p => p.Game.WinnerId == p.Id);

            // Count kills across all games where this user's player was the killer
            var playerIds = user.PlayerAccounts.Select(p => p.Id).ToHashSet();
            int totalKills = user.PlayerAccounts
                .SelectMany(p => p.Game.Kills)
                .Count(k => playerIds.Contains(k.KillerId) && k.IsValid);

            // Calculate biggest kill streak: max consecutive kills by any single player account
            int biggestStreak = 0;
            foreach (var player in user.PlayerAccounts)
            {
                var kills = player.Game.Kills
                    .Where(k => k.KillerId == player.Id && k.IsValid)
                    .OrderBy(k => k.Moment)
                    .ToList();

                biggestStreak = Math.Max(biggestStreak, kills.Count);
            }

            var dto = new UserProfileResponseDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                BirthDate = user.BirthDate,
                AccountCreationDate = user.AccountCreationDate,
                GamesPlayed = gamesPlayed,
                GamesWon = gamesWon,
                TotalKills = totalKills,
                BiggestKillStreak = biggestStreak
            };

            return Ok(dto);
        }

        // GET api/gotchausers/{id}/games?status=pending|active|ended
        [HttpGet("{id:guid}/games")]
        public async Task<IActionResult> GetGames(Guid id, [FromQuery] string status = "pending")
        {
            var players = await _context.Players
                .Include(p => p.Game)
                    .ThenInclude(g => g.Players)
                .Include(p => p.Game)
                    .ThenInclude(g => g.Kills)
                .Where(p => p.UserId == id)
                .ToListAsync();

            var games = players.Select(p => new { Player = p, Game = p.Game });

            games = status.ToLower() switch
            {
                "pending" => games.Where(x => !x.Game.HasStarted && !x.Game.IsFinished),
                "active" => games.Where(x => x.Game.HasStarted && !x.Game.IsFinished),
                "ended" => games.Where(x => x.Game.IsFinished),
                _ => games
            };

            var dtos = games.Select(x =>
            {
                // Find winner name if game is finished
                string? winnerName = null;
                if (x.Game.IsFinished && x.Game.WinnerId.HasValue)
                {
                    var winner = x.Game.Players.FirstOrDefault(p => p.Id == x.Game.WinnerId.Value);
                    winnerName = winner?.UserName ?? "Unknown";
                }

                return new Dtos.Games.GameItemResponseDto
                {
                    Id = x.Game.Id,
                    Name = x.Game.Name,
                    CreationDate = x.Game.CreationDate,
                    StartDate = x.Game.StartDate,
                    EndDate = x.Game.EndDate,
                    HasStarted = x.Game.HasStarted,
                    IsFinished = x.Game.IsFinished,
                    WinnerName = winnerName,
                    PlayerCount = x.Game.Players.Count,
                    IsAlive = x.Player.IsAlive,
                    PlayerId = x.Player.Id
                };
            }).ToList();

            return Ok(dtos);
        }
    }
}

using Gotcha.API.Dtos.Players;
using Gotcha.Core.Data;
using Gotcha.Core.Entities.Models;
using Gotcha.Core.Enums;
using Gotcha.Core.Exceptions;
using Gotcha.Core.Services;
using Gotcha.Core.Services.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gotcha.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly PlayerRepoService _playerRepo;
        private readonly GotchaDbContext _context;
        private readonly GameService _gameService;

        public PlayersController(PlayerRepoService playerRepo, GotchaDbContext context, GameService gameService)
        {
            _playerRepo = playerRepo;
            _context = context;
            _gameService = gameService;
        }

        // GET api/players
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _playerRepo.GetAllAsync();

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            var dtos = result.Data!.Select(p => new PlayerResponseDto
            {
                Id = p.Id,
                UserId = p.UserId,
                GameId = p.GameId,
                UserName = p.UserName,
                ProfileImageSource = p.ProfileImageSource,
                IsAlive = p.IsAlive,
                IsAdmin = p.IsAdmin,
                IsSpectator = p.IsSpectator,
                Notes = p.Notes
            }).ToList();

            return Ok(dtos);
        }

        // GET api/players/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _playerRepo.GetByIdAsync(id);

            if (!result.Success)
            {
                return NotFound("Player not found.");
            }

            var player = result.Data!;
            var dto = new PlayerResponseDto
            {
                Id = player.Id,
                UserId = player.UserId,
                GameId = player.GameId,
                UserName = player.UserName,
                ProfileImageSource = player.ProfileImageSource,
                IsAlive = player.IsAlive,
                IsAdmin = player.IsAdmin,
                IsSpectator = player.IsSpectator,
                Notes = player.Notes
            };

            return Ok(dto);
        }

        // PATCH api/players/{id}
        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Patch(Guid id, [FromBody] UpdatePlayerDto dto)
        {
            var result = await _playerRepo.GetByIdAsync(id);

            if (!result.Success)
            {
                return NotFound("Player not found.");
            }

            var player = result.Data!;

            if (dto.UserName != null) player.UserName = dto.UserName;
            if (dto.ProfileImageSource != null) player.ProfileImageSource = dto.ProfileImageSource;
            if (dto.IsAdmin.HasValue) player.IsAdmin = dto.IsAdmin.Value;
            if (dto.IsSpectator.HasValue) player.IsSpectator = dto.IsSpectator.Value;
            if (dto.Notes != null) player.Notes = dto.Notes;

            var updateResult = await _playerRepo.UpdateAsync(player);

            if (!updateResult.Success)
            {
                return BadRequest(updateResult.Errors);
            }

            return Ok();
        }

        // GET api/players/{id}/home
        [HttpGet("{id:guid}/home")]
        public async Task<IActionResult> GetHome(Guid id)
        {
            var player = await _context.Players
                .Include(p => p.User)
                .Include(p => p.Game)
                    .ThenInclude(g => g.Rules)
                .Include(p => p.Game)
                    .ThenInclude(g => g.Players)
                        .ThenInclude(p2 => p2.User)
                .Include(p => p.Game)
                    .ThenInclude(g => g.Kills)
                        .ThenInclude(k => k.Killer)
                .Include(p => p.Game)
                    .ThenInclude(g => g.Kills)
                        .ThenInclude(k => k.Victim)
                .Include(p => p.TargetAssignments)
                    .ThenInclude(ta => ta.Target)
                        .ThenInclude(t => t.User)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (player == null)
            {
                return NotFound("Player not found.");
            }

            var game = player.Game;
            var rules = game.Rules;

            // Find current target assignment (where this player is the hunter, status ongoing)
            var currentAssignment = player.TargetAssignments
                .FirstOrDefault(ta => ta.AssignmentStatus == AssignmentStatus.Ongoing);

            // Find who is hunting this player
            var hunterAssignment = await _context.TargetAssignments
                .Include(ta => ta.Hunter)
                    .ThenInclude(h => h.User)
                .FirstOrDefaultAsync(ta => ta.TargetId == id && ta.AssignmentStatus == AssignmentStatus.Ongoing);

            // Find if this player was killed
            var deathKill = game.Kills
                .FirstOrDefault(k => k.VictimId == id && k.IsValid);

            // Find winner info
            string winnerName = string.Empty;
            string winnerOtherName = string.Empty;
            if (game.IsFinished && game.WinnerId.HasValue)
            {
                var winner = game.Players.FirstOrDefault(p => p.Id == game.WinnerId.Value);
                if (winner != null)
                {
                    winnerName = $"{winner.User.FirstName} {winner.User.LastName}";
                    winnerOtherName = winner.UserName ?? string.Empty;
                }
            }

            // Build kills list (kills this player made)
            var kills = game.Kills
                .Where(k => k.KillerId == id && k.IsValid)
                .OrderByDescending(k => k.Moment)
                .Select(k => new KillItemDto
                {
                    VictimName = $"{k.Victim.User.FirstName} {k.Victim.User.LastName}",
                    VictimUsername = k.Victim.UserName ?? string.Empty,
                    Weapon = k.Weapon,
                    TimeStamp = k.Moment
                }).ToList();

            // Build players list
            var players = game.Players.Select(p => new PlayerItemDto
            {
                Name = $"{p.User.FirstName} {p.User.LastName}",
                OtherName = p.UserName ?? string.Empty,
                IsAlive = p.IsAlive
            }).ToList();

            var dto = new PlayerHomeResponseDto
            {
                IsAlive = player.IsAlive,
                IsSpectator = player.IsSpectator,

                TargetName = currentAssignment != null
                    ? $"{currentAssignment.Target.User.FirstName} {currentAssignment.Target.User.LastName}"
                    : string.Empty,
                TargetUsername = currentAssignment?.Target.UserName ?? string.Empty,
                Weapon = currentAssignment?.Weapon,
                AssignmentExpirationDate = currentAssignment?.AssignmentExpirationDate,

                IsAssassin = rules.IsAssassin,
                IsChaos = rules.IsChaos,
                IsTimed = rules.IsTimed,
                ChaosTimerMinHours = (int)rules.ChaosTimerMin.TotalHours,
                ChaosTimerMaxHours = (int)rules.ChaosTimerMax.TotalHours,
                TargetTimeOutHours = (int)rules.TargetTimeOut.TotalHours,
                CustomRules = rules.CustomRules?.ToList() ?? new List<string>(),

                ShowHunter = rules.ShowHunter,
                HunterName = hunterAssignment != null
                    ? $"{hunterAssignment.Hunter.User.FirstName} {hunterAssignment.Hunter.User.LastName}"
                    : string.Empty,
                HunterOtherName = hunterAssignment?.Hunter.UserName ?? string.Empty,

                StartDate = game.StartDate,
                EndDate = game.EndDate,
                WinnerName = winnerName,
                WinnerOtherName = winnerOtherName,
                KilledOnDate = deathKill?.Moment,
                KillerName = deathKill != null
                    ? $"{deathKill.Killer.User.FirstName} {deathKill.Killer.User.LastName}"
                    : string.Empty,
                KillerOtherName = deathKill?.Killer.UserName ?? string.Empty,

                ShowLivingPlayerCount = rules.ShowLivingPlayerCount,
                ShowLivingPlayerNames = rules.ShowLivingPlayerNames,

                Kills = kills,
                Players = players
            };

            return Ok(dto);
        }

        // GET api/players/{id}/confirmkill
        [HttpGet("{id:guid}/confirmkill")]
        public async Task<IActionResult> GetConfirmKill(Guid id)
        {
            var player = await _context.Players
                .Include(p => p.Game)
                    .ThenInclude(g => g.Rules)
                .Include(p => p.TargetAssignments)
                    .ThenInclude(ta => ta.Target)
                        .ThenInclude(t => t.User)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (player == null)
            {
                return NotFound("Player not found.");
            }

            var rules = player.Game.Rules;

            // Current target assignment
            var currentAssignment = player.TargetAssignments
                .FirstOrDefault(ta => ta.AssignmentStatus == AssignmentStatus.Ongoing);

            // Who is hunting this player
            var hunterAssignment = await _context.TargetAssignments
                .Include(ta => ta.Hunter)
                    .ThenInclude(h => h.User)
                .FirstOrDefaultAsync(ta => ta.TargetId == id && ta.AssignmentStatus == AssignmentStatus.Ongoing);

            var dto = new ConfirmKillResponseDto
            {
                TargetName = currentAssignment != null
                    ? $"{currentAssignment.Target.User.FirstName} {currentAssignment.Target.User.LastName}"
                    : string.Empty,
                TargetUsername = currentAssignment?.Target.UserName ?? string.Empty,
                Weapon = currentAssignment?.Weapon,
                HunterName = hunterAssignment != null
                    ? $"{hunterAssignment.Hunter.User.FirstName} {hunterAssignment.Hunter.User.LastName}"
                    : string.Empty,
                HunterUsername = hunterAssignment?.Hunter.UserName ?? string.Empty,
                IsAssassinMode = rules.IsAssassin,
                ShowHunter = rules.ShowHunter
            };

            return Ok(dto);
        }

        // POST api/players/{id}/confirmkill
        [HttpPost("{id:guid}/confirmkill")]
        public async Task<IActionResult> ConfirmKill(Guid id)
        {
            var player = await LoadFullPlayer(id);

            if (player == null)
            {
                return NotFound("Player not found.");
            }

            var game = player.Game;
            var currentAssignment = player.TargetAssignments
                .FirstOrDefault(ta => ta.AssignmentStatus == AssignmentStatus.Ongoing);

            if (currentAssignment == null)
            {
                return BadRequest("No ongoing target assignment found.");
            }

            var victim = game.Players.FirstOrDefault(p => p.Id == currentAssignment.TargetId);

            if (victim == null)
            {
                return BadRequest("Target player not found in game.");
            }

            try
            {
                _gameService.HandleValidKill(game, player, victim, currentAssignment.Weapon);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (GotchaException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/players/{id}/rejectkill
        [HttpPost("{id:guid}/rejectkill")]
        public async Task<IActionResult> RejectKill(Guid id)
        {
            var player = await LoadFullPlayer(id);

            if (player == null)
            {
                return NotFound("Player not found.");
            }

            var game = player.Game;
            var currentAssignment = player.TargetAssignments
                .FirstOrDefault(ta => ta.AssignmentStatus == AssignmentStatus.Ongoing);

            if (currentAssignment == null)
            {
                return BadRequest("No ongoing target assignment found.");
            }

            var victim = game.Players.FirstOrDefault(p => p.Id == currentAssignment.TargetId);

            if (victim == null)
            {
                return BadRequest("Target player not found in game.");
            }

            try
            {
                _gameService.HandleInValidKill(game, player, victim, weapon: currentAssignment.Weapon);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (GotchaException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/players/{id}/confirmdeath
        [HttpPost("{id:guid}/confirmdeath")]
        public async Task<IActionResult> ConfirmDeath(Guid id)
        {
            var player = await LoadFullPlayer(id);

            if (player == null)
            {
                return NotFound("Player not found.");
            }

            var game = player.Game;

            // Find the hunter who is targeting this player — already loaded via LoadFullPlayer
            (TargetAssignment? hunterAssignment, Player? hunter) = FindHunterForPlayer(game, id);

            if (hunterAssignment == null)
            {
                return BadRequest("No ongoing hunter assignment found for this player.");
            }

            if (hunter == null)
            {
                return BadRequest("Hunter not found in game.");
            }

            try
            {
                // The victim accepts their death — equivalent to the hunter's kill being confirmed
                _gameService.HandleValidKill(game, hunter, player, hunterAssignment.Weapon);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (GotchaException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/players/{id}/confirmhunterkill
        [HttpPost("{id:guid}/confirmhunterkill")]
        public async Task<IActionResult> ConfirmHunterKill(Guid id)
        {
            var player = await LoadFullPlayer(id);

            if (player == null)
            {
                return NotFound("Player not found.");
            }

            var game = player.Game;

            // Find the player's hunter (in Assassin mode, the player kills their hunter)
            (TargetAssignment? hunterAssignment, Player? hunter) = FindHunterForPlayer(game, id);

            if (hunterAssignment == null)
            {
                return BadRequest("No ongoing hunter assignment found.");
            }

            if (hunter == null)
            {
                return BadRequest("Hunter not found in game.");
            }

            try
            {
                _gameService.HandleValidKill(game, player, hunter, hunterAssignment.Weapon);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (GotchaException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/players/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _playerRepo.GetByIdAsync(id);

            if (!result.Success)
            {
                return NotFound("Player not found.");
            }

            var player = result.Data!;

            _context.Players.Remove(player);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // GET api/players/{id}/admin
        [HttpGet("{id:guid}/admin")]
        public async Task<IActionResult> GetAdmin(Guid id)
        {
            var player = await _context.Players
                .Include(p => p.User)
                .Include(p => p.Game)
                    .ThenInclude(g => g.Rules)
                .Include(p => p.Game)
                    .ThenInclude(g => g.Players)
                        .ThenInclude(p2 => p2.User)
                .Include(p => p.Game)
                    .ThenInclude(g => g.Kills)
                        .ThenInclude(k => k.Killer)
                .Include(p => p.Game)
                    .ThenInclude(g => g.Kills)
                        .ThenInclude(k => k.Victim)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (player == null)
            {
                return NotFound("Player not found.");
            }

            var game = player.Game;
            var rules = game.Rules;

            // Get VipSettings for the game creator
            var creatorVip = await _context.VipSettings
                .FirstOrDefaultAsync(v => EF.Property<Guid>(v, "UserId") == game.CreatorId);

            var adminPlayers = game.Players.Select(p => new AdminPlayerItemDto
            {
                PlayerId = p.Id,
                Name = $"{p.User.FirstName} {p.User.LastName}",
                Username = p.UserName ?? string.Empty,
                HasImage = p.ProfileImageSource != null,
                IsAdmin = p.IsAdmin,
                IsSpectator = p.IsSpectator
            }).ToList();

            // Pending kills = kills that are not yet validated (IsValid = false)
            var pendingKills = game.Kills
                .Where(k => !k.IsValid)
                .Select(k => new AdminKillItemDto
                {
                    KillId = k.Id,
                    KillerName = k.Killer.UserName ?? string.Empty,
                    VictimName = k.Victim.UserName ?? string.Empty,
                    Weapon = k.Weapon,
                    Moment = k.Moment
                }).ToList();

            var dto = new AdminDataResponseDto
            {
                GameId = game.Id,
                HasStarted = game.HasStarted,
                GameName = game.Name,
                InviteLink = $"gotcha://join/{game.Id}",
                PlayerCount = game.Players.Count,
                MaxPlayers = game.MaxPlayers,
                Players = adminPlayers,
                PendingKills = pendingKills,

                ShowPlayerImages = rules.ShowPlayerImages,
                ShowGender = rules.ShowGender,
                EnforcePlayerImages = rules.EnforcePlayerImages,
                ShowRealNames = rules.ShowRealNames,
                ShowUsernames = rules.ShowUsernames,
                ShowLivingPlayerCount = rules.ShowLivingPlayerCount,
                ShowLivingPlayerNames = rules.ShowLivingPlayerNames,
                ShowLivingPlayerNamesToDeath = rules.ShowLivingPlayerNamesToDeath,

                IsAssassin = rules.IsAssassin,
                ShowHunter = rules.ShowHunter,
                IsChaos = rules.IsChaos,
                IsTimed = rules.IsTimed,
                CustomKillMethods = rules.CustomKillMethods,
                KillMethods = rules.KillMethods != null ? string.Join(", ", rules.KillMethods) : string.Empty,
                ChaosTimerMinHours = (int)rules.ChaosTimerMin.TotalHours,
                ChaosTimerMaxHours = (int)rules.ChaosTimerMax.TotalHours,
                TargetTimeOutHours = (int)rules.TargetTimeOut.TotalHours,
                CustomRulesText = rules.CustomRules != null ? string.Join("\n", rules.CustomRules) : string.Empty,

                AssassinModeUnlocked = creatorVip?.AssassinModeUnlocked ?? false,
                ChaosModeUnlocked = creatorVip?.ChaosModeUnlocked ?? false,
                TimedKillsUnlocked = creatorVip?.TimedKillsUnlocked ?? false
            };

            return Ok(dto);
        }

        #region Private Helpers

        private (TargetAssignment? Assignment, Player? Hunter) FindHunterForPlayer(Game game, Guid playerId)
        {
            TargetAssignment? assignment = game.Players
                .SelectMany(p => p.TargetAssignments)
                .FirstOrDefault(ta => ta.TargetId == playerId && ta.AssignmentStatus == AssignmentStatus.Ongoing);

            Player? hunter = null;
            if (assignment != null)
            {
                hunter = game.Players.FirstOrDefault(p => p.Id == assignment.HunterId);
            }

            return (assignment, hunter);
        }

        private async Task<Player?> LoadFullPlayer(Guid playerId)
        {
            return await _context.Players
                .Include(p => p.User)
                .Include(p => p.Game)
                    .ThenInclude(g => g.Rules)
                .Include(p => p.Game)
                    .ThenInclude(g => g.Players)
                        .ThenInclude(p2 => p2.User)
                .Include(p => p.Game)
                    .ThenInclude(g => g.Players)
                        .ThenInclude(p2 => p2.TargetAssignments)
                            .ThenInclude(ta => ta.Target)
                .Include(p => p.Game)
                    .ThenInclude(g => g.Players)
                        .ThenInclude(p2 => p2.TargetAssignments)
                            .ThenInclude(ta => ta.Kill)
                .Include(p => p.Game)
                    .ThenInclude(g => g.Kills)
                .Include(p => p.TargetAssignments)
                    .ThenInclude(ta => ta.Target)
                .FirstOrDefaultAsync(p => p.Id == playerId);
        }

        #endregion
    }
}

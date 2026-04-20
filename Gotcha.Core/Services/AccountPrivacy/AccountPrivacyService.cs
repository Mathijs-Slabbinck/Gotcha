using Gotcha.Core.Data;
using Gotcha.Core.Entities.Models;
using Gotcha.Core.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gotcha.Core.Services.AccountPrivacy
{
    public class AccountPrivacyService
    {
        private readonly GotchaDbContext _context;
        private readonly UserManager<GotchaUser> _userManager;

        public AccountPrivacyService(GotchaDbContext context, UserManager<GotchaUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Soft-delete + anonymize. Keeps game history intact under the name "Deleted User"
        // so other players' records stay valid.
        public async Task<bool> SoftDeleteAndAnonymizeAsync(Guid userId)
        {
            GotchaUser? user = await _context.Users
                .Include(u => u.ProfileImage)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return false;
            }

            if (user.ProfileImage != null)
            {
                _context.ProfileImages.Remove(user.ProfileImage);
            }

            user.FirstName = "Deleted";
            user.LastName = "User";
            user.Email = $"deleted_{user.Id}@gotcha.local";
            user.NormalizedEmail = user.Email.ToUpperInvariant();
            user.UserName = $"deleted_{user.Id}";
            user.NormalizedUserName = user.UserName.ToUpperInvariant();
            user.ProfileImageSource = null;
            user.GuardianEmail = null;
            user.GuardianConsentToken = null;
            user.PhoneNumber = null;
            user.IsDeleted = true;

            // Rotate the security stamp inline so any existing cookies/JWTs are invalidated.
            // Setting it on the tracked entity means a single UpdateAsync saves everything
            // (profile anonymization + ProfileImage removal + stamp rotation) in one round-trip.
            user.SecurityStamp = Guid.NewGuid().ToString();

            IdentityResult result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<UserDataExport?> BuildExportAsync(Guid userId)
        {
            GotchaUser? user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return null;
            }

            List<GameExportRecord> games = await _context.Players
                .AsNoTracking()
                .Where(p => p.UserId == userId)
                .Select(p => new GameExportRecord
                {
                    PlayerId = p.Id,
                    GameName = p.Game.Name,
                    CreationDate = p.Game.CreationDate,
                    StartDate = p.Game.StartDate,
                    EndDate = p.Game.EndDate,
                    HasStarted = p.Game.HasStarted,
                    IsFinished = p.Game.IsFinished,
                    MyUsernameInGame = p.UserName,
                    IsAlive = p.IsAlive,
                    IsAdmin = p.IsAdmin,
                    IsSpectator = p.IsSpectator
                })
                .ToListAsync();

            List<Guid> playerIds = games.Select(g => g.PlayerId).ToList();

            // One query for both sides of every kill: partition in memory.
            var killRecords = await _context.Kills
                .AsNoTracking()
                .Where(k => playerIds.Contains(k.KillerId) || playerIds.Contains(k.VictimId))
                .Select(k => new
                {
                    k.Moment,
                    k.Weapon,
                    k.IsValid,
                    IsKiller = playerIds.Contains(k.KillerId)
                })
                .ToListAsync();

            List<KillExportRecord> kills = killRecords
                .Where(k => k.IsKiller)
                .Select(k => new KillExportRecord
                {
                    Moment = k.Moment,
                    Weapon = k.Weapon,
                    IsValid = k.IsValid,
                    Role = KillRole.Killer
                })
                .ToList();

            List<KillExportRecord> deaths = killRecords
                .Where(k => !k.IsKiller)
                .Select(k => new KillExportRecord
                {
                    Moment = k.Moment,
                    Weapon = k.Weapon,
                    IsValid = k.IsValid,
                    Role = KillRole.Victim
                })
                .ToList();

            return new UserDataExport
            {
                Profile = new UserProfileExport
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Email = user.Email,
                    Gender = user.Gender.ToString(),
                    BirthDate = user.BirthDate,
                    AccountCreationDate = user.AccountCreationDate,
                    PhoneNumber = user.PhoneNumber,
                    GuardianEmail = user.GuardianEmail,
                    HasGuardianConsent = user.HasGuardianConsent,
                    GuardianConsentDate = user.GuardianConsentDate
                },
                Games = games,
                Kills = kills,
                Deaths = deaths
            };
        }
    }
}

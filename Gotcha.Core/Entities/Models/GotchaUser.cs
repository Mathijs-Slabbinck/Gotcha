using Gotcha.Core.Enums;
using Microsoft.AspNetCore.Identity;

namespace Gotcha.Core.Entities.Models
{
    public class GotchaUser : IdentityUser<Guid>
    {
        #region Properties
        public override Guid Id { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; }
        public string LastName { get; set; }
        // UserName is inherited from IdentityUser, but we need to override it to make it required (non-nullable)
        public required override string UserName { get; set; }
        // Inherited from IdentityUser (GotchaUser.Email)
        public required override string Email { get; set; }
        public string? ProfileImageSource { get; set; }
        public Genders Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime AccountCreationDate { get; init; } = DateTime.UtcNow;
        public ICollection<Player> PlayerAccounts { get; set; } = new List<Player>();
        public VipSettings VipSettings { get; set; } = new VipSettings();
        public ProfileImage? ProfileImage { get; set; }

        // Guardian consent fields (COPPA/GDPR — required for users under 16)
        public string? GuardianEmail { get; set; }
        public bool HasGuardianConsent { get; set; }
        public DateTime? GuardianConsentDate { get; set; }
        public string? GuardianConsentToken { get; set; }

        // Account deletion (soft delete + anonymize, full deletion on request)
        public bool IsDeleted { get; set; }
        #endregion

        #region Query Methods
        public bool NeedsGuardianConsent() => GuardianEmail != null && !HasGuardianConsent;

        public IEnumerable<Game> GetAllGamesPlayed()
        {
            return PlayerAccounts
                    .Select(p => p.Game);
        }

        public IEnumerable<Game> GetAllActiveGames()
        {
            return PlayerAccounts
                    .Select(p => p.Game)
                    .Where(g => g.IsFinished == false);
        }

        public IEnumerable<Game> GetAllFinishedGames()
        {
            return PlayerAccounts
                    .Select(p => p.Game)
                    .Where(g => g.IsFinished == true);
        }

        public IEnumerable<Game> GetAllWonGames()
        {
            return PlayerAccounts
                    .Where(p => p.Game.Winner != null && p.Game.Winner.Id == p.Id)
                    .Select(p => p.Game);
        }

        public IEnumerable<Kill> GetAllKills()
        {
            return PlayerAccounts
                    .SelectMany(p => p.Game.Kills.Where(k => k.KillerId == p.Id && k.IsValid));
        }

        public IEnumerable<Kill> GetAllDeaths()
        {
            return PlayerAccounts
                    .SelectMany(p => p.Game.Kills.Where(k => k.VictimId == p.Id));
        }
        #endregion

        public override string ToString()
        {
            return $"({FirstName} {LastName} - ({UserName}))";
        }
    }
}
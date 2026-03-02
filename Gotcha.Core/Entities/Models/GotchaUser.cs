using System;
using System.Collections.Generic;
using System.Linq;
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
        public List<Player> PlayerAccounts { get; set; } = new List<Player>();
        public VipSettings VipSettings { get; set; } = new VipSettings();

        // Guardian consent fields (COPPA/GDPR — required for users under 16)
        public string? GuardianEmail { get; set; }
        public bool HasGuardianConsent { get; set; }
        public DateTime? GuardianConsentDate { get; set; }
        #endregion

        #region Query Methods
        public List<Game> GetAllGamesPlayed()
        {
            return PlayerAccounts
                    .Select(p => p.Game)
                    .ToList();
        }

        public List<Game> GetAllActiveGames()
        {
            return PlayerAccounts
                    .Select(p => p.Game)
                    .Where(g => g.IsFinished == false)
                    .ToList();
        }

        public List<Game> GetAllFinishedGames()
        {
            return PlayerAccounts
                    .Select(p => p.Game)
                    .Where(g => g.IsFinished == true)
                    .ToList();
        }

        public List<Game> GetAllWonGames()
        {
            return PlayerAccounts
                    .Where(p => p.Game.Winner != null && p.Game.Winner.Id == p.Id)
                    .Select(p => p.Game)
                    .ToList();
        }

        public List<Kill> GetAllKills()
        {
            return PlayerAccounts
                    .SelectMany(p => p.Game.Kills.Where(k => k.KillerId == p.Id && k.IsValid))
                    .ToList();
        }

        public List<Kill> GetAllDeaths()
        {
            return PlayerAccounts
                    .SelectMany(p => p.Game.Kills.Where(k => k.VictimId == p.Id))
                    .ToList();
        }
        #endregion

        public override string ToString()
        {
            return $"({FirstName} {LastName} - ({UserName}))";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Gotcha.Core.Enums;

namespace Gotcha.Core.Entities.Models
{
    public class User
    {
        #region Properties
        public Guid Id { get; init; } = Guid.NewGuid();
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string? ProfileImageSource { get; set; }
        public Genders Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime AccountCreationDate { get; init; } = DateTime.UtcNow;
        public List<Player> PlayerAccounts { get; set; } = new List<Player>();
        public VipSettings VipSettings { get; set; } = new VipSettings();
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
            return $"({FirstName} {LastName} - ({Username}))";
        }
    }
}

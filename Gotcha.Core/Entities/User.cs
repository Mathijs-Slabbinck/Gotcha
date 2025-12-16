using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Gotcha.Core.Services;

namespace Gotcha.Core.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string? ProfileImageSource { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime AccountCreationDate { get; set; }
        public List<Player> PlayerAccounts { get; set; }

        public User(string firstName, string lastName, string username, string email, DateTime birthdate)
        {
            ThirdLineValidationService thirdLineValidationService = new ThirdLineValidationService();
            Id = Guid.NewGuid();

            // this should be validated already, but this an extra layer for extra robustness & safety
            if (thirdLineValidationService.IsInputClean(firstName))
            {
                FirstName = firstName;
            }
            else
            {
                throw new ArgumentException("Invalid characters in first name.");
            }

            // this should be validated already, but this an extra layer for extra robustness & safety
            if (thirdLineValidationService.IsInputClean(lastName))
            {
                LastName = lastName;
            }
            else
            {
                throw new ArgumentException("Invalid characters in last name.");
            }

            // this should be validated already, but this an extra layer for extra robustness & safety
            if (thirdLineValidationService.IsInputClean(username))
            {
                Username = username;
            }
            else
            {
                throw new ArgumentException("Invalid characters in username.");
            }

            // this should be validated already, but this an extra layer for extra robustness & safety
            if (thirdLineValidationService.IsValidEmail(email))
            {
                Email = email;
            }
            else
            {
                throw new ArgumentException("Invalid email format.");
            }

            BirthDate = birthdate;

            AccountCreationDate = DateTime.UtcNow;
            PlayerAccounts = new List<Player>();
        }

        public User(string firstName, string lastName, string username, string email, DateTime birthdate, DateTime accountCreationDate) : this(firstName, lastName, username, email, birthdate)
        {
            AccountCreationDate = accountCreationDate;
        }

        public User(string firstName, string lastName, string username, string email, DateTime birthdate, DateTime accountCreationDate, string profileImageSource) : this(firstName, lastName, username, email, birthdate, accountCreationDate)
        {
            ProfileImageSource = profileImageSource;
        }

        public User(string firstName, string lastName, string username, string email, DateTime birthdate, DateTime accountCreationDate, string profileImageSource, List<Player> playerAccounts) : this(firstName, lastName, username, email, birthdate, accountCreationDate, profileImageSource)
        {
            PlayerAccounts = playerAccounts;
        }


        public List<Game> GetAllGamesPlayed()
        {
            return PlayerAccounts.Select(p => p.Game).ToList();
        }

        public List<Game> GetAllActiveGames()
        {
            return PlayerAccounts.Select(p => p.Game)
                    .Where(g => g.IsFinished == false)
                    .ToList();
        }

        public List<Game> GetAllFinishedGames()
        {
            return PlayerAccounts.Select(p => p.Game)
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

        public List<Kill> GetAllKills() {
            return PlayerAccounts
                    .SelectMany(p => p.Game.Kills)
                    .Where(k => k.KillerId == this.Id && k.IsValid)
                    .ToList();
        }

        public List<Kill> GetAllDeaths()
        {
            return PlayerAccounts
                    .SelectMany(p => p.Game.Kills)
                    .Where(k => k.VictimId == this.Id)
                    .ToList();
        }
    }
}

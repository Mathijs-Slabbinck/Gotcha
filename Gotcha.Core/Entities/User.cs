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
        private readonly Guid _id;
        private string firstName;
        private string lastName;
        private string username;
        private string email;
        private string? profileImageSource;
        private DateTime birthDate;
        private readonly DateTime _accountCreationDate;
        public List<Player> PlayerAccounts { get; set; }

        public User(string firstName, string lastName, string username, string email, DateTime birthdate)
        {
            _id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Email = email;
            BirthDate = birthdate;
            _accountCreationDate = DateTime.UtcNow;
            PlayerAccounts = new List<Player>();
        }

        public User(string firstName, string lastName, string username, string email, DateTime birthdate, Guid id) : this(firstName, lastName, username, email, birthdate)
        {
            _id = id;
        }

        public User(string firstName, string lastName, string username, string email, DateTime birthdate, Guid id, DateTime accountCreationDate) : this(firstName, lastName, username, email, birthdate, id)
        {
            _accountCreationDate = accountCreationDate;
        }

        public User(string firstName, string lastName, string username, string email, DateTime birthdate, Guid id, DateTime accountCreationDate, string profileImageSource) : this(firstName, lastName, username, email, birthdate, id, accountCreationDate)
        {
            ProfileImageSource = profileImageSource;
        }

        public User(string firstName, string lastName, string username, string email, DateTime birthdate, Guid id, DateTime accountCreationDate, string profileImageSource, List<Player> playerAccounts) : this(firstName, lastName, username, email, birthdate, id, accountCreationDate, profileImageSource)
        {
            PlayerAccounts = playerAccounts;
        }

        public Guid Id
        {
            get { return _id; }
        }

        public string FirstName
        {
            get { return firstName; }
            set
            {
                if (!ThirdLineValidationService.IsValidLength(value, 2, 70))
                {
                    throw new ArgumentException("Username must be between 2 and 25 characters long.");
                }

                string sanitizedValue = ThirdLineValidationService.SaniziteInput(value);

                if (ThirdLineValidationService.IsInputClean(sanitizedValue))
                {
                    firstName = sanitizedValue;
                }
                else
                {
                    throw new ArgumentException("Invalid characters in first name.");
                }
            }
        }

        public string LastName
        {
            get { return lastName; }
            set
            {
                if (!ThirdLineValidationService.IsValidLength(value, 2, 100))
                {
                    throw new ArgumentException("Username must be between 2 and 25 characters long.");
                }

                string sanitizedValue = ThirdLineValidationService.SaniziteInput(value);

                if (ThirdLineValidationService.IsInputClean(sanitizedValue))
                {
                    lastName = sanitizedValue;
                }
                else
                {
                    throw new ArgumentException("Invalid characters in first name.");
                }
            }
        }

        public string Username
        {
            get { return username; }
            set
            {
                if(!ThirdLineValidationService.IsValidLength(value, 2, 25))
                {
                    throw new ArgumentException("Username must be between 2 and 25 characters long.");
                }
                
                string sanitizedValue = ThirdLineValidationService.SaniziteInput(value);
                
                if (ThirdLineValidationService.IsInputClean(sanitizedValue))
                {
                    username = sanitizedValue;
                }
                else
                {
                    throw new ArgumentException("Invalid characters in username.");
                }
            }
        }

        public string Email
        {
            get { return email; }
            set
            {
                if (ThirdLineValidationService.IsValidEmail(value))
                {
                    email = value;
                }
                else
                {
                    throw new ArgumentException("Invalid email format.");
                }
            }
        }

        public DateTime BirthDate
        {
            get { return birthDate; }
            set
            {
                if (value <= DateTime.UtcNow)
                {
                    birthDate = value;
                }
                else if ((DateTime.UtcNow.Year - value.Year) >= 180)
                {
                    throw new ArgumentException("Birthdate indicates age over 180 years, which is not allowed (not possible).");
                }
                else
                {
                    throw new ArgumentException("Birthdate cannot be in the future.");
                }
            }
        }

        public string? ProfileImageSource
        {
            get { return profileImageSource; }
            set
            {
                if (ThirdLineValidationService.IsCleanProfileImageSource(value))
                {
                    profileImageSource = value;
                }
                else
                {
                    throw new ArgumentException("Invalid characters in profile image source.");
                }
            }
        }

        public DateTime AccountCreationDate
        {
            get { return _accountCreationDate; }
        }

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

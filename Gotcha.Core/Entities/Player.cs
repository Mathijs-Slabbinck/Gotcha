using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using Gotcha.Core.Services;

namespace Gotcha.Core.Entities
{
    public class Player
    {
        private readonly Guid _id;
        private readonly Guid _userId;
        private readonly User _user;
        private readonly Guid _gameId;
        private readonly Game _game;
        private readonly string _playerName;
        public string? profileImageSource;
        public bool IsAlive { get; set; }
        public string Notes { get; set; }
        public ICollection<TargetAssignment> TargetAssignments { get; set; }

        public Player(User user, Game game)
        {
            _id = Guid.NewGuid();
            _userId = user.Id;
            _user = user;
            _gameId = game.Id;
            _game = game;

            if (game.Rules.UseRealNames)
            {
                _playerName = user.FirstName + " " + user.LastName;
            }
            else
            {
                _playerName = user.Username;
            }

            if (game.Rules.EnforcePlayerImages && user.ProfileImageSource == null)
            {
                throw new Exception("This game requires players to have profile images, but the user does not have one set.");
            }

            ProfileImageSource = user.ProfileImageSource;

            Notes = string.Empty;
            IsAlive = true;
            TargetAssignments = new List<TargetAssignment>();
        }

        public Player(User user, Game game, string username) : this(user, game)
        {
            if (game.Rules.UseRealNames)
            {
                _playerName = user.FirstName + " " + user.LastName;
                // throw an error since this should never happen; makes debugging easier
                throw new ArgumentException("The game is set to use real names, so a custom username cannot be set.");
            }
            else
            {
                _playerName = username;
            }
        }

        public Player(User user, Game game, string? username, string profileImageSource) : this(user, game)
        {
            if (game.Rules.UseRealNames && username != null)
            {
                _playerName = user.FirstName + " " + user.LastName;
                // throw an error since this should never happen; makes debugging easier
                throw new ArgumentException("The game is set to use real names, so a custom username cannot be set.");
            }
            else
            {
                if(username == null)
                {
                    username = user.Username;
                }
                _playerName = username;
            }

            ProfileImageSource = profileImageSource;
        }

        public Player(Guid id, Guid userId, User user, Guid gameId, Game game, string playerName, string? profileImageSource, bool isAlive, string notes, ICollection<TargetAssignment> targetAssignments)
        {
            _id = id;
            _userId = userId;
            _user = user;
            _gameId = gameId;
            _game = game;
            _playerName = playerName;
            if (game.Rules.EnforcePlayerImages && user.ProfileImageSource == null)
            {
                throw new Exception("This game requires players to have profile images, but the user does not have one set.");
            }
            ProfileImageSource = profileImageSource;
            IsAlive = isAlive;
            Notes = notes;
            TargetAssignments = targetAssignments;
        }

        public Guid Id
        {
            get { return _id; }
        }

        public Guid UserId
        {
            get { return _userId; }
        }

        public User User
        {
            get { return _user; }
        }

        public Guid GameId
        {
            get { return _gameId; }
        }

        public Game Game
        {
            get { return _game; }
        }

        public string PlayerName
        {
            get { return _playerName; }
        }

        public string? ProfileImageSource
        {
            get { return profileImageSource; }
            set
            {
                if (!Game.Rules.ShowPlayerImages)
                {
                    throw new Exception("The game settings do not allow player images to be set.");
                }

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
        public override string ToString()
        {
            return PlayerName;
        }
    }
}

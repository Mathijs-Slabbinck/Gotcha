using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using Gotcha.Core.Exceptions;
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

        /* Standard constructor
         * Used when creating a new Player object when a User joins a game */
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

        /* Constructor to set a custom username
         * Only used if the game rules do not enforce real names */
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

        /* Constructor to set a custom username (optionally) and profile image (optionally)
         * Used when creating a new player using custom rules */
        public Player(User user, Game game, string? username, string? profileImageSource) : this(user, game)
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

            // checks are done in the property setter
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

                if (this.Game.Rules.EnforcePlayerImages && value == null)
                {
                    throw new Exception("This game requires players to have profile images, but the user does not have one set.");
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

        public Player GetCurrentTarget()
        {
            if (!IsAlive)
            {
                throw new GameStateException($"Player {PlayerName} is not alive and has no current target.");
            }

            if (!Game.HasStarted)
            {
                throw new GameStateException("Game has not started yet.");
            }

            TargetAssignment? currentAssignment = TargetAssignments
                                        .FirstOrDefault(ta => ta.TargetId == this.Id && ta.AssignmentStatus == Enums.AssignmentStatus.Ongoing);

            if (currentAssignment == null)
            {
                throw new InvalidTargetAssignmentException(this.Id, Guid.Empty, "No ongoing target assignment found for this player.");
            }

            if(currentAssignment.Target == null)
            {
                throw new InvalidTargetAssignmentException(this.Id, currentAssignment.TargetId, "The current target assignment does not have a valid target.");
            }

            return currentAssignment.Target;
        }

        public Player GetCurrentHunter()
        {
            TargetAssignment? currentAssignment = TargetAssignments
                                        .FirstOrDefault(ta => ta.TargetId == this.Id && ta.AssignmentStatus == Enums.AssignmentStatus.Ongoing);
            
            if (currentAssignment == null)
            {
                throw new InvalidOperationException("No ongoing target assignment found for this player.");
            }
            if (currentAssignment.Hunter == null)
            {
                throw new InvalidOperationException("The current target assignment does not have a valid hunter.");
            }
            return currentAssignment.Hunter;
        }

        public List<Player> GetKilledPlayers()
        {
            List<Player> killedPlayers = TargetAssignments
                                            .Where(ta => ta.Kill != null)
                                            .Select(ta => ta.Target)
                                            .ToList();

            return killedPlayers;
        }

        public Player? GetKiller()
        {
            TargetAssignment? killAssignment = TargetAssignments
                                     .FirstOrDefault(ta => ta.Kill != null);

            return killAssignment?.Hunter;
        }
    }
}

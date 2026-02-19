using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using Gotcha.Core.Exceptions;
using Gotcha.Core.Services.ValidationServices;

namespace Gotcha.Core.Entities
{
    public class Player
    {
        private readonly Guid _id;
        private readonly Guid _userId;
        private readonly User _user;
        private readonly Guid _gameId;
        private readonly Game _game;
        private string username;
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
            _gameId = game.Id;
            _user = user;
            _game = game;

            if (game.Rules.ShowUsernames)
            {
                Username = user.Username;
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
            Username = username; // checks are done in the property setter
        }

        /* Constructor to set a custom username (optionally) and profile image (optionally)
         * Used when creating a new player using custom rules */
        public Player(User user, Game game, string? username, string? profileImageSource) : this(user, game)
        {
            if(username == null)
            {
                username = user.Username;
            }

            Username = username; // checks are done in the property setter
            ProfileImageSource = profileImageSource; // checks are done in the property setter
        }

        public Player(Guid id, Guid userId, User user, Guid gameId, Game game, string? username, string? profileImageSource, bool isAlive, string notes, ICollection<TargetAssignment> targetAssignments) : this (user, game, username, profileImageSource)
        {
            _id = id;
            _userId = userId;
            _user = user;
            _gameId = gameId;
            _game = game;
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
            get
            {
                return _user;
                throw new NotImplementedException("FIX THIS SHIT");
            }
        }

        public Guid GameId
        {
            get { return _gameId; }
        }

        public Game Game
        {
            get
            {
                return _game;
                throw new NotImplementedException("FIX THIS SHIT");
            }
        }

        public string Username
        {
            get {
                if (Game.Rules.ShowUsernames)
                {
                    if (username != null)
                    {
                        return username;
                    }
                    else
                    {
                        Console.WriteLine("⚠️⚠⚠️ Show Usernames are turned ON but but parameter username in Player constructor is null.\nReturning user's standard username in stead!");
                        return User.Username;
                    }
                }
                else
                {
                    Console.WriteLine("⚠️⚠️ Show Usernames are turned OFF but but Username getter was accessed.\nReturning user's standard username in stead!");
                    return User.Username;
                }
            }

            set {
                if (Game.Rules.ShowUsernames)
                {
                    if(username != null)
                    {
                        if (LastLineValidationService.IsReservedUsername(value))
                        {
                            throw new ValidationException("username", "Player", "This username is reserved and cannot be used.");
                        }
                        else
                        {
                            username = value;
                        }
                    }
                    else
                    {
                        Console.WriteLine("⚠️⚠️ Show usernames are turned ON in this game and username was set to null.\nSetting to user's default username in stead!");
                        username = User.Username;
                    }
                }
                else
                {
                    throw new GameRuleViolationException("ShowUsernames", "Show Usernames are turned OFF but but parameter username in Player constructor was set.");
                }
            }
        }

        public string DisplayName
        {
            get
            {
                if (Game.Rules.ShowUsernames && !Game.Rules.ShowRealNames)
                {
                    return Username;
                }
                else if (Game.Rules.ShowUsernames && Game.Rules.ShowRealNames)
                {
                    return $"{User.FirstName} {User.LastName} ({Username})";
                }
                else if(!Game.Rules.ShowUsernames && Game.Rules.ShowRealNames)
                {
                    return $"{User.FirstName} {User.LastName}";
                }
                else
                {
                    throw new GameStateException("Showusernames and ShowRealNames cannot both be false in Player entity!");
                }
            }
        }

        public string? ProfileImageSource
        {
            get { return profileImageSource; }
            set
            {
                if (!Game.Rules.ShowPlayerImages)
                {
                    throw new GameRuleViolationException("ShowPlayerImages", "The game settings do not allow player images to be set.");
                }

                if (this.Game.Rules.EnforcePlayerImages && value == null)
                {
                    throw new GameRuleViolationException("EnforcePlayerImages", "This game requires players to have profile images, but the user does not have one set.");
                }

                if (LastLineValidationService.IsAllowedImageUrl(value))
                {
                    profileImageSource = value;
                }
                else
                {
                    throw new ValidationException("ProfileImageSource", "Player", "Invalid characters in profile image source.");
                }
            }
        }
        public override string ToString()
        {
            return Username;
        }

        public Player GetCurrentTarget()
        {
            if (!IsAlive)
            {
                throw new GameStateException($"Player {Username} is not alive and has no current target.");
            }

            if (!Game.HasStarted)
            {
                throw new GameStateException("Game has not started yet.");
            }

            TargetAssignment? currentAssignment = TargetAssignments
                                                            .FirstOrDefault(ta => ta.HunterId == Id &&
                                                                                  ta.AssignmentStatus == Enums.AssignmentStatus.Ongoing);

            if (currentAssignment == null)
            {
                throw new InvalidTargetAssignmentException(Id, Guid.Empty, "No ongoing target assignment found for this player.");
            }

            if(currentAssignment.Target == null)
            {
                throw new InvalidTargetAssignmentException(Id, currentAssignment.TargetId, "The current target assignment does not have a valid target.");
            }

            return currentAssignment.Target;
        }

        public Player GetCurrentHunter()
        {
            TargetAssignment? currentAssignment = TargetAssignments
                                                            .FirstOrDefault(ta => ta.TargetId == this.Id &&
                                                                                  ta.AssignmentStatus == Enums.AssignmentStatus.Ongoing);
            
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

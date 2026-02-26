using Gotcha.Core.Enums;

namespace Gotcha.Core.Entities.Models
{
    public class Player
    {
        #region Properties
        public Guid Id { get; init; } = Guid.NewGuid();
        public Guid UserId { get; init; }
        public User User { get; init; }
        public Guid GameId { get; init; }
        public Game Game { get; init; }
        public string? Username { get; set; }
        public string? ProfileImageSource { get; set; }
        public bool IsAlive { get; set; } = true;
        public bool IsAdmin { get; set; } = false;
        public bool IsSpectator { get; set; } = false;
        public string Notes { get; set; } = string.Empty;
        public ICollection<TargetAssignment> TargetAssignments { get; set; } = new List<TargetAssignment>();
        #endregion

        #region Computed Properties
        public string DisplayName
        {
            get
            {
                if (Game.Rules.ShowUsernames && !Game.Rules.ShowRealNames)
                {
                    if (Username == null)
                        return User.Username;
                    else
                        return Username;
                }
                else if (Game.Rules.ShowUsernames && Game.Rules.ShowRealNames)
                {
                    return $"({Username ?? User.Username})"; // we will get the real name from User seperately in this case
                }
                else
                {
                    return $"{User.FirstName} {User.LastName}";
                }
            }
        }
        #endregion

        #region Query Methods
        public Player? GetCurrentTarget()
        {
            TargetAssignment? currentAssignment = TargetAssignments
                                                            .FirstOrDefault(ta => ta.HunterId == Id &&
                                                                                  ta.AssignmentStatus == AssignmentStatus.Ongoing);
            return currentAssignment?.Target;
        }

        public Player? GetCurrentHunter(Game game)
        {
            TargetAssignment? currentAssignment = game.Players
                                                        .SelectMany(p => p.TargetAssignments)
                                                        .FirstOrDefault(ta => ta.TargetId == Id &&
                                                                              ta.AssignmentStatus == AssignmentStatus.Ongoing);
            return currentAssignment?.Hunter;
        }

        public List<Player> GetKilledPlayers()
        {
            return TargetAssignments
                            .Where(ta => ta.Kill != null)
                            .Select(ta => ta.Target)
                            .ToList();
        }

        public Player? GetKiller(Game game)
        {
            TargetAssignment? killAssignment = game.Players
                                                        .SelectMany(p => p.TargetAssignments)
                                                        .FirstOrDefault(ta => ta.TargetId == Id &&
                                                                              ta.Kill != null);
            return killAssignment?.Hunter;
        }
        #endregion

        public override string ToString()
        {
            if(Username == null)
            {
                return $"{User.FirstName} + {User.LastName}";
            }

            return Username;
        }
    }
}

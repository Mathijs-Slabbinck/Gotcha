using Gotcha.Core.Enums;
using Gotcha.Core.Exceptions;

namespace Gotcha.Core.Entities
{
    public class Rules
    {
        public readonly Guid _id;
        private GameModes gameMode;
        private bool showPlayerImages;
        private bool enforcePlayerImages;
        private bool showRealNames;
        private bool showUsernames;
        private bool isTimed;
        private TimeSpan targetTimeOut;
        private string? customRules;
        private bool customKillMethods;
        private List<string>? killMethods;
        private bool randomTargetAssignment;
        private bool isChaos; // target changes every x amount of time
        private TimeSpan chaosTimer; // timer for chaos mode

        /* Standard constructor
         * Used when creating a new Rules object with default settings */
        public Rules()
        {
            _id = Guid.NewGuid();
            GameMode = GameModes.Gotcha;
            ShowPlayerImages = false;
            EnforcePlayerImages = false;
            ShowRealNames = true;
            showUsernames = false;
            IsTimed = false;
            TargetTimeOut = Timeout.InfiniteTimeSpan;
            CustomRules = string.Empty;
            CustomKillMethods = false;
            RandomTargetAssignment = false;
            isChaos = false;
            ChaosTimer = Timeout.InfiniteTimeSpan;
        }

        /* Constructor with game mode
         * Used when creating a new Rules object with a specific game mode and no other custom settings */
        public Rules(GameModes gameMode) : this()
        {
            GameMode = gameMode;
        }

        /* Constructor with ShowPlayerImages and EnforcePlayerImages
         * Used when creating a new Rules object with specific settings for player images */
        public Rules(bool showPlayerImages, bool enforcePlayerImages) : this()
        {
            EnforcePlayerImages = enforcePlayerImages;

            // If player images are enforced, they must be shown
            if (enforcePlayerImages && !showPlayerImages)
            {
                ShowPlayerImages = true;
                return;
            }

            ShowPlayerImages = showPlayerImages;
        }

        /* Constructor with GameMode, ShowPlayerImages and EnforcePlayerImages
        * Used when creating a new Rules object with a custom gameMode & specific settings for player images */
        public Rules(GameModes gameMode, bool showPlayerImages, bool enforcePlayerImages) : this(showPlayerImages, enforcePlayerImages)
        {
            GameMode = gameMode;
        }


        // Full constructor
        public Rules(Guid id, GameModes gameMode, bool showPlayerImages, bool enforcePlayerImages, bool showRealNames, bool showUsernames, bool isTimed, TimeSpan? targetTimeOut, string? customRules, bool customKillMethods, List<string>? killMethods, bool randomTargetAssignment, bool isChaos, TimeSpan? chaosTimer)
        {
            _id = id;
            GameMode = gameMode;
            EnforcePlayerImages = enforcePlayerImages;
            // If player images are enforced, they must be shown
            if (enforcePlayerImages && !showPlayerImages)
            {
                ShowPlayerImages = true;
            }
            else
            {
                ShowPlayerImages = showPlayerImages;
            }

            ShowRealNames = showRealNames;
            ShowUsernames = showUsernames;
            IsTimed = isTimed;

            if (!IsTimed)
            {
                TargetTimeOut = Timeout.InfiniteTimeSpan;
            }
            else
            {
                if(targetTimeOut == null)
                {
                    throw new GameRuleViolationException("IsTimed", "targetTimeOut must be set when IsTimed is true.");
                }

                TargetTimeOut = (TimeSpan)targetTimeOut;
            }

            CustomRules = customRules;
            CustomKillMethods = customKillMethods;
            KillMethods = killMethods;
            RandomTargetAssignment = randomTargetAssignment;
            IsChaos = isChaos;

            if (!IsChaos)
            {
                TargetTimeOut = Timeout.InfiniteTimeSpan;
            }
            else
            {
                if (chaosTimer == null)
                {
                    throw new GameRuleViolationException("IsChaos", "chaosTimer must be set when IsChaos is true.");
                }

                ChaosTimer = (TimeSpan)chaosTimer;
            }
        }

        public Guid Id
        {
            get { return Id; }
        }
        public GameModes GameMode
        {
            get { return gameMode; }
            set { gameMode = value; }
        }

        public bool ShowPlayerImages
        {
            get { return showPlayerImages; }
            set { showPlayerImages = value; }
        }

        public bool EnforcePlayerImages
        {
            get { return enforcePlayerImages; }
            set { enforcePlayerImages = value; }
        }

        public bool ShowRealNames
        {
            get { return showRealNames; }
            set {
                if(!ShowUsernames && value == false)
                {
                    throw new InvalidOperationException("Cannot set ShowRealNames on false when ShowUsernames is already false (cannot both be false at the same time).");
                }

                showRealNames = value;
            }
        }

        public bool ShowUsernames
        {
            get { return showUsernames; }
            set {
                if(!ShowRealNames && value == false)
                {
                    throw new InvalidOperationException("Cannot set ShowUsernames on false when ShowRealNames is already false (cannot both be false at the same time).");
                }

                showUsernames = value;
            }
        }

        public bool IsTimed
        {
            get { return isTimed; }
            set { isTimed = value; }
        }

        public TimeSpan TargetTimeOut
        {
            get { return targetTimeOut; }
            set { targetTimeOut = value; }
        }

        public string? CustomRules
        {
            get { return customRules; }
            set { customRules = value; }
        }

        public bool CustomKillMethods
        {
            get { return customKillMethods; }
            set { customKillMethods = value; }
        }

        public List<string> KillMethods
        {
            get {
                if (CustomKillMethods)
                {
                    if (killMethods != null)
                    {
                        return killMethods;
                    }
                    else
                    {
                        throw new GameRuleViolationException("CustomKillMethods", "CustomKillMethods is enabled but KillMethods property getter in Rules was requested and returned null.");
                    }
                }
                else
                {
                    throw new GotchaInvalidOperationsExceptions("CustomKillMethods is enabled but KillMethods property getter in Rules was requested.\nThis is only available if CustomKillMethods is enabled!.");
                }
            }
            set { killMethods = value; }
        }

        public bool RandomTargetAssignment
        {
            get { return randomTargetAssignment; }
            set { randomTargetAssignment = value; }
        }

        public bool IsChaos
        {
            get { return isChaos; }
            set { isChaos = value; }
        }

        public TimeSpan ChaosTimer
        {
            get {
                if (!isChaos)
                {
                    throw new GotchaInvalidOperationsExceptions("IsChaos is disabled but ChaosTimer property getter in Rules was requested.\nThis is only available if IsChaos is enabled!.");
                }

                return chaosTimer;
            }
            set { chaosTimer = value; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gotcha.Core.Enums;

namespace Gotcha.Core.Entities
{
    public class Rules
    {
        private GameModes gameMode;
        private bool showPlayerImages;
        private bool enforcePlayerImagese;
        private bool useRealNames;
        private bool isTimed;
        private TimeSpan? targetTimeOut;
        private string customRules;
        private bool customKillMethods;
        private List<string> killMethods;
        private bool randomTargetAssignment;

        /* Standard constructor
         * Used when creating a new Rules object with default settings */
        public Rules()
        {
            GameMode = GameModes.Gotcha;
            ShowPlayerImages = false;
            EnforcePlayerImages = false;
            UseRealNames = true;
            IsTimed = false;
            TargetTimeOut = null;
            CustomRules = string.Empty;
            CustomKillMethods = false;
            RandomTargetAssignment = false;
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


        // Full constructor
        public Rules(GameModes gameMode, bool showPlayerImages, bool enforcePlayerImages, bool useRealNames, bool isTimed, TimeSpan? targetTimeOut, string customRules, bool customKillMethods, List<string> killMethods, bool randomTargetAssignment)
        {
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
            UseRealNames = useRealNames;
            IsTimed = isTimed;
            TargetTimeOut = targetTimeOut;
            CustomRules = customRules;
            CustomKillMethods = customKillMethods;
            KillMethods = killMethods;
            RandomTargetAssignment = randomTargetAssignment;
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
            get { return enforcePlayerImagese; }
            set { enforcePlayerImagese = value; }
        }

        public bool UseRealNames
        {
            get { return useRealNames; }
            set { useRealNames = value; }
        }

        public bool IsTimed
        {
            get { return isTimed; }
            set { isTimed = value; }
        }

        public TimeSpan? TargetTimeOut
        {
            get { return targetTimeOut; }
            set { targetTimeOut = value; }
        }

        public string CustomRules
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
            get { return killMethods; }
            set { killMethods = value; }
        }

        public bool RandomTargetAssignment
        {
            get { return randomTargetAssignment; }
            set { randomTargetAssignment = value; }
        }
    }
}

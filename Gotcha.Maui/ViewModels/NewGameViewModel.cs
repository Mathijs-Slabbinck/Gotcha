using Gotcha.Maui.ViewModels.BaseViewModels;
using System.Windows.Input;

namespace Gotcha.Maui.ViewModels
{
    public class NewGameViewModel : PageBaseViewModel
    {
        private string gameName = string.Empty;
        public string GameName
        {
            get { return gameName; }
            set { SetProperty(ref gameName, value); }
        }

        private string customRules = string.Empty;
        public string CustomRules
        {
            get { return customRules; }
            set { SetProperty(ref customRules, value); }
        }

        // Display settings
        private bool showPlayerImages;
        public bool ShowPlayerImages
        {
            get { return showPlayerImages; }
            set { SetProperty(ref showPlayerImages, value); }
        }

        private bool showGender;
        public bool ShowGender
        {
            get { return showGender; }
            set { SetProperty(ref showGender, value); }
        }

        private bool enforcePlayerImages;
        public bool EnforcePlayerImages
        {
            get { return enforcePlayerImages; }
            set { SetProperty(ref enforcePlayerImages, value); }
        }

        private bool showRealNames = true;
        public bool ShowRealNames
        {
            get { return showRealNames; }
            set { SetProperty(ref showRealNames, value); }
        }

        private bool showUsernames;
        public bool ShowUsernames
        {
            get { return showUsernames; }
            set { SetProperty(ref showUsernames, value); }
        }

        private bool showLivingPlayerCount;
        public bool ShowLivingPlayerCount
        {
            get { return showLivingPlayerCount; }
            set { SetProperty(ref showLivingPlayerCount, value); }
        }

        private bool showLivingPlayerNames;
        public bool ShowLivingPlayerNames
        {
            get { return showLivingPlayerNames; }
            set { SetProperty(ref showLivingPlayerNames, value); }
        }

        private bool showLivingPlayerNamesToDeath;
        public bool ShowLivingPlayerNamesToDeath
        {
            get { return showLivingPlayerNamesToDeath; }
            set { SetProperty(ref showLivingPlayerNamesToDeath, value); }
        }

        // Game mode settings
        private bool isAssassin;
        public bool IsAssassin
        {
            get { return isAssassin; }
            set { SetProperty(ref isAssassin, value); }
        }

        private bool showHunter;
        public bool ShowHunter
        {
            get { return showHunter; }
            set { SetProperty(ref showHunter, value); }
        }

        private bool isChaos;
        public bool IsChaos
        {
            get { return isChaos; }
            set { SetProperty(ref isChaos, value); }
        }

        private int chaosTimerMinHours;
        public int ChaosTimerMinHours
        {
            get { return chaosTimerMinHours; }
            set { SetProperty(ref chaosTimerMinHours, value); }
        }

        private int chaosTimerMaxHours;
        public int ChaosTimerMaxHours
        {
            get { return chaosTimerMaxHours; }
            set { SetProperty(ref chaosTimerMaxHours, value); }
        }

        private bool isTimed;
        public bool IsTimed
        {
            get { return isTimed; }
            set { SetProperty(ref isTimed, value); }
        }

        private int targetTimeOutHours;
        public int TargetTimeOutHours
        {
            get { return targetTimeOutHours; }
            set { SetProperty(ref targetTimeOutHours, value); }
        }

        private bool customKillMethods;
        public bool CustomKillMethods
        {
            get { return customKillMethods; }
            set { SetProperty(ref customKillMethods, value); }
        }

        private string killMethods = string.Empty;
        public string KillMethods
        {
            get { return killMethods; }
            set { SetProperty(ref killMethods, value); }
        }

        private string inviteLink = "https://gotcha.app/join/abc123";
        public string InviteLink
        {
            get { return inviteLink; }
            set { SetProperty(ref inviteLink, value); }
        }

        // VIP unlock flags (mock data)
        private bool assassinModeUnlocked = true;
        public bool AssassinModeUnlocked
        {
            get { return assassinModeUnlocked; }
            set { SetProperty(ref assassinModeUnlocked, value); }
        }

        private bool chaosModeUnlocked;
        public bool ChaosModeUnlocked
        {
            get { return chaosModeUnlocked; }
            set { SetProperty(ref chaosModeUnlocked, value); }
        }

        private bool timedKillsUnlocked = true;
        public bool TimedKillsUnlocked
        {
            get { return timedKillsUnlocked; }
            set { SetProperty(ref timedKillsUnlocked, value); }
        }

        public ICommand CreateGameCommand { get; }
        public ICommand CopyLinkCommand { get; }

        public NewGameViewModel()
        {
            CreateGameCommand = new Command(ExecuteCreateGameCommand);
            CopyLinkCommand = new Command(ExecuteCopyLinkCommand);
        }

        private async void ExecuteCreateGameCommand()
        {
            try
            {
                ErrorMessage = string.Empty;

                if (string.IsNullOrWhiteSpace(GameName))
                {
                    ErrorMessage = "Please enter a game name.";
                    return;
                }

                IsBusy = true;

                try
                {
                    await Shell.Current.DisplayAlertAsync(
                        "Not yet implemented",
                        "Game creation is not yet available.",
                        "OK");
                }
                finally
                {
                    IsBusy = false;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        private async void ExecuteCopyLinkCommand()
        {
            try
            {
                await Clipboard.Default.SetTextAsync(InviteLink);

                await Shell.Current.DisplayAlertAsync(
                    "Copied",
                    "Invite link copied to clipboard.",
                    "OK");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}

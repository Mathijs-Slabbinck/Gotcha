using CommunityToolkit.Mvvm.ComponentModel;
using Gotcha.Maui.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Gotcha.Maui.ViewModels
{
    public class PlayerAdminViewModel : ObservableObject
    {
        // Game info
        private bool hasStarted;
        public bool HasStarted
        {
            get { return hasStarted; }
            set
            {
                SetProperty(ref hasStarted, value);
                OnPropertyChanged(nameof(IsNotStarted));
            }
        }

        public bool IsNotStarted
        {
            get { return !HasStarted; }
        }

        private string gameName = string.Empty;
        public string GameName
        {
            get { return gameName; }
            set { SetProperty(ref gameName, value); }
        }

        private string inviteLink = string.Empty;
        public string InviteLink
        {
            get { return inviteLink; }
            set { SetProperty(ref inviteLink, value); }
        }

        private int playerCount = 8;
        public int PlayerCount
        {
            get { return playerCount; }
            set { SetProperty(ref playerCount, value); }
        }

        private int maxPlayers = 50;
        public int MaxPlayers
        {
            get { return maxPlayers; }
            set { SetProperty(ref maxPlayers, value); }
        }

        // Collections
        private ObservableCollection<AdminPlayerItem> players = new ObservableCollection<AdminPlayerItem>();
        public ObservableCollection<AdminPlayerItem> Players
        {
            get { return players; }
            set { SetProperty(ref players, value); }
        }

        private ObservableCollection<AdminKillItem> pendingKills = new ObservableCollection<AdminKillItem>();
        public ObservableCollection<AdminKillItem> PendingKills
        {
            get { return pendingKills; }
            set { SetProperty(ref pendingKills, value); }
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

        private bool isTimed;
        public bool IsTimed
        {
            get { return isTimed; }
            set { SetProperty(ref isTimed, value); }
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

        private int targetTimeOutHours;
        public int TargetTimeOutHours
        {
            get { return targetTimeOutHours; }
            set { SetProperty(ref targetTimeOutHours, value); }
        }

        private string customRulesText = string.Empty;
        public string CustomRulesText
        {
            get { return customRulesText; }
            set { SetProperty(ref customRulesText, value); }
        }

        // VIP unlock flags
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

        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        private string errorMessage = string.Empty;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { SetProperty(ref errorMessage, value); }
        }

        // Commands
        public ICommand PlayerActionCommand { get; }
        public ICommand CopyLinkCommand { get; }
        public ICommand CancelKillCommand { get; }
        public ICommand SaveSettingsCommand { get; }
        public ICommand StartGameCommand { get; }
        public ICommand EndGameCommand { get; }

        public PlayerAdminViewModel()
        {
            PlayerActionCommand = new Command<string>(ExecutePlayerActionCommand);
            CopyLinkCommand = new Command(ExecuteCopyLinkCommand);
            CancelKillCommand = new Command<Guid>(ExecuteCancelKillCommand);
            SaveSettingsCommand = new Command(ExecuteSaveSettingsCommand);
            StartGameCommand = new Command(ExecuteStartGameCommand);
            EndGameCommand = new Command(ExecuteEndGameCommand);

            LoadMockData();
        }

        private void LoadMockData()
        {
            GameName = "Friday Night Gotcha";
            InviteLink = "https://gotcha.app/join/abc123";

            Players.Add(new AdminPlayerItem
            {
                PlayerId = Guid.NewGuid(),
                Name = "Player Alpha",
                Username = "AlphaWolf",
                HasImage = true,
                IsAdmin = true,
                IsSpectator = false
            });
            Players.Add(new AdminPlayerItem
            {
                PlayerId = Guid.NewGuid(),
                Name = "Player Bravo",
                Username = "BravoFox",
                HasImage = false,
                IsAdmin = false,
                IsSpectator = false
            });
            Players.Add(new AdminPlayerItem
            {
                PlayerId = Guid.NewGuid(),
                Name = "Player Charlie",
                Username = "CharlieGhost",
                HasImage = true,
                IsAdmin = false,
                IsSpectator = true
            });

            PendingKills.Add(new AdminKillItem
            {
                KillId = Guid.NewGuid(),
                KillerName = "AlphaWolf",
                VictimName = "BravoFox",
                Weapon = "Water Gun",
                Moment = new DateTime(2026, 3, 10, 15, 30, 0)
            });
        }

        private async void ExecutePlayerActionCommand(string action)
        {
            try
            {
                await Shell.Current.DisplayAlertAsync(
                    "Player Action",
                    $"Action: {action}",
                    "OK");
            }
            catch
            {
                ErrorMessage = "Something went wrong. Please try again.";
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
            catch
            {
                ErrorMessage = "Something went wrong. Please try again.";
            }
        }

        private async void ExecuteCancelKillCommand(Guid killId)
        {
            try
            {
                await Shell.Current.DisplayAlertAsync(
                    "Not yet implemented",
                    "Kill cancellation is not yet available.",
                    "OK");
            }
            catch
            {
                ErrorMessage = "Something went wrong. Please try again.";
            }
        }

        private async void ExecuteSaveSettingsCommand()
        {
            try
            {
                await Shell.Current.DisplayAlertAsync(
                    "Not yet implemented",
                    "Saving game settings is not yet available.",
                    "OK");
            }
            catch
            {
                ErrorMessage = "Something went wrong. Please try again.";
            }
        }

        private async void ExecuteStartGameCommand()
        {
            try
            {
                await Shell.Current.DisplayAlertAsync(
                    "Not yet implemented",
                    "Starting the game is not yet available.",
                    "OK");
            }
            catch
            {
                ErrorMessage = "Something went wrong. Please try again.";
            }
        }

        private async void ExecuteEndGameCommand()
        {
            try
            {
                await Shell.Current.DisplayAlertAsync(
                    "Not yet implemented",
                    "Ending the game is not yet available.",
                    "OK");
            }
            catch
            {
                ErrorMessage = "Something went wrong. Please try again.";
            }
        }
    }
}

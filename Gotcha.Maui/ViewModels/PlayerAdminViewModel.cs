using Gotcha.Maui.Enums;
using Gotcha.Maui.Models.Items;
using Gotcha.Maui.Models.Payloads;
using Gotcha.Maui.Services;
using Gotcha.Maui.ViewModels.BaseViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Gotcha.Maui.ViewModels
{
    public class PlayerAdminViewModel : PageBaseViewModel
    {
        private readonly IPlayerService _playerService;
        private readonly IGameService _gameService;
        private readonly SessionService _sessionService;

        private Guid gameId;

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

        private int playerCount;
        public int PlayerCount
        {
            get { return playerCount; }
            set { SetProperty(ref playerCount, value); }
        }

        private int maxPlayers;
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

        private bool showRealNames;
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
        private bool assassinModeUnlocked;
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

        private bool timedKillsUnlocked;
        public bool TimedKillsUnlocked
        {
            get { return timedKillsUnlocked; }
            set { SetProperty(ref timedKillsUnlocked, value); }
        }

        // Commands
        public ICommand PlayerActionCommand { get; }
        public ICommand CopyLinkCommand { get; }
        public ICommand CancelKillCommand { get; }
        public ICommand SaveSettingsCommand { get; }
        public ICommand StartGameCommand { get; }
        public ICommand EndGameCommand { get; }

        public PlayerAdminViewModel(IPlayerService playerService, IGameService gameService, SessionService sessionService)
        {
            _playerService = playerService;
            _gameService = gameService;
            _sessionService = sessionService;

            PlayerActionCommand = new Command<AdminPlayerItem>(ExecutePlayerActionCommand);
            CopyLinkCommand = new Command(ExecuteCopyLinkCommand);
            CancelKillCommand = new Command<Guid>(ExecuteCancelKillCommand);
            SaveSettingsCommand = new Command(ExecuteSaveSettingsCommand);
            StartGameCommand = new Command(ExecuteStartGameCommand);
            EndGameCommand = new Command(ExecuteEndGameCommand);
        }

        public async void LoadData()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;

                var (data, error) = await _playerService.GetAdminDataAsync(_sessionService.CurrentPlayerId);

                if (data == null)
                {
                    ErrorMessage = error ?? "Something went wrong loading admin data.";
                    IsBusy = false;
                    return;
                }

                gameId = data.GameId;
                HasStarted = data.HasStarted;
                GameName = data.GameName;
                InviteLink = data.InviteLink;
                PlayerCount = data.PlayerCount;
                MaxPlayers = data.MaxPlayers;
                ShowPlayerImages = data.ShowPlayerImages;
                ShowGender = data.ShowGender;
                EnforcePlayerImages = data.EnforcePlayerImages;
                ShowRealNames = data.ShowRealNames;
                ShowUsernames = data.ShowUsernames;
                ShowLivingPlayerCount = data.ShowLivingPlayerCount;
                ShowLivingPlayerNames = data.ShowLivingPlayerNames;
                ShowLivingPlayerNamesToDeath = data.ShowLivingPlayerNamesToDeath;
                IsAssassin = data.IsAssassin;
                ShowHunter = data.ShowHunter;
                IsChaos = data.IsChaos;
                IsTimed = data.IsTimed;
                CustomKillMethods = data.CustomKillMethods;
                KillMethods = data.KillMethods;
                ChaosTimerMinHours = data.ChaosTimerMinHours;
                ChaosTimerMaxHours = data.ChaosTimerMaxHours;
                TargetTimeOutHours = data.TargetTimeOutHours;
                CustomRulesText = data.CustomRulesText;
                AssassinModeUnlocked = data.AssassinModeUnlocked;
                ChaosModeUnlocked = data.ChaosModeUnlocked;
                TimedKillsUnlocked = data.TimedKillsUnlocked;

                Players.Clear();
                foreach (var player in data.Players)
                {
                    Players.Add(player);
                }

                PendingKills.Clear();
                foreach (var kill in data.PendingKills)
                {
                    PendingKills.Add(kill);
                }
            }
            catch
            {
                ErrorMessage = "Something went wrong loading admin data.";
            }

            IsBusy = false;
        }

        private async void ExecutePlayerActionCommand(AdminPlayerItem player)
        {
            if (player == null)
            {
                return;
            }

            try
            {
                string kickLabel = "Kick player";
                string adminLabel = player.IsAdmin ? "Remove admin" : "Make admin";
                string spectatorLabel = player.IsSpectator ? "Remove spectator" : "Make spectator";

                Dictionary<string, AdminPlayerCommandActions> labelToAction = new Dictionary<string, AdminPlayerCommandActions>
                {
                    { kickLabel, AdminPlayerCommandActions.Kick },
                    { adminLabel, AdminPlayerCommandActions.ToggleAdmin },
                    { spectatorLabel, AdminPlayerCommandActions.ToggleSpectator }
                };

                string cancelLabel = "Cancel";
                string choice = await Shell.Current.DisplayActionSheetAsync(
                    $"Manage {player.Name}",
                    cancelLabel,
                    null,
                    labelToAction.Keys.ToArray());

                if (string.IsNullOrEmpty(choice) || choice == cancelLabel || !labelToAction.TryGetValue(choice, out AdminPlayerCommandActions action))
                {
                    return;
                }

                // For toggles, flip the current state; for kick, NewValue is unused.
                bool newValue = false;
                if (action == AdminPlayerCommandActions.ToggleAdmin)
                {
                    newValue = !player.IsAdmin;
                }
                else if (action == AdminPlayerCommandActions.ToggleSpectator)
                {
                    newValue = !player.IsSpectator;
                }

                await RunAdminActionAsync(
                    () => _playerService.PerformPlayerActionAsync(new PlayerActionCommand
                    {
                        PlayerId = player.PlayerId,
                        Action = action,
                        NewValue = newValue
                    }),
                    "Done",
                    $"{player.Name}: {choice.ToLower()}.");
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
            await RunAdminActionAsync(
                () => _playerService.CancelKillAsync(killId, _sessionService.CurrentPlayerId),
                "Kill cancelled",
                "The pending kill has been rejected.");
        }

        private async void ExecuteSaveSettingsCommand()
        {
            var payload = new UpdateGameSettingsCommand
            {
                GameId = gameId,
                AdminPlayerId = _sessionService.CurrentPlayerId,
                GameName = GameName,
                ShowPlayerImages = ShowPlayerImages,
                ShowGender = ShowGender,
                EnforcePlayerImages = EnforcePlayerImages,
                ShowRealNames = ShowRealNames,
                ShowUsernames = ShowUsernames,
                ShowLivingPlayerCount = ShowLivingPlayerCount,
                ShowLivingPlayerNames = ShowLivingPlayerNames,
                ShowLivingPlayerNamesToDeath = ShowLivingPlayerNamesToDeath,
                IsAssassin = IsAssassin,
                ShowHunter = ShowHunter,
                IsChaos = IsChaos,
                IsTimed = IsTimed,
                CustomKillMethods = CustomKillMethods,
                KillMethods = KillMethods,
                ChaosTimerMinHours = ChaosTimerMinHours,
                ChaosTimerMaxHours = ChaosTimerMaxHours,
                TargetTimeOutHours = TargetTimeOutHours,
                CustomRulesText = CustomRulesText
            };

            await RunAdminActionAsync(
                () => _gameService.UpdateGameSettingsAsync(payload),
                "Settings saved",
                "Game settings have been updated.");
        }

        private async void ExecuteStartGameCommand()
        {
            await RunAdminActionAsync(
                () => _gameService.StartGameAsync(gameId, _sessionService.CurrentPlayerId),
                "Game started",
                "The hunt is on!");
        }

        private async void ExecuteEndGameCommand()
        {
            await RunAdminActionAsync(
                () => _gameService.EndGameAsync(gameId, _sessionService.CurrentPlayerId),
                "Game ended",
                "The game has been ended.");
        }

        private async Task RunAdminActionAsync(Func<Task<(bool Success, string? ErrorMessage)>> serviceCall, string successTitle, string successMessage)
        {
            try
            {
                ErrorMessage = string.Empty;
                IsBusy = true;

                (bool success, string? error) = await serviceCall();

                if (success)
                {
                    await Shell.Current.DisplayAlertAsync(successTitle, successMessage, "OK");
                    await LoadDataAsync();
                }
                else
                {
                    ErrorMessage = error ?? "Something went wrong. Please try again.";
                }
            }
            catch
            {
                ErrorMessage = "Something went wrong. Please try again.";
            }

            IsBusy = false;
        }
    }
}

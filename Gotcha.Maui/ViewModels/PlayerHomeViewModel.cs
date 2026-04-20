using Gotcha.Maui.Models.Items;
using Gotcha.Maui.Services;
using Gotcha.Maui.ViewModels.BaseViewModels;
using System.Collections.ObjectModel;

namespace Gotcha.Maui.ViewModels
{
    public class PlayerHomeViewModel : PageBaseViewModel
    {
        private readonly IPlayerService _playerService;
        private readonly SessionService _sessionService;

        // Status
        private bool isAlive;
        public bool IsAlive
        {
            get { return isAlive; }
            set
            {
                SetProperty(ref isAlive, value);
                OnPropertyChanged(nameof(ShowTargetCard));
            }
        }

        private bool isSpectator;
        public bool IsSpectator
        {
            get { return isSpectator; }
            set
            {
                SetProperty(ref isSpectator, value);
                OnPropertyChanged(nameof(ShowTargetCard));
            }
        }

        // Target info
        private string targetName = string.Empty;
        public string TargetName
        {
            get { return targetName; }
            set { SetProperty(ref targetName, value); }
        }

        private string targetUsername = string.Empty;
        public string TargetUsername
        {
            get { return targetUsername; }
            set { SetProperty(ref targetUsername, value); }
        }

        private string weapon = string.Empty;
        public string Weapon
        {
            get { return weapon; }
            set { SetProperty(ref weapon, value); }
        }

        private DateTime? assignmentExpirationDate;
        public DateTime? AssignmentExpirationDate
        {
            get { return assignmentExpirationDate; }
            set { SetProperty(ref assignmentExpirationDate, value); }
        }

        // Game rules
        private bool isAssassin;
        public bool IsAssassin
        {
            get { return isAssassin; }
            set
            {
                SetProperty(ref isAssassin, value);
                OnPropertyChanged(nameof(ShowHunterCard));
            }
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

        private ObservableCollection<string> customRules = new ObservableCollection<string>();
        public ObservableCollection<string> CustomRules
        {
            get { return customRules; }
            set { SetProperty(ref customRules, value); }
        }

        // Hunter info
        private bool showHunter;
        public bool ShowHunter
        {
            get { return showHunter; }
            set
            {
                SetProperty(ref showHunter, value);
                OnPropertyChanged(nameof(ShowHunterCard));
            }
        }

        private string hunterName = string.Empty;
        public string HunterName
        {
            get { return hunterName; }
            set { SetProperty(ref hunterName, value); }
        }

        private string hunterOtherName = string.Empty;
        public string HunterOtherName
        {
            get { return hunterOtherName; }
            set { SetProperty(ref hunterOtherName, value); }
        }

        // Game stats
        private DateTime? startDate;
        public DateTime? StartDate
        {
            get { return startDate; }
            set { SetProperty(ref startDate, value); }
        }

        private DateTime? endDate;
        public DateTime? EndDate
        {
            get { return endDate; }
            set { SetProperty(ref endDate, value); }
        }

        private string winnerName = string.Empty;
        public string WinnerName
        {
            get { return winnerName; }
            set { SetProperty(ref winnerName, value); }
        }

        private string winnerOtherName = string.Empty;
        public string WinnerOtherName
        {
            get { return winnerOtherName; }
            set { SetProperty(ref winnerOtherName, value); }
        }

        private DateTime? killedOnDate;
        public DateTime? KilledOnDate
        {
            get { return killedOnDate; }
            set { SetProperty(ref killedOnDate, value); }
        }

        private string killerName = string.Empty;
        public string KillerName
        {
            get { return killerName; }
            set { SetProperty(ref killerName, value); }
        }

        private string killerOtherName = string.Empty;
        public string KillerOtherName
        {
            get { return killerOtherName; }
            set { SetProperty(ref killerOtherName, value); }
        }

        // Visibility settings
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

        // Collections
        private ObservableCollection<KillItem> kills = new ObservableCollection<KillItem>();
        public ObservableCollection<KillItem> Kills
        {
            get { return kills; }
            set
            {
                SetProperty(ref kills, value);
                OnPropertyChanged(nameof(HasKills));
                OnPropertyChanged(nameof(KillCount));
            }
        }

        private ObservableCollection<PlayerItem> players = new ObservableCollection<PlayerItem>();
        public ObservableCollection<PlayerItem> Players
        {
            get { return players; }
            set
            {
                SetProperty(ref players, value);
                OnPropertyChanged(nameof(PlayerCount));
                OnPropertyChanged(nameof(LivingPlayerCount));
            }
        }

        // Computed properties
        public bool ShowTargetCard
        {
            get { return IsAlive && !IsSpectator; }
        }

        public bool ShowHunterCard
        {
            get { return IsAssassin && ShowHunter; }
        }

        public bool HasKills
        {
            get { return Kills.Count > 0; }
        }

        public int KillCount
        {
            get { return Kills.Count; }
        }

        public int PlayerCount
        {
            get { return Players.Count; }
        }

        public int LivingPlayerCount
        {
            get { return Players.Count(p => p.IsAlive); }
        }

        public PlayerHomeViewModel(IPlayerService playerService, SessionService sessionService)
        {
            _playerService = playerService;
            _sessionService = sessionService;
        }

        public async void LoadData()
        {
            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;
                var (data, error) = await _playerService.GetPlayerHomeDataAsync(_sessionService.CurrentPlayerId);

                if (data == null)
                {
                    ErrorMessage = error ?? "Something went wrong loading game data.";
                    IsBusy = false;
                    return;
                }

                IsAlive = data.IsAlive;
                IsSpectator = data.IsSpectator;
                TargetName = data.TargetName;
                TargetUsername = data.TargetUsername;
                Weapon = data.Weapon;
                AssignmentExpirationDate = data.AssignmentExpirationDate;
                IsAssassin = data.IsAssassin;
                IsChaos = data.IsChaos;
                IsTimed = data.IsTimed;
                ChaosTimerMinHours = data.ChaosTimerMinHours;
                ChaosTimerMaxHours = data.ChaosTimerMaxHours;
                TargetTimeOutHours = data.TargetTimeOutHours;
                ShowHunter = data.ShowHunter;
                HunterName = data.HunterName;
                HunterOtherName = data.HunterOtherName;
                StartDate = data.StartDate;
                EndDate = data.EndDate;
                WinnerName = data.WinnerName;
                WinnerOtherName = data.WinnerOtherName;
                KilledOnDate = data.KilledOnDate;
                KillerName = data.KillerName;
                KillerOtherName = data.KillerOtherName;
                ShowLivingPlayerCount = data.ShowLivingPlayerCount;
                ShowLivingPlayerNames = data.ShowLivingPlayerNames;

                CustomRules.Clear();
                foreach (var rule in data.CustomRules)
                {
                    CustomRules.Add(rule);
                }

                Kills.Clear();
                foreach (var kill in data.Kills)
                {
                    Kills.Add(kill);
                }

                Players.Clear();
                foreach (var player in data.Players)
                {
                    Players.Add(player);
                }

                OnPropertyChanged(nameof(HasKills));
                OnPropertyChanged(nameof(KillCount));
                OnPropertyChanged(nameof(PlayerCount));
                OnPropertyChanged(nameof(LivingPlayerCount));
            }
            catch
            {
                ErrorMessage = "Something went wrong loading game data.";
            }

            IsBusy = false;
        }
    }
}

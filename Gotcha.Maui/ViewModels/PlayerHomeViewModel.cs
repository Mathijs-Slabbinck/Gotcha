using CommunityToolkit.Mvvm.ComponentModel;
using Gotcha.Maui.Models;
using System.Collections.ObjectModel;

namespace Gotcha.Maui.ViewModels
{
    public class PlayerHomeViewModel : ObservableObject
    {
        // Status
        private bool isAlive = true;
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
        private bool isAssassin = true;
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

        private bool isTimed = true;
        public bool IsTimed
        {
            get { return isTimed; }
            set { SetProperty(ref isTimed, value); }
        }

        private int chaosTimerMinHours = 2;
        public int ChaosTimerMinHours
        {
            get { return chaosTimerMinHours; }
            set { SetProperty(ref chaosTimerMinHours, value); }
        }

        private int chaosTimerMaxHours = 6;
        public int ChaosTimerMaxHours
        {
            get { return chaosTimerMaxHours; }
            set { SetProperty(ref chaosTimerMaxHours, value); }
        }

        private int targetTimeOutHours = 48;
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
        private bool showHunter = true;
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
        private bool showLivingPlayerCount = true;
        public bool ShowLivingPlayerCount
        {
            get { return showLivingPlayerCount; }
            set { SetProperty(ref showLivingPlayerCount, value); }
        }

        private bool showLivingPlayerNames = true;
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

        public PlayerHomeViewModel()
        {
            LoadMockData();
        }

        private void LoadMockData()
        {
            TargetName = "Player Bravo";
            TargetUsername = "BravoFox";
            Weapon = "Water Gun";
            AssignmentExpirationDate = new DateTime(2026, 3, 20, 18, 0, 0);

            HunterName = "Player Charlie";
            HunterOtherName = "CharlieGhost";

            StartDate = new DateTime(2026, 3, 1);

            CustomRules.Add("No kills inside classrooms");
            CustomRules.Add("Safe zones: library and cafeteria");

            Kills.Add(new KillItem
            {
                VictimName = "Player Delta",
                VictimUsername = "DeltaHawk",
                Weapon = "Water Gun",
                TimeStamp = new DateTime(2026, 3, 5, 14, 30, 0)
            });
            Kills.Add(new KillItem
            {
                VictimName = "Player Echo",
                VictimUsername = "EchoRanger",
                Weapon = "Nerf Dart",
                TimeStamp = new DateTime(2026, 3, 8, 9, 15, 0)
            });

            Players.Add(new PlayerItem { Name = "Player Alpha", OtherName = "AlphaWolf", IsAlive = true });
            Players.Add(new PlayerItem { Name = "Player Bravo", OtherName = "BravoFox", IsAlive = true });
            Players.Add(new PlayerItem { Name = "Player Charlie", OtherName = "CharlieGhost", IsAlive = true });
            Players.Add(new PlayerItem { Name = "Player Delta", OtherName = "DeltaHawk", IsAlive = false });

            OnPropertyChanged(nameof(HasKills));
            OnPropertyChanged(nameof(KillCount));
            OnPropertyChanged(nameof(PlayerCount));
            OnPropertyChanged(nameof(LivingPlayerCount));
        }
    }
}

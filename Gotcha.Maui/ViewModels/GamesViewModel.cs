using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Gotcha.Maui.ViewModels
{
    public class GamesViewModel : ObservableObject
    {
        public class GameItem
        {
            public Guid GameId { get; set; }
            public string Name { get; set; } = string.Empty;
            public string CreatedDate { get; set; } = string.Empty;
            public string StartDate { get; set; } = string.Empty;
            public string EndDate { get; set; } = string.Empty;
            public string WinnerName { get; set; } = string.Empty;
            public int PlayerCount { get; set; }
            public bool IsAlive { get; set; }
        }

        private ObservableCollection<GameItem> pendingGames = new ObservableCollection<GameItem>();
        public ObservableCollection<GameItem> PendingGames
        {
            get { return pendingGames; }
            set { SetProperty(ref pendingGames, value); }
        }

        private ObservableCollection<GameItem> activeGames = new ObservableCollection<GameItem>();
        public ObservableCollection<GameItem> ActiveGames
        {
            get { return activeGames; }
            set { SetProperty(ref activeGames, value); }
        }

        private ObservableCollection<GameItem> endedGames = new ObservableCollection<GameItem>();
        public ObservableCollection<GameItem> EndedGames
        {
            get { return endedGames; }
            set { SetProperty(ref endedGames, value); }
        }

        private string errorMessage = string.Empty;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { SetProperty(ref errorMessage, value); }
        }

        public ICommand GameTappedCommand { get; }
        public ICommand NewGameCommand { get; }

        public GamesViewModel()
        {
            GameTappedCommand = new Command<GameItem>(ExecuteGameTappedCommand);
            NewGameCommand = new Command(ExecuteNewGameCommand);

            LoadMockData();
        }

        private void LoadMockData()
        {
            PendingGames.Add(new GameItem
            {
                GameId = Guid.NewGuid(),
                Name = "Friday Night Gotcha",
                CreatedDate = "2025-03-10",
                PlayerCount = 8
            });
            PendingGames.Add(new GameItem
            {
                GameId = Guid.NewGuid(),
                Name = "Office Battle Royale",
                CreatedDate = "2025-03-12",
                PlayerCount = 15
            });

            ActiveGames.Add(new GameItem
            {
                GameId = Guid.NewGuid(),
                Name = "Campus Hunt",
                CreatedDate = "2025-03-01",
                StartDate = "2025-03-05",
                PlayerCount = 12,
                IsAlive = true
            });

            EndedGames.Add(new GameItem
            {
                GameId = Guid.NewGuid(),
                Name = "Summer Showdown",
                CreatedDate = "2025-01-15",
                StartDate = "2025-01-20",
                EndDate = "2025-02-10",
                WinnerName = "TheLegend27",
                PlayerCount = 20,
                IsAlive = false
            });
        }

        private async void ExecuteGameTappedCommand(GameItem game)
        {
            try
            {
                await Shell.Current.DisplayAlertAsync(
                    game.Name,
                    $"Players: {game.PlayerCount}",
                    "OK");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        private async void ExecuteNewGameCommand()
        {
            try
            {
                await Shell.Current.GoToAsync("NewGame");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}

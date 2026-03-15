using CommunityToolkit.Mvvm.ComponentModel;
using Gotcha.Maui.Models;
using Gotcha.Maui.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Gotcha.Maui.ViewModels
{
    public class GamesViewModel : ObservableObject
    {
        private readonly IGameService _gameService;

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

        public ICommand GameTappedCommand { get; }
        public ICommand NewGameCommand { get; }

        public GamesViewModel(IGameService gameService)
        {
            _gameService = gameService;

            GameTappedCommand = new Command<GameItem>(ExecuteGameTappedCommand);
            NewGameCommand = new Command(ExecuteNewGameCommand);
        }

        public async void LoadData()
        {
            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;

                PendingGames.Clear();
                var pending = await _gameService.GetPendingGamesAsync();
                foreach (var game in pending)
                {
                    PendingGames.Add(game);
                }

                ActiveGames.Clear();
                var active = await _gameService.GetActiveGamesAsync();
                foreach (var game in active)
                {
                    ActiveGames.Add(game);
                }

                EndedGames.Clear();
                var ended = await _gameService.GetEndedGamesAsync();
                foreach (var game in ended)
                {
                    EndedGames.Add(game);
                }
            }
            catch
            {
                ErrorMessage = "Something went wrong loading your games.";
            }

            IsBusy = false;
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
            catch
            {
                ErrorMessage = "Something went wrong. Please try again.";
            }
        }

        private async void ExecuteNewGameCommand()
        {
            try
            {
                await Shell.Current.GoToAsync("NewGame");
            }
            catch
            {
                ErrorMessage = "Something went wrong. Please try again.";
            }
        }
    }
}

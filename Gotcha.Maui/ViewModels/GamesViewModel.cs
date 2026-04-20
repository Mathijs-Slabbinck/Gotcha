using Gotcha.Maui.Models;
using Gotcha.Maui.Services;
using Gotcha.Maui.ViewModels.BaseViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Gotcha.Maui.ViewModels
{
    public class GamesViewModel : PageBaseViewModel
    {
        private readonly IGameService _gameService;
        private readonly SessionService _sessionService;

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

        public ICommand GameTappedCommand { get; }
        public ICommand NewGameCommand { get; }

        public GamesViewModel(IGameService gameService, SessionService sessionService)
        {
            _gameService = gameService;
            _sessionService = sessionService;

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
                _sessionService.SetPlayer(game.PlayerId);
                await Shell.Current.GoToAsync(Routes.PlayerHome);
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

using Gotcha.Maui.Services;
using Gotcha.Maui.ViewModels.BaseViewModels;
using System.Windows.Input;

namespace Gotcha.Maui.ViewModels
{
    public class ConfirmKillViewModel : PageBaseViewModel
    {
        private readonly IPlayerService _playerService;
        private readonly SessionService _sessionService;

        private string targetName = string.Empty;
        private string targetUsername = string.Empty;
        private string weapon = string.Empty;
        private string hunterName = string.Empty;
        private string hunterUsername = string.Empty;
        private bool isAssassinMode; // = false
        private bool showHunter; // = false


        public string TargetName
        {
            get { return targetName; }
            set { SetProperty(ref targetName, value); }
        }

        public string TargetUsername
        {
            get { return targetUsername; }
            set { SetProperty(ref targetUsername, value); }
        }

        public string Weapon
        {
            get { return weapon; }
            set { SetProperty(ref weapon, value); }
        }

        public string HunterName
        {
            get { return hunterName; }
            set { SetProperty(ref hunterName, value); }
        }

        public string HunterUsername
        {
            get { return hunterUsername; }
            set { SetProperty(ref hunterUsername, value); }
        }

        public bool IsAssassinMode
        {
            get { return isAssassinMode; }
            set { SetProperty(ref isAssassinMode, value); }
        }

        public bool ShowHunter
        {
            get { return showHunter; }
            set { SetProperty(ref showHunter, value); }
        }

        public ICommand ConfirmKillCommand { get; }
        public ICommand ConfirmDeathCommand { get; }
        public ICommand ConfirmHunterKillCommand { get; }

        public ConfirmKillViewModel(IPlayerService playerService, SessionService sessionService)
        {
            _playerService = playerService;
            _sessionService = sessionService;

            ConfirmKillCommand = new Command(ExecuteConfirmKillCommand);
            ConfirmDeathCommand = new Command(ExecuteConfirmDeathCommand);
            ConfirmHunterKillCommand = new Command(ExecuteConfirmHunterKillCommand);
        }

        // LoadData is split into a public `async void` wrapper and a private `async Task` body.
        // The wrapper exists to satisfy the project convention (the page's OnAppearing calls
        // `_viewModel.LoadData();` without `await`). The private `LoadDataAsync` is so
        // `RunConfirmAsync` can `await` the reload after a successful confirm — keeping
        // IsBusy=true through the refresh and preventing the user from tapping a Confirm
        // button mid-reload. See Notes_To_Self.md Note 19 for the full reasoning.
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

                var (data, error) = await _playerService.GetConfirmKillDataAsync(_sessionService.CurrentPlayerId);

                if (data == null)
                {
                    ErrorMessage = error ?? "Something went wrong loading kill data.";
                    IsBusy = false;
                    return;
                }

                TargetName = data.TargetName;
                TargetUsername = data.TargetUsername;
                Weapon = data.Weapon;
                HunterName = data.HunterName;
                HunterUsername = data.HunterUsername;
                IsAssassinMode = data.IsAssassinMode;
                ShowHunter = data.ShowHunter;
            }
            catch
            {
                ErrorMessage = "Something went wrong loading kill data.";
            }

            IsBusy = false;
        }

        private async void ExecuteConfirmKillCommand()
        {
            await RunConfirmAsync(
                () => _playerService.ConfirmKillAsync(_sessionService.CurrentPlayerId),
                "Kill confirmed!",
                "Nice shot. Your next target is on the way.");
        }

        private async void ExecuteConfirmDeathCommand()
        {
            await RunConfirmAsync(
                () => _playerService.ConfirmDeathAsync(_sessionService.CurrentPlayerId),
                "Death confirmed",
                "You're out of the game. Good luck next time!");
        }

        private async void ExecuteConfirmHunterKillCommand()
        {
            await RunConfirmAsync(
                () => _playerService.ConfirmHunterKillAsync(_sessionService.CurrentPlayerId),
                "Hunter kill confirmed",
                "Your hunter got you. You're out of the game.");
        }

        private async Task RunConfirmAsync(Func<Task<(bool Success, string? ErrorMessage)>> serviceCall, string successTitle, string successMessage)
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

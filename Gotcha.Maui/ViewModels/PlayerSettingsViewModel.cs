using Gotcha.Maui.Services;
using Gotcha.Maui.ViewModels.BaseViewModels;
using System.Windows.Input;

namespace Gotcha.Maui.ViewModels
{
    public class PlayerSettingsViewModel : PageBaseViewModel
    {
        private readonly IPlayerService _playerService;
        private readonly SessionService _sessionService;

        private string username = string.Empty;
        public string Username
        {
            get { return username; }
            set { SetProperty(ref username, value); }
        }

        private string profileImageSource = string.Empty;
        public string ProfileImageSource
        {
            get { return profileImageSource; }
            set { SetProperty(ref profileImageSource, value); }
        }

        private string successMessage = string.Empty;
        public string SuccessMessage
        {
            get { return successMessage; }
            set { SetProperty(ref successMessage, value); }
        }

        public ICommand SaveChangesCommand { get; }
        public ICommand LogoutCommand { get; }

        public PlayerSettingsViewModel(IPlayerService playerService, SessionService sessionService)
        {
            _playerService = playerService;
            _sessionService = sessionService;

            SaveChangesCommand = new Command(ExecuteSaveChangesCommand);
            LogoutCommand = new Command(ExecuteLogoutCommand);
        }

        public async void LoadData()
        {
            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;

                (string? fetchedUsername, string? error) = await _playerService.GetPlayerUsernameAsync(_sessionService.CurrentPlayerId);

                if (fetchedUsername == null)
                {
                    ErrorMessage = error ?? "Something went wrong loading your profile.";
                    IsBusy = false;
                    return;
                }

                Username = fetchedUsername;
            }
            catch
            {
                ErrorMessage = "Something went wrong loading your profile.";
            }

            IsBusy = false;
        }

        private async void ExecuteSaveChangesCommand()
        {
            try
            {
                ErrorMessage = string.Empty;
                SuccessMessage = string.Empty;

                if (string.IsNullOrWhiteSpace(Username))
                {
                    ErrorMessage = "Username is required.";
                    return;
                }

                IsBusy = true;

                (bool success, string? error) = await _playerService.UpdatePlayerUsernameAsync(_sessionService.CurrentPlayerId, Username);

                if (success)
                {
                    SuccessMessage = "Your changes have been saved.";
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

        private async void ExecuteLogoutCommand()
        {
            try
            {
                await _sessionService.SignOutAsync();
            }
            catch
            {
                ErrorMessage = "Something went wrong. Please try again.";
            }
        }
    }
}

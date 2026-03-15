using CommunityToolkit.Mvvm.ComponentModel;
using Gotcha.Maui.Services;
using System.Windows.Input;

namespace Gotcha.Maui.ViewModels
{
    public class PlayerSettingsViewModel : ObservableObject
    {
        private readonly IPlayerService playerService;

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

        private string errorMessage = string.Empty;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { SetProperty(ref errorMessage, value); }
        }

        private string successMessage = string.Empty;
        public string SuccessMessage
        {
            get { return successMessage; }
            set { SetProperty(ref successMessage, value); }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        public ICommand SaveChangesCommand { get; }
        public ICommand LogoutCommand { get; }

        public PlayerSettingsViewModel(IPlayerService playerService)
        {
            this.playerService = playerService;

            SaveChangesCommand = new Command(ExecuteSaveChangesCommand);
            LogoutCommand = new Command(ExecuteLogoutCommand);
        }

        public async void LoadData()
        {
            try
            {
                Username = await playerService.GetPlayerUsernameAsync(Guid.Empty);
            }
            catch
            {
                ErrorMessage = "Something went wrong loading your profile.";
            }
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

                await playerService.UpdatePlayerUsernameAsync(Guid.Empty, Username);
                SuccessMessage = "Your changes have been saved.";
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
                await Shell.Current.GoToAsync(Routes.SignIn);
            }
            catch
            {
                ErrorMessage = "Something went wrong. Please try again.";
            }
        }
    }
}

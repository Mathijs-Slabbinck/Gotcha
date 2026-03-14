using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace Gotcha.Maui.ViewModels
{
    public class PlayerSettingsViewModel : ObservableObject
    {
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

        public PlayerSettingsViewModel()
        {
            SaveChangesCommand = new Command(ExecuteSaveChangesCommand);
            LogoutCommand = new Command(ExecuteLogoutCommand);

            LoadMockData();
        }

        private void LoadMockData()
        {
            Username = "AlphaWolf";
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

                await Shell.Current.DisplayAlertAsync(
                    "Not yet implemented",
                    "Saving player settings is not yet available.",
                    "OK");
            }
            catch
            {
                ErrorMessage = "Something went wrong. Please try again.";
            }
            finally
            {
                IsBusy = false;
            }
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

using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace Gotcha.Maui.ViewModels
{
    public class SettingsViewModel : ObservableObject
    {
        private string firstName = "John";
        public string FirstName
        {
            get { return firstName; }
            set { SetProperty(ref firstName, value); }
        }

        private string lastName = "Doe";
        public string LastName
        {
            get { return lastName; }
            set { SetProperty(ref lastName, value); }
        }

        private string username = "TheLegend27";
        public string Username
        {
            get { return username; }
            set { SetProperty(ref username, value); }
        }

        private string email = "john.doe@example.com";
        public string Email
        {
            get { return email; }
            set { SetProperty(ref email, value); }
        }

        private DateTime birthday = new DateTime(2000, 1, 15);
        public DateTime Birthday
        {
            get { return birthday; }
            set { SetProperty(ref birthday, value); }
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
        public ICommand ResetPasswordCommand { get; }
        public ICommand UnlockFeaturesCommand { get; }

        public SettingsViewModel()
        {
            SaveChangesCommand = new Command(ExecuteSaveChangesCommand);
            ResetPasswordCommand = new Command(ExecuteResetPasswordCommand);
            UnlockFeaturesCommand = new Command(ExecuteUnlockFeaturesCommand);
        }

        private async void ExecuteSaveChangesCommand()
        {
            try
            {
                ErrorMessage = string.Empty;
                SuccessMessage = string.Empty;

                if (string.IsNullOrWhiteSpace(FirstName))
                {
                    ErrorMessage = "First name is required.";
                    return;
                }

                if (string.IsNullOrWhiteSpace(LastName))
                {
                    ErrorMessage = "Last name is required.";
                    return;
                }

                if (string.IsNullOrWhiteSpace(Username))
                {
                    ErrorMessage = "Username is required.";
                    return;
                }

                if (string.IsNullOrWhiteSpace(Email))
                {
                    ErrorMessage = "Email is required.";
                    return;
                }

                IsBusy = true;

                try
                {
                    // TODO: Save changes to the API
                    await Task.Delay(500);

                    SuccessMessage = "Your changes have been saved.";
                }
                finally
                {
                    IsBusy = false;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        private async void ExecuteResetPasswordCommand()
        {
            try
            {
                await Shell.Current.GoToAsync("ResetPassword");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        private async void ExecuteUnlockFeaturesCommand()
        {
            try
            {
                await Shell.Current.GoToAsync("//UserStore");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}

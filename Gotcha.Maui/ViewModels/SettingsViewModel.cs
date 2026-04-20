using Gotcha.Maui.Models;
using Gotcha.Maui.Services;
using Gotcha.Maui.ViewModels.BaseViewModels;
using System.Windows.Input;

namespace Gotcha.Maui.ViewModels
{
    public class SettingsViewModel : PageBaseViewModel
    {
        private readonly IUserService userService;

        private string firstName = string.Empty;
        public string FirstName
        {
            get { return firstName; }
            set { SetProperty(ref firstName, value); }
        }

        private string lastName = string.Empty;
        public string LastName
        {
            get { return lastName; }
            set { SetProperty(ref lastName, value); }
        }

        private string username = string.Empty;
        public string Username
        {
            get { return username; }
            set { SetProperty(ref username, value); }
        }

        private string email = string.Empty;
        public string Email
        {
            get { return email; }
            set { SetProperty(ref email, value); }
        }

        private DateTime birthday = DateTime.Today;
        public DateTime Birthday
        {
            get { return birthday; }
            set { SetProperty(ref birthday, value); }
        }

        private string successMessage = string.Empty;
        public string SuccessMessage
        {
            get { return successMessage; }
            set { SetProperty(ref successMessage, value); }
        }

        public ICommand SaveChangesCommand { get; }
        public ICommand ResetPasswordCommand { get; }
        public ICommand UnlockFeaturesCommand { get; }

        public SettingsViewModel(IUserService userService)
        {
            this.userService = userService;

            SaveChangesCommand = new Command(ExecuteSaveChangesCommand);
            ResetPasswordCommand = new Command(ExecuteResetPasswordCommand);
            UnlockFeaturesCommand = new Command(ExecuteUnlockFeaturesCommand);
        }

        public async void LoadData()
        {
            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;

                var profile = await userService.GetProfileAsync();

                FirstName = profile.FirstName;
                LastName = profile.LastName;
                Username = profile.Username;
                Email = profile.Email;
                Birthday = profile.Birthday;
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

                var profile = new UserProfile
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Username = Username,
                    Email = Email,
                    Birthday = Birthday
                };

                await userService.UpdateProfileAsync(profile);

                SuccessMessage = "Your changes have been saved.";
            }
            catch
            {
                ErrorMessage = "Something went wrong. Please try again.";
            }

            IsBusy = false;
        }

        private async void ExecuteResetPasswordCommand()
        {
            try
            {
                await Shell.Current.GoToAsync(Routes.ResetPassword);
            }
            catch
            {
                ErrorMessage = "Something went wrong. Please try again.";
            }
        }

        private async void ExecuteUnlockFeaturesCommand()
        {
            try
            {
                await Shell.Current.GoToAsync(Routes.UserStore);
            }
            catch
            {
                ErrorMessage = "Something went wrong. Please try again.";
            }
        }
    }
}

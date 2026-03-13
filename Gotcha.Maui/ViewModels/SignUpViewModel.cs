using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace Gotcha.Maui.ViewModels
{
    public class SignUpViewModel : ObservableObject
    {
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

        private string password = string.Empty;
        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value); }
        }

        private string repeatPassword = string.Empty;
        public string RepeatPassword
        {
            get { return repeatPassword; }
            set { SetProperty(ref repeatPassword, value); }
        }

        private DateTime birthday = DateTime.Today;
        public DateTime Birthday
        {
            get { return birthday; }
            set
            {
                if (SetProperty(ref birthday, value))
                {
                    OnPropertyChanged(nameof(IsUnder16));
                }
            }
        }

        public bool IsUnder16
        {
            get
            {
                var age = DateTime.Today.Year - Birthday.Year;
                if (Birthday > DateTime.Today.AddYears(-age))
                {
                    age--;
                }
                return age < 16;
            }
        }

        private string selectedGender = string.Empty;
        public string SelectedGender
        {
            get { return selectedGender; }
            set { SetProperty(ref selectedGender, value); }
        }

        private string guardianEmail = string.Empty;
        public string GuardianEmail
        {
            get { return guardianEmail; }
            set { SetProperty(ref guardianEmail, value); }
        }

        private bool isPasswordVisible;
        public bool IsPasswordVisible
        {
            get { return isPasswordVisible; }
            set
            {
                if (SetProperty(ref isPasswordVisible, value))
                {
                    OnPropertyChanged(nameof(PasswordEyeIconSource));
                }
            }
        }

        private bool isRepeatPasswordVisible;
        public bool IsRepeatPasswordVisible
        {
            get { return isRepeatPasswordVisible; }
            set
            {
                if (SetProperty(ref isRepeatPasswordVisible, value))
                {
                    OnPropertyChanged(nameof(RepeatPasswordEyeIconSource));
                }
            }
        }

        public string PasswordEyeIconSource
        {
            get { return IsPasswordVisible ? "eye_open.svg" : "eye_closed.svg"; }
        }

        public string RepeatPasswordEyeIconSource
        {
            get { return IsRepeatPasswordVisible ? "eye_open.svg" : "eye_closed.svg"; }
        }

        private string errorMessage = string.Empty;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { SetProperty(ref errorMessage, value); }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        public List<string> GenderOptions { get; } = new List<string> { "Male", "Female", "Other" };

        public ICommand SignUpCommand { get; }
        public ICommand TogglePasswordVisibilityCommand { get; }
        public ICommand ToggleRepeatPasswordVisibilityCommand { get; }
        public ICommand SignInCommand { get; }

        public SignUpViewModel()
        {
            SignUpCommand = new Command(ExecuteSignUpCommand);
            TogglePasswordVisibilityCommand = new Command(ExecuteTogglePasswordVisibility);
            ToggleRepeatPasswordVisibilityCommand = new Command(ExecuteToggleRepeatPasswordVisibility);
            SignInCommand = new Command(ExecuteSignInCommand);
        }

        private async void ExecuteSignUpCommand()
        {
            try
            {
                ErrorMessage = string.Empty;

                if (string.IsNullOrWhiteSpace(FirstName)
                    || string.IsNullOrWhiteSpace(LastName)
                    || string.IsNullOrWhiteSpace(Username)
                    || string.IsNullOrWhiteSpace(Email)
                    || string.IsNullOrWhiteSpace(Password)
                    || string.IsNullOrWhiteSpace(RepeatPassword)
                    || string.IsNullOrWhiteSpace(SelectedGender))
                {
                    ErrorMessage = "Please fill in all required fields.";
                    return;
                }

                if (Password != RepeatPassword)
                {
                    ErrorMessage = "Passwords do not match.";
                    return;
                }

                if (IsUnder16 && string.IsNullOrWhiteSpace(GuardianEmail))
                {
                    ErrorMessage = "Parent/guardian email is required for users under 16.";
                    return;
                }

                IsBusy = true;

                try
                {
                    await Shell.Current.DisplayAlertAsync(
                        "Not yet implemented",
                        "Sign-up functionality is not yet available.",
                        "OK");
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

        private void ExecuteTogglePasswordVisibility()
        {
            IsPasswordVisible = !IsPasswordVisible;
        }

        private void ExecuteToggleRepeatPasswordVisibility()
        {
            IsRepeatPasswordVisible = !IsRepeatPasswordVisible;
        }

        private async void ExecuteSignInCommand()
        {
            try
            {
                await Shell.Current.GoToAsync("//SignIn");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}

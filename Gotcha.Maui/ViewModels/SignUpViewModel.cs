using Gotcha.Maui.Models.Forms;
using Gotcha.Maui.Services;
using Gotcha.Maui.ViewModels.BaseViewModels;
using Gotcha.Shared.Helpers;
using System.Windows.Input;

namespace Gotcha.Maui.ViewModels
{
    public class SignUpViewModel : PageBaseViewModel
    {
        private readonly IAuthService _authService;

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
            get { return AgeHelper.IsUnder16(Birthday); }
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
            set { SetProperty(ref isPasswordVisible, value); }
        }

        private bool isRepeatPasswordVisible;
        public bool IsRepeatPasswordVisible
        {
            get { return isRepeatPasswordVisible; }
            set { SetProperty(ref isRepeatPasswordVisible, value); }
        }

        private string successMessage = string.Empty;
        public string SuccessMessage
        {
            get { return successMessage; }
            set { SetProperty(ref successMessage, value); }
        }

        public List<string> GenderOptions { get; } = new List<string> { "Male", "Female", "Other" };

        public ICommand SignUpCommand { get; }
        public ICommand TogglePasswordVisibilityCommand { get; }
        public ICommand ToggleRepeatPasswordVisibilityCommand { get; }
        public ICommand SignInCommand { get; }

        public SignUpViewModel(IAuthService authService)
        {
            _authService = authService;

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
                SuccessMessage = string.Empty;

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

                SignUpData data = new SignUpData
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Username = Username,
                    Email = Email,
                    Password = Password,
                    Gender = SelectedGender,
                    Birthday = Birthday,
                    GuardianEmail = string.IsNullOrWhiteSpace(GuardianEmail) ? null : GuardianEmail,
                };

                (bool success, string? error) = await _authService.SignUpAsync(data);

                if (success)
                {
                    ClearFormFields();
                    SuccessMessage = "Account created! Check your email to confirm your account before signing in.";
                }
                else
                {
                    ErrorMessage = error ?? "Sign-up failed. Please check your details and try again.";
                }
            }
            catch
            {
                ErrorMessage = "Something went wrong. Please try again.";
            }

            IsBusy = false;
        }

        private void ClearFormFields()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Username = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            RepeatPassword = string.Empty;
            SelectedGender = string.Empty;
            GuardianEmail = string.Empty;
            Birthday = DateTime.Today;
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
                await Shell.Current.GoToAsync(Routes.SignIn);
            }
            catch
            {
                ErrorMessage = "Something went wrong. Please try again.";
            }
        }
    }
}

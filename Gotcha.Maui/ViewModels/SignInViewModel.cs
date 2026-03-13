using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace Gotcha.Maui.ViewModels
{
    public class SignInViewModel : ObservableObject
    {
        private string usernameOrEmail = string.Empty;
        public string UsernameOrEmail
        {
            get { return usernameOrEmail; }
            set { SetProperty(ref usernameOrEmail, value); }
        }

        private string password = string.Empty;
        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value); }
        }

        private bool rememberMe;
        public bool RememberMe
        {
            get { return rememberMe; }
            set { SetProperty(ref rememberMe, value); }
        }

        private bool isPasswordVisible;
        public bool IsPasswordVisible
        {
            get { return isPasswordVisible; }
            set
            {
                if (SetProperty(ref isPasswordVisible, value))
                {
                    OnPropertyChanged(nameof(EyeIconSource));
                }
            }
        }

        public string EyeIconSource
        {
            get { return IsPasswordVisible ? "eye_open.svg" : "eye_closed.svg"; }
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

        public ICommand SignInCommand { get; }
        public ICommand TogglePasswordVisibilityCommand { get; }
        public ICommand ForgotPasswordCommand { get; }
        public ICommand SignUpCommand { get; }

        public SignInViewModel()
        {
            SignInCommand = new Command(ExecuteSignInCommand);
            TogglePasswordVisibilityCommand = new Command(ExecuteTogglePasswordVisibility);
            ForgotPasswordCommand = new Command(ExecuteForgotPasswordCommand);
            SignUpCommand = new Command(ExecuteSignUpCommand);
        }

        private async void ExecuteSignInCommand()
        {
            try
            {
                ErrorMessage = string.Empty;

                if (string.IsNullOrWhiteSpace(UsernameOrEmail) || string.IsNullOrWhiteSpace(Password))
                {
                    ErrorMessage = "Please fill in all fields.";
                    return;
                }

                IsBusy = true;

                try
                {
                    await Shell.Current.DisplayAlertAsync(
                        "Not yet implemented",
                        "Sign-in functionality is not yet available.",
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

        private async void ExecuteForgotPasswordCommand()
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

        private async void ExecuteSignUpCommand()
        {
            try
            {
                await Shell.Current.GoToAsync("//SignUp");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}

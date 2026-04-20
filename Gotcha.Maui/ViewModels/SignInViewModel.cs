using Gotcha.Maui.Services;
using Gotcha.Maui.ViewModels.BaseViewModels;
using System.Windows.Input;

namespace Gotcha.Maui.ViewModels
{
    public class SignInViewModel : PageBaseViewModel
    {
        private readonly IAuthService _authService;
        private readonly SessionService _sessionService;

        private string usernameOrEmail = string.Empty;
        private string password = string.Empty;
        private bool rememberMe; // = false
        private bool isPasswordVisible; // = false


        public string UsernameOrEmail
        {
            get { return usernameOrEmail; }
            set { SetProperty(ref usernameOrEmail, value); }
        }

        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value); }
        }

        public bool RememberMe
        {
            get { return rememberMe; }
            set { SetProperty(ref rememberMe, value); }
        }

        public bool IsPasswordVisible
        {
            get { return isPasswordVisible; }
            set { SetProperty(ref isPasswordVisible, value); }
        }

        public ICommand SignInCommand { get; }
        public ICommand TogglePasswordVisibilityCommand { get; }
        public ICommand ForgotPasswordCommand { get; }
        public ICommand SignUpCommand { get; }

        public SignInViewModel(IAuthService authService, SessionService sessionService)
        {
            _authService = authService;
            _sessionService = sessionService;

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

                (Guid? userId, string? error) = await _authService.SignInAsync(UsernameOrEmail, Password);

                if (error != null)
                {
                    ErrorMessage = error;
                }
                else if (userId != null)
                {
                    _sessionService.SetUser(userId.Value);

                    if (RememberMe)
                    {
                        await SecureStorage.SetAsync("userId", userId.Value.ToString());
                    }

                    ((AppShell)Shell.Current).SwitchToUserTabBar();
                }
            }
            catch
            {
                ErrorMessage = "Something went wrong. Please try again.";
            }

            IsBusy = false;
        }

        private void ExecuteTogglePasswordVisibility()
        {
            IsPasswordVisible = !IsPasswordVisible;
        }

        private async void ExecuteForgotPasswordCommand()
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

        private async void ExecuteSignUpCommand()
        {
            try
            {
                await Shell.Current.GoToAsync(Routes.SignUp);
            }
            catch
            {
                ErrorMessage = "Something went wrong. Please try again.";
            }
        }
    }
}

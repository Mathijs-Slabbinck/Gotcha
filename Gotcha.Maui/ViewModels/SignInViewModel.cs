using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace Gotcha.Maui.ViewModels;

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
        set { SetProperty(ref isPasswordVisible, value); }
    }

    private string errorMessage = string.Empty;
    public string ErrorMessage
    {
        get { return errorMessage; }
        set { SetProperty(ref errorMessage, value); }
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
        ErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(UsernameOrEmail) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Please fill in all fields.";
            return;
        }

        var window = Application.Current!.Windows[0];
        await window.Page!.DisplayAlertAsync(
            "Not yet implemented",
            "Sign-in functionality is not yet available.",
            "OK");
    }

    private void ExecuteTogglePasswordVisibility()
    {
        IsPasswordVisible = !IsPasswordVisible;
    }

    private async void ExecuteForgotPasswordCommand()
    {
        await Shell.Current.GoToAsync("//ResetPassword");
    }

    private async void ExecuteSignUpCommand()
    {
        await Shell.Current.GoToAsync("//SignUp");
    }
}

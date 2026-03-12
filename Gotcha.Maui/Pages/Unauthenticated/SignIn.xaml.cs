using Gotcha.Maui.ViewModels;

namespace Gotcha.Maui.Pages.Unauthenticated;

public partial class SignIn : ContentPage
{
    private readonly SignInViewModel viewModel;

    public SignIn(SignInViewModel viewModel)
    {
        InitializeComponent();
        this.viewModel = viewModel;
        BindingContext = viewModel;
    }
}

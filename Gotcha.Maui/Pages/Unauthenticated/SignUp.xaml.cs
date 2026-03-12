using Gotcha.Maui.ViewModels;

namespace Gotcha.Maui.Pages.Unauthenticated;

public partial class SignUp : ContentPage
{
    private readonly SignUpViewModel viewModel;

    public SignUp(SignUpViewModel viewModel)
    {
        InitializeComponent();
        this.viewModel = viewModel;
        BindingContext = viewModel;
    }
}

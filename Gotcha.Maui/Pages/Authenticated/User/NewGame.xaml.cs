using Gotcha.Maui.ViewModels;

namespace Gotcha.Maui.Pages.Authenticated.User;

public partial class NewGame : ContentPage
{
    private readonly NewGameViewModel viewModel;

    public NewGame(NewGameViewModel viewModel)
    {
        InitializeComponent();
        this.viewModel = viewModel;
        BindingContext = viewModel;
    }
}

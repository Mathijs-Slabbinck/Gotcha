using Gotcha.Maui.ViewModels;

namespace Gotcha.Maui.Pages.Authenticated.Player;

public partial class Home : ContentPage
{
    public Home(PlayerHomeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }
}

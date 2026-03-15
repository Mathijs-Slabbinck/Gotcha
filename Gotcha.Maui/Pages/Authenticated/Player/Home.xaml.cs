using Gotcha.Maui.ViewModels;

namespace Gotcha.Maui.Pages.Authenticated.Player;

public partial class Home : ContentPage
{
    private readonly PlayerHomeViewModel _viewModel;

    public Home(PlayerHomeViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadData();
    }
}

using Gotcha.Maui.ViewModels;

namespace Gotcha.Maui.Pages.Authenticated.Player;

public partial class Settings : ContentPage
{
    private readonly PlayerSettingsViewModel _viewModel;

    public Settings(PlayerSettingsViewModel viewModel)
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

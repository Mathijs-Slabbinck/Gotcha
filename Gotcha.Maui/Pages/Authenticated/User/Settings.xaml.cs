using Gotcha.Maui.ViewModels;

namespace Gotcha.Maui.Pages.Authenticated.User;

public partial class Settings : ContentPage
{
    private readonly SettingsViewModel viewModel;

    public Settings(SettingsViewModel viewModel)
    {
        InitializeComponent();
        this.viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.LoadData();
    }
}

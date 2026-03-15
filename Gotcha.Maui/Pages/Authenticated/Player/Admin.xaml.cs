using Gotcha.Maui.ViewModels;

namespace Gotcha.Maui.Pages.Authenticated.Player;

public partial class Admin : ContentPage
{
    private readonly PlayerAdminViewModel _viewModel;

    public Admin(PlayerAdminViewModel viewModel)
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

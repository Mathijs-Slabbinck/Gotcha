using Gotcha.Maui.ViewModels;

namespace Gotcha.Maui.Pages.Authenticated.Player;

public partial class ConfirmKill : ContentPage
{
    private readonly ConfirmKillViewModel _viewModel;

    public ConfirmKill(ConfirmKillViewModel viewModel)
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

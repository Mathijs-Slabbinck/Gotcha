using Gotcha.Maui.ViewModels;

namespace Gotcha.Maui.Pages.Authenticated.User;

public partial class Home : ContentPage
{
    private readonly HomeViewModel viewModel;

    public Home(HomeViewModel viewModel)
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

using Gotcha.Maui.ViewModels;

namespace Gotcha.Maui.Pages.Authenticated.User;

public partial class Store : ContentPage
{
    private readonly StoreViewModel viewModel;

    public Store(StoreViewModel viewModel)
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

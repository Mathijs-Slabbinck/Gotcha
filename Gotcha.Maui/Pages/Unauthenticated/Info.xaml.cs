using Gotcha.Maui.ViewModels;

namespace Gotcha.Maui.Pages.Unauthenticated;

public partial class Info : ContentPage
{
    private readonly InfoViewModel viewModel;

    public Info(InfoViewModel viewModel)
    {
        InitializeComponent();
        this.viewModel = viewModel;
        BindingContext = viewModel;
    }
}

using Gotcha.Maui.ViewModels;

namespace Gotcha.Maui.Pages.Unauthenticated;

public partial class Contact : ContentPage
{
    private readonly ContactViewModel viewModel;

    public Contact(ContactViewModel viewModel)
    {
        InitializeComponent();
        this.viewModel = viewModel;
        BindingContext = viewModel;
    }
}

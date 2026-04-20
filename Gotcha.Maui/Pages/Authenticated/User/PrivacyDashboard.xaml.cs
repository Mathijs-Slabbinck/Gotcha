using Gotcha.Maui.ViewModels;

namespace Gotcha.Maui.Pages.Authenticated.User;

public partial class PrivacyDashboard : ContentPage
{
	public PrivacyDashboard(PrivacyDashboardViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}

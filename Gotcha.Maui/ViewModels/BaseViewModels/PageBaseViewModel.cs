using CommunityToolkit.Mvvm.ComponentModel;

namespace Gotcha.Maui.ViewModels.BaseViewModels
{
    // Base for ViewModels that load data or run commands against a service.
    // Holds the universal IsBusy + ErrorMessage pair so pages can consistently bind
    // IsEnabled to !IsBusy and show errors in the same way across the app.
    public abstract class PageBaseViewModel : ObservableObject
    {
        private bool isBusy; // = false
        private string errorMessage = string.Empty;

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        public string ErrorMessage
        {
            get { return errorMessage; }
            set { SetProperty(ref errorMessage, value); }
        }
    }
}

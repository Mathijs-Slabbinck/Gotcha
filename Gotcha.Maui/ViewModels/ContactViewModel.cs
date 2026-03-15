using CommunityToolkit.Mvvm.ComponentModel;
using Gotcha.Maui.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Gotcha.Maui.ViewModels
{
    public class ContactViewModel : ObservableObject
    {
        private readonly IContactService contactService;

        private string selectedReason = string.Empty;
        public string SelectedReason
        {
            get { return selectedReason; }
            set { SetProperty(ref selectedReason, value); }
        }

        private ObservableCollection<string> reasons = new ObservableCollection<string>
        {
            "Bug Report",
            "Suggestion",
            "Feedback",
            "Request Custom App or Website",
            "Other"
        };
        public ObservableCollection<string> Reasons
        {
            get { return reasons; }
            set { SetProperty(ref reasons, value); }
        }

        private string message = string.Empty;
        public string Message
        {
            get { return message; }
            set { SetProperty(ref message, value); }
        }

        private string errorMessage = string.Empty;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { SetProperty(ref errorMessage, value); }
        }

        private string successMessage = string.Empty;
        public string SuccessMessage
        {
            get { return successMessage; }
            set { SetProperty(ref successMessage, value); }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        public ICommand SubmitCommand { get; }
        public ICommand GoBackCommand { get; }

        public ContactViewModel(IContactService contactService)
        {
            this.contactService = contactService;

            SubmitCommand = new Command(ExecuteSubmitCommand);
            GoBackCommand = new Command(ExecuteGoBackCommand);
        }

        private async void ExecuteSubmitCommand()
        {
            try
            {
                ErrorMessage = string.Empty;
                SuccessMessage = string.Empty;

                if (string.IsNullOrWhiteSpace(SelectedReason))
                {
                    ErrorMessage = "Please select a reason for contact.";
                    return;
                }

                if (string.IsNullOrWhiteSpace(Message))
                {
                    ErrorMessage = "Please enter a message.";
                    return;
                }

                IsBusy = true;

                await contactService.SubmitAsync(SelectedReason, Message);

                SuccessMessage = "Your message has been sent. We'll get back to you soon!";
                SelectedReason = string.Empty;
                Message = string.Empty;
            }
            catch
            {
                ErrorMessage = "Something went wrong. Please try again.";
            }

            IsBusy = false;
        }

        private async void ExecuteGoBackCommand()
        {
            try
            {
                await Shell.Current.GoToAsync(Routes.SignIn);
            }
            catch
            {
                ErrorMessage = "Something went wrong. Please try again.";
            }
        }
    }
}

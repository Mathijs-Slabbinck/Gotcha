using System.Windows.Input;
using Gotcha.Maui.Services;
using Gotcha.Maui.ViewModels.BaseViewModels;

namespace Gotcha.Maui.ViewModels
{
    public class PrivacyDashboardViewModel : PageBaseViewModel
    {
        private readonly IUserService _userService;
        private readonly SessionService _sessionService;

        public ICommand DownloadDataCommand { get; }
        public ICommand DeleteAccountCommand { get; }

        public PrivacyDashboardViewModel(IUserService userService, SessionService sessionService)
        {
            _userService = userService;
            _sessionService = sessionService;

            DownloadDataCommand = new Command(ExecuteDownloadDataCommand);
            DeleteAccountCommand = new Command(ExecuteDeleteAccountCommand);
        }

        private async void ExecuteDownloadDataCommand()
        {
            string? filePath = null;

            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;

                string? json = await _userService.ExportDataAsync();

                if (json == null)
                {
                    ErrorMessage = "Something went wrong exporting your data.";
                }
                else
                {
                    string fileName = $"gotcha-data-{DateTime.UtcNow:yyyyMMdd}.json";
                    filePath = Path.Combine(FileSystem.CacheDirectory, fileName);
                    await File.WriteAllTextAsync(filePath, json);

                    await Share.Default.RequestAsync(new ShareFileRequest
                    {
                        Title = "Your Gotcha data",
                        File = new ShareFile(filePath)
                    });
                }
            }
            catch
            {
                ErrorMessage = "Something went wrong. Please try again.";
            }

            if (filePath != null && File.Exists(filePath))
            {
                try { File.Delete(filePath); } catch { }
            }

            IsBusy = false;
        }

        private async void ExecuteDeleteAccountCommand()
        {
            try
            {
                bool confirm = await Shell.Current.DisplayAlertAsync(
                    "Are you sure?",
                    "This cannot be undone. Your account will be deactivated and your personal data anonymized. Game records will remain visible as 'Deleted User'.",
                    "Yes, delete my account",
                    "Cancel");

                if (!confirm)
                {
                    return;
                }

                IsBusy = true;
                ErrorMessage = string.Empty;

                bool success = await _userService.DeleteAccountAsync();

                if (success)
                {
                    await _sessionService.SignOutAsync();
                }
                else
                {
                    ErrorMessage = "Something went wrong deleting your account.";
                }
            }
            catch
            {
                ErrorMessage = "Something went wrong. Please try again.";
            }

            IsBusy = false;
        }
    }
}

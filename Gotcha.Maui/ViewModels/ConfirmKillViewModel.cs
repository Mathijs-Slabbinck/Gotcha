using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace Gotcha.Maui.ViewModels
{
    public class ConfirmKillViewModel : ObservableObject
    {
        private string targetName = string.Empty;
        public string TargetName
        {
            get { return targetName; }
            set { SetProperty(ref targetName, value); }
        }

        private string targetUsername = string.Empty;
        public string TargetUsername
        {
            get { return targetUsername; }
            set { SetProperty(ref targetUsername, value); }
        }

        private string weapon = string.Empty;
        public string Weapon
        {
            get { return weapon; }
            set { SetProperty(ref weapon, value); }
        }

        private string hunterName = string.Empty;
        public string HunterName
        {
            get { return hunterName; }
            set { SetProperty(ref hunterName, value); }
        }

        private string hunterUsername = string.Empty;
        public string HunterUsername
        {
            get { return hunterUsername; }
            set { SetProperty(ref hunterUsername, value); }
        }

        private bool isAssassinMode = true;
        public bool IsAssassinMode
        {
            get { return isAssassinMode; }
            set { SetProperty(ref isAssassinMode, value); }
        }

        private bool showHunter = true;
        public bool ShowHunter
        {
            get { return showHunter; }
            set { SetProperty(ref showHunter, value); }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        private string errorMessage = string.Empty;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { SetProperty(ref errorMessage, value); }
        }

        public ICommand ConfirmKillCommand { get; }
        public ICommand ConfirmDeathCommand { get; }
        public ICommand ConfirmHunterKillCommand { get; }

        public ConfirmKillViewModel()
        {
            ConfirmKillCommand = new Command(ExecuteConfirmKillCommand);
            ConfirmDeathCommand = new Command(ExecuteConfirmDeathCommand);
            ConfirmHunterKillCommand = new Command(ExecuteConfirmHunterKillCommand);

            LoadMockData();
        }

        private void LoadMockData()
        {
            TargetName = "Player Bravo";
            TargetUsername = "BravoFox";
            Weapon = "Water Gun";
            HunterName = "Player Charlie";
            HunterUsername = "CharlieGhost";
        }

        private async void ExecuteConfirmKillCommand()
        {
            try
            {
                await Shell.Current.DisplayAlertAsync(
                    "Not yet implemented",
                    "Kill confirmation is not yet available.",
                    "OK");
            }
            catch
            {
                ErrorMessage = "Something went wrong. Please try again.";
            }
        }

        private async void ExecuteConfirmDeathCommand()
        {
            try
            {
                await Shell.Current.DisplayAlertAsync(
                    "Not yet implemented",
                    "Death confirmation is not yet available.",
                    "OK");
            }
            catch
            {
                ErrorMessage = "Something went wrong. Please try again.";
            }
        }

        private async void ExecuteConfirmHunterKillCommand()
        {
            try
            {
                await Shell.Current.DisplayAlertAsync(
                    "Not yet implemented",
                    "Hunter kill confirmation is not yet available.",
                    "OK");
            }
            catch
            {
                ErrorMessage = "Something went wrong. Please try again.";
            }
        }
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using Gotcha.Maui.Services;
using System.Windows.Input;

namespace Gotcha.Maui.ViewModels
{
    public class ConfirmKillViewModel : ObservableObject
    {
        private readonly IPlayerService playerService;

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

        private bool isAssassinMode;
        public bool IsAssassinMode
        {
            get { return isAssassinMode; }
            set { SetProperty(ref isAssassinMode, value); }
        }

        private bool showHunter;
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

        public ConfirmKillViewModel(IPlayerService playerService)
        {
            this.playerService = playerService;

            ConfirmKillCommand = new Command(ExecuteConfirmKillCommand);
            ConfirmDeathCommand = new Command(ExecuteConfirmDeathCommand);
            ConfirmHunterKillCommand = new Command(ExecuteConfirmHunterKillCommand);
        }

        public async void LoadData()
        {
            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;

                var data = await playerService.GetConfirmKillDataAsync(Guid.Empty);

                TargetName = data.TargetName;
                TargetUsername = data.TargetUsername;
                Weapon = data.Weapon;
                HunterName = data.HunterName;
                HunterUsername = data.HunterUsername;
                IsAssassinMode = data.IsAssassinMode;
                ShowHunter = data.ShowHunter;
            }
            catch
            {
                ErrorMessage = "Something went wrong loading kill data.";
            }

            IsBusy = false;
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

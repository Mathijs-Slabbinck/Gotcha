using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace Gotcha.Maui.ViewModels
{
    public class StoreViewModel : ObservableObject
    {
        // Game feature unlocks (mock: all locked)
        private bool assassinUnlocked;
        public bool AssassinUnlocked
        {
            get { return assassinUnlocked; }
            set { SetProperty(ref assassinUnlocked, value); }
        }

        private bool chaosUnlocked;
        public bool ChaosUnlocked
        {
            get { return chaosUnlocked; }
            set { SetProperty(ref chaosUnlocked, value); }
        }

        private bool timedKillsUnlocked;
        public bool TimedKillsUnlocked
        {
            get { return timedKillsUnlocked; }
            set { SetProperty(ref timedKillsUnlocked, value); }
        }

        private bool customKillMethodsUnlocked;
        public bool CustomKillMethodsUnlocked
        {
            get { return customKillMethodsUnlocked; }
            set { SetProperty(ref customKillMethodsUnlocked, value); }
        }

        // Lobby size unlocks (mock: all not owned)
        private bool lobby100Owned;
        public bool Lobby100Owned
        {
            get { return lobby100Owned; }
            set { SetProperty(ref lobby100Owned, value); }
        }

        private bool lobby150Owned;
        public bool Lobby150Owned
        {
            get { return lobby150Owned; }
            set { SetProperty(ref lobby150Owned, value); }
        }

        private bool lobby500Owned;
        public bool Lobby500Owned
        {
            get { return lobby500Owned; }
            set { SetProperty(ref lobby500Owned, value); }
        }

        private bool lobby10000Owned;
        public bool Lobby10000Owned
        {
            get { return lobby10000Owned; }
            set { SetProperty(ref lobby10000Owned, value); }
        }

        // Subscription plan
        private string currentPlan = "Standard";
        public string CurrentPlan
        {
            get { return currentPlan; }
            set
            {
                if (SetProperty(ref currentPlan, value))
                {
                    OnPropertyChanged(nameof(IsStandardPlan));
                    OnPropertyChanged(nameof(IsPremiumPlan));
                    OnPropertyChanged(nameof(IsDeluxePlan));
                }
            }
        }

        public bool IsStandardPlan
        {
            get { return CurrentPlan == "Standard"; }
        }

        public bool IsPremiumPlan
        {
            get { return CurrentPlan == "Premium"; }
        }

        public bool IsDeluxePlan
        {
            get { return CurrentPlan == "Deluxe"; }
        }

        private string errorMessage = string.Empty;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { SetProperty(ref errorMessage, value); }
        }

        public ICommand BuyFeatureCommand { get; }
        public ICommand SubscribeCommand { get; }

        public StoreViewModel()
        {
            BuyFeatureCommand = new Command<string>(ExecuteBuyFeatureCommand);
            SubscribeCommand = new Command<string>(ExecuteSubscribeCommand);
        }

        private async void ExecuteBuyFeatureCommand(string featureName)
        {
            try
            {
                await Shell.Current.DisplayAlertAsync(
                    "Not yet implemented",
                    $"Purchasing \"{featureName}\" is not yet available.",
                    "OK");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        private async void ExecuteSubscribeCommand(string planName)
        {
            try
            {
                await Shell.Current.DisplayAlertAsync(
                    "Not yet implemented",
                    $"Subscribing to \"{planName}\" is not yet available.",
                    "OK");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}

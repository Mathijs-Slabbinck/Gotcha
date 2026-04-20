using Gotcha.Maui.Services;
using Gotcha.Maui.ViewModels.BaseViewModels;
using Gotcha.Shared.Enums;
using System.Windows.Input;

namespace Gotcha.Maui.ViewModels
{
    public class StoreViewModel : PageBaseViewModel
    {
        private readonly IStoreService storeService;

        // Game feature unlocks
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

        // Lobby size unlocks
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
        private Plan currentPlan = Plan.Standard;
        public Plan CurrentPlan
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
            get { return CurrentPlan == Plan.Standard; }
        }

        public bool IsPremiumPlan
        {
            get { return CurrentPlan == Plan.Premium; }
        }

        public bool IsDeluxePlan
        {
            get { return CurrentPlan == Plan.Deluxe; }
        }

        public ICommand BuyFeatureCommand { get; }
        public ICommand SubscribeCommand { get; }

        public StoreViewModel(IStoreService storeService)
        {
            this.storeService = storeService;

            BuyFeatureCommand = new Command<string>(ExecuteBuyFeatureCommand);
            SubscribeCommand = new Command<string>(ExecuteSubscribeCommand);
        }

        public async void LoadData()
        {
            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;

                var state = await storeService.GetStoreStateAsync();

                AssassinUnlocked = state.AssassinUnlocked;
                ChaosUnlocked = state.ChaosUnlocked;
                TimedKillsUnlocked = state.TimedKillsUnlocked;
                CustomKillMethodsUnlocked = state.CustomKillMethodsUnlocked;
                Lobby100Owned = state.Lobby100Owned;
                Lobby150Owned = state.Lobby150Owned;
                Lobby500Owned = state.Lobby500Owned;
                Lobby10000Owned = state.Lobby10000Owned;
                CurrentPlan = state.CurrentPlan;
            }
            catch
            {
                ErrorMessage = "Something went wrong loading the store.";
            }

            IsBusy = false;
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
            catch
            {
                ErrorMessage = "Something went wrong. Please try again.";
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
            catch
            {
                ErrorMessage = "Something went wrong. Please try again.";
            }
        }
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using Gotcha.Maui.Services;

namespace Gotcha.Maui.ViewModels
{
    public class HomeViewModel : ObservableObject
    {
        private readonly IUserService userService;

        private string username = string.Empty;
        public string Username
        {
            get { return username; }
            set { SetProperty(ref username, value); }
        }

        private int gamesPlayed;
        public int GamesPlayed
        {
            get { return gamesPlayed; }
            set { SetProperty(ref gamesPlayed, value); }
        }

        private int gamesWon;
        public int GamesWon
        {
            get { return gamesWon; }
            set { SetProperty(ref gamesWon, value); }
        }

        private int totalKills;
        public int TotalKills
        {
            get { return totalKills; }
            set { SetProperty(ref totalKills, value); }
        }

        private int biggestKillStreak;
        public int BiggestKillStreak
        {
            get { return biggestKillStreak; }
            set { SetProperty(ref biggestKillStreak, value); }
        }

        private string accountCreated = string.Empty;
        public string AccountCreated
        {
            get { return accountCreated; }
            set { SetProperty(ref accountCreated, value); }
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

        public HomeViewModel(IUserService userService)
        {
            this.userService = userService;
        }

        public async void LoadData()
        {
            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;

                var profile = await userService.GetProfileAsync();

                Username = profile.Username;
                GamesPlayed = profile.GamesPlayed;
                GamesWon = profile.GamesWon;
                TotalKills = profile.TotalKills;
                BiggestKillStreak = profile.BiggestKillStreak;
                AccountCreated = profile.AccountCreated;
            }
            catch
            {
                ErrorMessage = "Something went wrong loading your profile.";
            }

            IsBusy = false;
        }
    }
}

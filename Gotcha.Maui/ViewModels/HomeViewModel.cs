using Gotcha.Maui.Services;
using Gotcha.Maui.ViewModels.BaseViewModels;

namespace Gotcha.Maui.ViewModels
{
    public class HomeViewModel : PageBaseViewModel
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

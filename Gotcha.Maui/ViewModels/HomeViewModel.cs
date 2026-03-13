using CommunityToolkit.Mvvm.ComponentModel;

namespace Gotcha.Maui.ViewModels
{
    public class HomeViewModel : ObservableObject
    {
        private string username = "TheLegend27";
        public string Username
        {
            get { return username; }
            set { SetProperty(ref username, value); }
        }

        private int gamesPlayed = 7;
        public int GamesPlayed
        {
            get { return gamesPlayed; }
            set { SetProperty(ref gamesPlayed, value); }
        }

        private int gamesWon = 2;
        public int GamesWon
        {
            get { return gamesWon; }
            set { SetProperty(ref gamesWon, value); }
        }

        private int totalKills = 15;
        public int TotalKills
        {
            get { return totalKills; }
            set { SetProperty(ref totalKills, value); }
        }

        private int biggestKillStreak = 5;
        public int BiggestKillStreak
        {
            get { return biggestKillStreak; }
            set { SetProperty(ref biggestKillStreak, value); }
        }

        private string accountCreated = "January 2025";
        public string AccountCreated
        {
            get { return accountCreated; }
            set { SetProperty(ref accountCreated, value); }
        }
    }
}

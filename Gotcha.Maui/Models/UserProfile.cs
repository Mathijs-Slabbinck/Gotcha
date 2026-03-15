namespace Gotcha.Maui.Models
{
    public class UserProfile
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime Birthday { get; set; }
        public int GamesPlayed { get; set; }
        public int GamesWon { get; set; }
        public int TotalKills { get; set; }
        public int BiggestKillStreak { get; set; }
        public string AccountCreated { get; set; } = string.Empty;
    }
}

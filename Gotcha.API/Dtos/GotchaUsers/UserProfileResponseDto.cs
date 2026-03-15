namespace Gotcha.API.Dtos.GotchaUsers
{
    public class UserProfileResponseDto
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime AccountCreationDate { get; set; }
        public int GamesPlayed { get; set; }
        public int GamesWon { get; set; }
        public int TotalKills { get; set; }
        public int BiggestKillStreak { get; set; }
    }
}

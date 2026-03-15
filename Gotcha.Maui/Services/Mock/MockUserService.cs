using Gotcha.Maui.Models;

namespace Gotcha.Maui.Services.Mock
{
    public class MockUserService : IUserService
    {
        public Task<UserProfile> GetProfileAsync()
        {
            var profile = new UserProfile
            {
                FirstName = "John",
                LastName = "Doe",
                Username = "TheLegend27",
                Email = "john.doe@example.com",
                Birthday = new DateTime(2000, 1, 15),
                GamesPlayed = 7,
                GamesWon = 2,
                TotalKills = 15,
                BiggestKillStreak = 5,
                AccountCreated = "January 2025"
            };

            return Task.FromResult(profile);
        }

        public Task<bool> UpdateProfileAsync(UserProfile profile)
        {
            return Task.FromResult(true);
        }
    }
}

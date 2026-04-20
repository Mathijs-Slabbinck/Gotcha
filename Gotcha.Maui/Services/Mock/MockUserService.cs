using Gotcha.Maui.Models.PageData;

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

        public Task<bool> DeleteAccountAsync()
        {
            return Task.FromResult(true);
        }

        public Task<string?> ExportDataAsync()
        {
            string mockJson = """
            {
              "ExportedAt": "2026-01-01T00:00:00Z",
              "Profile": {
                "FirstName": "John",
                "LastName": "Doe",
                "UserName": "TheLegend27",
                "Email": "john.doe@example.com"
              },
              "Games": [],
              "Kills": [],
              "Deaths": []
            }
            """;
            return Task.FromResult<string?>(mockJson);
        }
    }
}

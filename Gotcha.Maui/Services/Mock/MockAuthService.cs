using Gotcha.Maui.Models;
using Gotcha.Shared.Constants;

namespace Gotcha.Maui.Services.Mock
{
    public class MockAuthService : IAuthService
    {
        public Task<(Guid? UserId, string? ErrorMessage)> SignInAsync(string usernameOrEmail, string password)
        {
            return Task.FromResult<(Guid?, string?)>((DevConstants.TestUserId, null));
        }

        public Task<(bool Success, string? ErrorMessage)> SignUpAsync(SignUpData data)
        {
            return Task.FromResult<(bool, string?)>((true, null));
        }
    }
}

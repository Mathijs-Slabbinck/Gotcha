using Gotcha.Maui.Models;

namespace Gotcha.Maui.Services
{
    public interface IAuthService
    {
        Task<(Guid? UserId, string? ErrorMessage)> SignInAsync(string usernameOrEmail, string password);
        Task<(bool Success, string? ErrorMessage)> SignUpAsync(SignUpData data);
    }
}

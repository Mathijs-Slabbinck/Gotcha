using Gotcha.Maui.Models;

namespace Gotcha.Maui.Services
{
    public interface IUserService
    {
        Task<UserProfile> GetProfileAsync();
        Task<bool> UpdateProfileAsync(UserProfile profile);
    }
}

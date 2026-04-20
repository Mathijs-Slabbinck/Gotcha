using Gotcha.Maui.Models.PageData;

namespace Gotcha.Maui.Services
{
    public interface IUserService
    {
        Task<UserProfile> GetProfileAsync();
        Task<bool> UpdateProfileAsync(UserProfile profile);
    }
}

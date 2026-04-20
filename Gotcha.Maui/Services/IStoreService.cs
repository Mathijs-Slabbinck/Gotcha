using Gotcha.Maui.Models.PageData;

namespace Gotcha.Maui.Services
{
    public interface IStoreService
    {
        Task<StoreState> GetStoreStateAsync();
        Task<bool> BuyFeatureAsync(string featureName);
        Task<bool> SubscribeAsync(string planName);
    }
}

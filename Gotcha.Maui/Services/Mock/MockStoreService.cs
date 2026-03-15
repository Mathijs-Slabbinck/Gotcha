using Gotcha.Maui.Enums;
using Gotcha.Maui.Models;

namespace Gotcha.Maui.Services.Mock
{
    public class MockStoreService : IStoreService
    {
        public Task<StoreState> GetStoreStateAsync()
        {
            var state = new StoreState
            {
                AssassinUnlocked = false,
                ChaosUnlocked = false,
                TimedKillsUnlocked = false,
                CustomKillMethodsUnlocked = false,
                Lobby100Owned = false,
                Lobby150Owned = false,
                Lobby500Owned = false,
                Lobby10000Owned = false,
                CurrentPlan = Plan.Standard
            };

            return Task.FromResult(state);
        }

        public Task<bool> BuyFeatureAsync(string featureName)
        {
            return Task.FromResult(true);
        }

        public Task<bool> SubscribeAsync(string planName)
        {
            return Task.FromResult(true);
        }
    }
}

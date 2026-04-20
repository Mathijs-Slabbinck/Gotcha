using Gotcha.Core.Enums;

namespace Gotcha.Core.Services.Payment
{
    public static class StorePricing
    {
        private static readonly Dictionary<StoreItem, decimal> _prices = new()
        {
            { StoreItem.AssassinMode, 3.00m },
            { StoreItem.ChaosMode, 2.00m },
            { StoreItem.TimedKills, 2.00m },
            { StoreItem.CustomKillMethods, 2.00m },
            { StoreItem.Lobby100, 1.50m },
            { StoreItem.Lobby150, 3.00m },
            { StoreItem.Lobby500, 6.00m },
            { StoreItem.Lobby10000, 10.00m }
        };

        private static readonly Dictionary<StoreItem, string> _descriptions = new()
        {
            { StoreItem.AssassinMode, "Assassin Mode" },
            { StoreItem.ChaosMode, "Chaos Mode" },
            { StoreItem.TimedKills, "Timed Kills" },
            { StoreItem.CustomKillMethods, "Different Kill Methods" },
            { StoreItem.Lobby100, "Lobby Size: 100 Players" },
            { StoreItem.Lobby150, "Lobby Size: 150 Players" },
            { StoreItem.Lobby500, "Lobby Size: 500 Players" },
            { StoreItem.Lobby10000, "Lobby Size: 10,000 Players" }
        };

        public static decimal GetPrice(StoreItem item)
        {
            return _prices[item];
        }

        public static string GetDescription(StoreItem item)
        {
            return _descriptions[item];
        }
    }
}

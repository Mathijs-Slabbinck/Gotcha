namespace Gotcha.Maui.Constants
{
    public static class DevConstants
    {
        // Must match Seeder.TestUserId in Gotcha.Core
        public static readonly Guid TestUserId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

        // Set to false to use real API services instead of mocks
        public const bool UseMockServices = true;
    }
}

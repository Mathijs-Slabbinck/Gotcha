namespace Gotcha.Shared.Constants
{
    // Fixed seed-user IDs used for development and mock services before auth is wired up.
    // The Seeder creates users with these exact IDs; the API and MAUI app reference them
    // so they all agree on which user is "the current user" during development.
    public static class DevConstants
    {
        public static readonly Guid TestUserId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        public static readonly Guid TestUser2Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
        public static readonly Guid TestUser3Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");
    }
}

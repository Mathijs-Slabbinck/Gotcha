namespace Gotcha.Maui.Services
{
    // Singleton that holds the currently signed-in user and the player they have selected
    // (a user joins many games, each as a separate Player — the selected one drives the Player tab bar).
    public class SessionService
    {
        public const string StoredUserIdKey = "userId";

        public Guid CurrentUserId { get; private set; } = Guid.Empty;
        public Guid CurrentPlayerId { get; private set; } = Guid.Empty;

        public void SetUser(Guid userId)
        {
            CurrentUserId = userId;
        }

        public void SetPlayer(Guid playerId)
        {
            CurrentPlayerId = playerId;
        }

        public void Clear()
        {
            CurrentUserId = Guid.Empty;
            CurrentPlayerId = Guid.Empty;
        }

        // Clears in-memory session, removes the persisted userId, flips the shell to the
        // unauthenticated TabBar, and navigates to Sign In. Centralised so every sign-out
        // path (logout, delete account) stays in sync.
        public async Task SignOutAsync()
        {
            Clear();
            SecureStorage.Remove(StoredUserIdKey);

            if (Shell.Current is AppShell appShell)
            {
                appShell.SwitchToUnauthenticatedTabBar();
            }

            await Shell.Current.GoToAsync(Routes.SignIn);
        }
    }
}

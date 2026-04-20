using Gotcha.Maui.Services;

namespace Gotcha.Maui
{
    public partial class AppShell : Shell
    {
        private const int UnauthenticatedTabIndex = 0;
        private const int AuthenticatedUserTabIndex = 1;

        private readonly SessionService _sessionService;

        public AppShell(SessionService sessionService)
        {
            InitializeComponent();
            _sessionService = sessionService;
            _ = CheckRememberedSessionAsync();
        }

        private async Task CheckRememberedSessionAsync()
        {
            string? stored = await SecureStorage.GetAsync("userId");

            if (stored != null && Guid.TryParse(stored, out Guid userId))
            {
                _sessionService.SetUser(userId);
                SwitchToUserTabBar();
            }
        }

        public void SwitchToUserTabBar()
        {
            CurrentItem = Items[AuthenticatedUserTabIndex];
        }

        public void SwitchToUnauthenticatedTabBar()
        {
            CurrentItem = Items[UnauthenticatedTabIndex];
        }
    }
}

namespace Gotcha.Maui
{
    /* App is the root of the MAUI application. It is a partial class because the other half
     * is generated from App.xaml (resource dictionaries, global styles, etc.). */
    public partial class App : Application
    {
        /* AppShell is the root navigation container (tab bars, routes). We inject it via DI
         * so it gets its own dependencies (SessionService) properly wired up.
         * Without DI, new AppShell() would crash because AppShell now requires SessionService. */
        private readonly AppShell _appShell;

        public App(AppShell appShell)
        {
            InitializeComponent(); // loads App.xaml (merges resource dictionaries)
            _appShell = appShell;
        }

        /* MAUI calls CreateWindow when it needs to create the OS-level window that holds the UI.
         * On Android this maps to an Activity, on iOS to a UIWindow, on Windows to a WinUI Window.
         * We override it to control what gets shown inside that window — here: our AppShell.
         *
         * IActivationState? activationState — info about HOW the app was opened:
         *   - normal launch: null or empty
         *   - opened from a notification: contains the notification payload
         *   - opened via a URL scheme (deep link): contains the URL
         * We ignore it here, but you'd read it if you wanted to navigate somewhere specific on launch. */
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(_appShell); // wrap AppShell in an OS window and show it
        }
    }
}

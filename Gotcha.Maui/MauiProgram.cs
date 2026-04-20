using CommunityToolkit.Maui;
using Gotcha.Maui.Constants;
using Gotcha.Maui.Services;
using Gotcha.Maui.Services.Api;
using Gotcha.Maui.Services.Mock;
using Gotcha.Maui.ViewModels;
using Microsoft.Extensions.Logging;

using Contact = Gotcha.Maui.Pages.Unauthenticated.Contact;
using Info = Gotcha.Maui.Pages.Unauthenticated.Info;
using ResetPassword = Gotcha.Maui.Pages.Unauthenticated.ResetPassword;
using SignIn = Gotcha.Maui.Pages.Unauthenticated.SignIn;
using SignUp = Gotcha.Maui.Pages.Unauthenticated.SignUp;

using UserHome = Gotcha.Maui.Pages.Authenticated.User.Home;
using UserGames = Gotcha.Maui.Pages.Authenticated.User.Games;
using UserSettings = Gotcha.Maui.Pages.Authenticated.User.Settings;
using UserStore = Gotcha.Maui.Pages.Authenticated.User.Store;
using NewGame = Gotcha.Maui.Pages.Authenticated.User.NewGame;

using PlayerHome = Gotcha.Maui.Pages.Authenticated.Player.Home;
using PlayerConfirmKill = Gotcha.Maui.Pages.Authenticated.Player.ConfirmKill;
using PlayerSettings = Gotcha.Maui.Pages.Authenticated.Player.Settings;
using PlayerAdmin = Gotcha.Maui.Pages.Authenticated.Player.Admin;

namespace Gotcha.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            Routing.RegisterRoute(Routes.ResetPassword, typeof(ResetPassword));
            Routing.RegisterRoute(Routes.NewGame, typeof(NewGame));

            MauiAppBuilder builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Nosifer-Regular.ttf", "Nosifer");
                    fonts.AddFont("Bungee-Regular.ttf", "Bungee");
                    fonts.AddFont("Roboto-Regular.ttf", "Roboto");
                    fonts.AddFont("Roboto-Bold.ttf", "RobotoBold");
                    fonts.AddFont("RobotoSlab-Regular.ttf", "RobotoSlab");
                    fonts.AddFont("RobotoSlab-Bold.ttf", "RobotoSlabBold");
                });

            // Make switch off-track visible on white card backgrounds
            Microsoft.Maui.Handlers.SwitchHandler.Mapper.AppendToMapping("OffTrackColor", (handler, view) =>
            {
#if WINDOWS
                var toggle = handler.PlatformView;
                var offTrackBrush = new Microsoft.UI.Xaml.Media.SolidColorBrush(
                    Windows.UI.Color.FromArgb(255, 200, 200, 200));

                toggle.Resources["ToggleSwitchFillOff"] = offTrackBrush;
                toggle.Resources["ToggleSwitchFillOffPointerOver"] = offTrackBrush;
                toggle.Resources["ToggleSwitchFillOffPressed"] = offTrackBrush;
                toggle.Resources["ToggleSwitchStrokeOff"] = offTrackBrush;
                toggle.Resources["ToggleSwitchStrokeOffPointerOver"] = offTrackBrush;
                toggle.Resources["ToggleSwitchStrokeOffPressed"] = offTrackBrush;
#elif ANDROID
                var offTrackColor = Android.Graphics.Color.ParseColor("#C8C8C8");
                var onTrackColor = Android.Graphics.Color.ParseColor("#5AD6DE");

                handler.PlatformView.TrackTintList = new Android.Content.Res.ColorStateList(
                    new int[][]
                    {
                        new int[] { Android.Resource.Attribute.StateChecked },
                        new int[] { -Android.Resource.Attribute.StateChecked }
                    },
                    new int[] { onTrackColor, offTrackColor });
#endif
            });

            // HttpClient for API calls
            builder.Services.AddHttpClient("GotchaApi", client =>
            {
                // Android emulator can't reach localhost — use 10.0.2.2
                string baseUrl = DeviceInfo.Platform == DevicePlatform.Android
                    ? "http://10.0.2.2:5208"
                    : "http://localhost:5208";
                client.BaseAddress = new Uri(baseUrl);
            });

            // Session — singleton that holds the current user ID across the app
            builder.Services.AddSingleton<SessionService>();

            // Services — toggle between mock and API implementations
            if (DevConstants.UseMockServices)
            {
                builder.Services.AddTransient<IAuthService, MockAuthService>();
                builder.Services.AddTransient<IUserService, MockUserService>();
                builder.Services.AddTransient<IGameService, MockGameService>();
                builder.Services.AddTransient<IPlayerService, MockPlayerService>();
                builder.Services.AddTransient<IStoreService, MockStoreService>();
                builder.Services.AddTransient<IContactService, MockContactService>();
            }
            else
            {
                builder.Services.AddTransient<IAuthService, ApiAuthService>();
                builder.Services.AddTransient<IUserService, ApiUserService>();
                builder.Services.AddTransient<IGameService, ApiGameService>();
                builder.Services.AddTransient<IPlayerService, ApiPlayerService>();
                builder.Services.AddTransient<IStoreService, ApiStoreService>();
                builder.Services.AddTransient<IContactService, ApiContactService>();
            }

            // AppShell — singleton so DI can inject SessionService into it
            builder.Services.AddSingleton<AppShell>();

            // Unauthenticated ViewModels
            builder.Services.AddTransient<SignInViewModel>();
            builder.Services.AddTransient<SignUpViewModel>();
            builder.Services.AddTransient<InfoViewModel>();
            builder.Services.AddTransient<ContactViewModel>();

            // Authenticated User ViewModels
            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddTransient<GamesViewModel>();
            builder.Services.AddTransient<NewGameViewModel>();
            builder.Services.AddTransient<SettingsViewModel>();
            builder.Services.AddTransient<StoreViewModel>();

            // Unauthenticated Pages
            builder.Services.AddTransient<SignIn>();
            builder.Services.AddTransient<SignUp>();
            builder.Services.AddTransient<ResetPassword>();
            builder.Services.AddTransient<Contact>();
            builder.Services.AddTransient<Info>();

            // Authenticated User Pages
            builder.Services.AddTransient<UserHome>();
            builder.Services.AddTransient<UserGames>();
            builder.Services.AddTransient<UserSettings>();
            builder.Services.AddTransient<UserStore>();
            builder.Services.AddTransient<NewGame>();

            // Authenticated Player ViewModels
            builder.Services.AddTransient<PlayerHomeViewModel>();
            builder.Services.AddTransient<ConfirmKillViewModel>();
            builder.Services.AddTransient<PlayerSettingsViewModel>();
            builder.Services.AddTransient<PlayerAdminViewModel>();

            // Authenticated Player Pages
            builder.Services.AddTransient<PlayerHome>();
            builder.Services.AddTransient<PlayerConfirmKill>();
            builder.Services.AddTransient<PlayerSettings>();
            builder.Services.AddTransient<PlayerAdmin>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

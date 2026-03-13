using CommunityToolkit.Maui;
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

namespace Gotcha.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            Routing.RegisterRoute("ResetPassword", typeof(ResetPassword));
            Routing.RegisterRoute("NewGame", typeof(NewGame));

            var builder = MauiApp.CreateBuilder();
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

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

using CommunityToolkit.Maui;
using Gotcha.Maui.Pages.Unauthenticated;
using Gotcha.Maui.ViewModels;
using Microsoft.Extensions.Logging;

namespace Gotcha.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Pages & ViewModels
            builder.Services.AddSingleton<SignInViewModel>();
            builder.Services.AddSingleton<SignIn>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

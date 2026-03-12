using CommunityToolkit.Maui;
using Gotcha.Maui.ViewModels;
using Microsoft.Extensions.Logging;

using Contact = Gotcha.Maui.Pages.Unauthenticated.Contact;
using Info = Gotcha.Maui.Pages.Unauthenticated.Info;
using ResetPassword = Gotcha.Maui.Pages.Unauthenticated.ResetPassword;
using SignIn = Gotcha.Maui.Pages.Unauthenticated.SignIn;
using SignUp = Gotcha.Maui.Pages.Unauthenticated.SignUp;

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
                    fonts.AddFont("Nosifer-Regular.ttf", "Nosifer");
                    fonts.AddFont("Bungee-Regular.ttf", "Bungee");
                    fonts.AddFont("Roboto-Regular.ttf", "Roboto");
                    fonts.AddFont("Roboto-Bold.ttf", "RobotoBold");
                    fonts.AddFont("RobotoSlab-Regular.ttf", "RobotoSlab");
                    fonts.AddFont("RobotoSlab-Bold.ttf", "RobotoSlabBold");
                });

            // ViewModels
            builder.Services.AddTransient<SignInViewModel>();
            builder.Services.AddTransient<SignUpViewModel>();
            builder.Services.AddTransient<InfoViewModel>();
            builder.Services.AddTransient<ContactViewModel>();

            // Pages
            builder.Services.AddTransient<SignIn>();
            builder.Services.AddTransient<SignUp>();
            builder.Services.AddTransient<ResetPassword>();
            builder.Services.AddTransient<Contact>();
            builder.Services.AddTransient<Info>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

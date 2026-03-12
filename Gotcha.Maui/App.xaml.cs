using CommunityToolkit.Maui.Converters;

namespace Gotcha.Maui
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Resources["InvertBoolConverter"] = new InvertedBoolConverter();
            Resources["IsStringNotNullOrEmptyConverter"] = new IsStringNotNullOrEmptyConverter();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}
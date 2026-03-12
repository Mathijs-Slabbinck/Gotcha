using Gotcha.Maui.Pages.Unauthenticated;

using Contact = Gotcha.Maui.Pages.Unauthenticated.Contact;

namespace Gotcha.Maui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("SignUp", typeof(SignUp));
            Routing.RegisterRoute("ResetPassword", typeof(ResetPassword));
            Routing.RegisterRoute("Contact", typeof(Contact));
            Routing.RegisterRoute("Info", typeof(Info));
        }
    }
}

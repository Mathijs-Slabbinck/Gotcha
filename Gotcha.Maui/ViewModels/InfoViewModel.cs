using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace Gotcha.Maui.ViewModels
{
    public class InfoViewModel : ObservableObject
    {
        public ICommand BackToSignInCommand => new Command(ExecuteBackToSignInCommand);

        private async void ExecuteBackToSignInCommand()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}

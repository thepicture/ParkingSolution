using System.Windows.Input;
using Xamarin.Forms;

namespace ParkingSolution.XamarinApp.ViewModels
{
    public class WelcomeViewModel : BaseViewModel
    {

        private Command tryCommand;

        public ICommand TryCommand
        {
            get
            {
                if (tryCommand == null)
                {
                    tryCommand = new Command(Try);
                }

                return tryCommand;
            }
        }

        private void Try()
        {
            AppShell.LoadLoginAndRegisterShell();
        }
    }
}
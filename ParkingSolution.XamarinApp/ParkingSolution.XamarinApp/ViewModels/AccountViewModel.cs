using ParkingSolution.XamarinApp.Services;
using ParkingSolution.XamarinApp.Views;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ParkingSolution.XamarinApp.ViewModels
{
    public class AccountViewModel : BaseViewModel
    {

        private string phoneNumber;

        public string PhoneNumber
        {
            get => phoneNumber;
            set => SetProperty(ref phoneNumber, value);
        }

        private Command exitLoginCommand;

        public ICommand ExitLoginCommand
        {
            get
            {
                if (exitLoginCommand == null)
                {
                    exitLoginCommand = new Command(ExitLogin);
                }

                return exitLoginCommand;
            }
        }

        internal void OnAppearing()
        {
            Task.Run(() =>
            {
                string[] phoneNumberAndPassword =
                PhoneNumberAndPasswordFromBasicDecoder.Decode();
                PhoneNumber = phoneNumberAndPassword[0];
            });
        }

        private async void ExitLogin()
        {
            if (await FeedbackService.Ask("Выйти из аккаунта?"))
            {
                AppIdentity.Reset();
                (AppShell.Current as AppShell).LoadLoginAndRegisterShell();
            }
        }

        private Command goToMyCarsPageCommand;

        public ICommand GoToMyCarsPageCommand
        {
            get
            {
                if (goToMyCarsPageCommand == null)
                {
                    goToMyCarsPageCommand = new Command(GoToMyCarsPageAsync);
                }

                return goToMyCarsPageCommand;
            }
        }

        private async void GoToMyCarsPageAsync()
        {
            await Shell.Current.GoToAsync(
                $"{nameof(MyCarsPage)}");
        }
    }
}
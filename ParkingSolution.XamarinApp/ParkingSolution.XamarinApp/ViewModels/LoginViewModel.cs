using ParkingSolution.XamarinApp.Models;
using ParkingSolution.XamarinApp.Models.Serialized;
using ParkingSolution.XamarinApp.Services;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ParkingSolution.XamarinApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        internal void OnAppearing()
        {

        }

        private string phoneNumber;

        public string PhoneNumber
        {
            get => phoneNumber;
            set => SetProperty(ref phoneNumber, value);
        }

        private string password;

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        private Command loginCommand;

        public ICommand LoginCommand
        {
            get
            {
                if (loginCommand == null)
                {
                    loginCommand = new Command(LoginAsync);
                }

                return loginCommand;
            }
        }

        private async void LoginAsync()
        {
            IsBusy = true;
            string rawPhoneNumber = MaskDeleter.DeleteMask(PhoneNumber);
            SerializedLoginUser loginUser = new SerializedLoginUser
            {
                PhoneNumber = rawPhoneNumber,
                Password = Password,
                IsRememberMe = IsRememberMe
            };
            if (await LoginDataStore.AddItemAsync(loginUser))
            {
                (AppShell.Current as AppShell).SetShellStacksDependingOnRole();
            }
            IsBusy = false;
        }

        private Command exitCommand;
        private bool isRememberMe;

        public ICommand ExitCommand
        {
            get
            {
                if (exitCommand == null)
                {
                    exitCommand = new Command(ExitAsync);
                }

                return exitCommand;
            }
        }

        public bool IsRememberMe
        {
            get => isRememberMe;
            set => SetProperty(ref isRememberMe, value);
        }

        private async void ExitAsync()
        {
            if (await FeedbackService.Ask("Выйти из приложения?"))
            {
                System.Environment.Exit(0);
            }
        }
    }
}
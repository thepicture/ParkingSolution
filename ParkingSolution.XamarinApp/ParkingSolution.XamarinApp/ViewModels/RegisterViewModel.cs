using ParkingSolution.XamarinApp.Models;
using ParkingSolution.XamarinApp.Models.Serialized;
using System.Windows.Input;
using Xamarin.Forms;

namespace ParkingSolution.XamarinApp.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private SerializedUserType currentUserType;

        public SerializedUserType CurrentUserType
        {
            get => currentUserType;
            set => SetProperty(ref currentUserType, value);
        }

        private string phoneNumber;

        public string PhoneNumber
        {
            get => phoneNumber;
            set => SetProperty(ref phoneNumber, value);
        }

        private Command registerCommand;

        public ICommand RegisterCommand
        {
            get
            {
                if (registerCommand == null)
                {
                    registerCommand = new Command(RegisterAsync);
                }

                return registerCommand;
            }
        }

        private async void RegisterAsync()
        {
            IsBusy = true;
            IsRefreshing = true;
            string rawPhoneNumber = MaskDeleter.DeleteMask(PhoneNumber);
            SerializedRegistrationUser registrationUser = new SerializedRegistrationUser
            {
                PhoneNumber = rawPhoneNumber,
                Password = Password,
                UserTypeId = CurrentUserType?.Id ?? 0
            };

            if (await RegistrationDataStore.AddItemAsync(registrationUser))
            {
                AppShell.LoadLoginAndRegisterShell();
            }
            IsRefreshing = false;
            IsBusy = false;
        }

        private string password;

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }
    }
}
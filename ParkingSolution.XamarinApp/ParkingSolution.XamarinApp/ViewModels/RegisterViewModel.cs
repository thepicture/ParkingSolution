using ParkingSolution.XamarinApp.Models;
using ParkingSolution.XamarinApp.Models.Serialized;
using System;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ParkingSolution.XamarinApp.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        internal void OnAppearing()
        {

        }

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
            StringBuilder validationErrors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(PhoneNumber)
                || PhoneNumber.Length != 18)
            {
                _ = validationErrors.AppendLine("Введите номер телефона");
            }
            if (string.IsNullOrWhiteSpace(Password))
            {
                _ = validationErrors.AppendLine("Введите пароль");
            }
            if (CurrentUserType == null)
            {
                _ = validationErrors.AppendLine("Укажите тип пользователя");
            }

            if (validationErrors.Length > 0)
            {
                await FeedbackService.InformError(validationErrors);
                IsBusy = false;
                return;
            }

            string rawPhoneNumber = MaskDeleter.DeleteMask(PhoneNumber);
            SerializedUser identity = new SerializedUser
            {
                PhoneNumber = rawPhoneNumber,
                Password = Password,
                UserTypeId = CurrentUserType.Id
            };

            bool isRegistered;
            try
            {
                isRegistered = await RegistrationService
                .IsRegisteredAsync(identity);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                await FeedbackService.Inform("При регистрации " +
                    "возникла неизвестная ошибка. " +
                    "Проверьте подключение к сети");
                IsBusy = false;
                return;
            }
            if (isRegistered)
            {
                await FeedbackService.Inform(
                    RegistrationService.ValidationResult);
                AppShell.LoadLoginAndRegisterShell();
            }
            else
            {
                await FeedbackService.InformError(
                    RegistrationService.ValidationResult);
            }
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
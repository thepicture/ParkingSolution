using ParkingSolution.XamarinApp.Services;
using System;
using System.Diagnostics;
using System.Text;
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
            StringBuilder validationErrors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(PhoneNumber))
            {
                _ = validationErrors.AppendLine("Введите номер телефона");
            }
            if (string.IsNullOrWhiteSpace(Password))
            {
                _ = validationErrors.AppendLine("Введите пароль");
            }

            if (validationErrors.Length > 0)
            {
                await FeedbackService.InformError(
                    validationErrors.ToString());
                return;
            }

            bool isAuthenticated;
            try
            {
                isAuthenticated = await AuthenticatorService
                .IsCorrectAsync(PhoneNumber, Password);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                await FeedbackService.Inform("Подключение к интернету " +
                     "отсутствует, проверьте подключение " +
                     "и попробуйте ещё раз");
                return;
            }
            if (isAuthenticated)
            {
                string encodedLoginAndPassword =
                    PhoneNumberAndPasswordToBasicEncoder
                    .Encode(PhoneNumber, Password);
                if (IsRememberMe)
                {
                    await SecureStorage
                        .SetAsync("Identity",
                                  encodedLoginAndPassword);
                    await SecureStorage
                       .SetAsync("Role",
                                 AuthenticatorService.Role);
                }
                else
                {
                    (App.Current as App).Role = AuthenticatorService.Role;
                    (App.Current as App).Identity = encodedLoginAndPassword;
                }
                await FeedbackService.Inform("Вы авторизованы " +
                    $"как {AuthenticatorService.Role}");
                (AppShell.Current as AppShell).SetShellStacksDependingOnRole();
            }
            else
            {
                await FeedbackService.InformError("Неверный логин или пароль");
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
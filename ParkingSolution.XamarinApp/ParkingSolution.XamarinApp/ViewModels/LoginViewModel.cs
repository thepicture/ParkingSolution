using ParkingSolution.XamarinApp.Models;
using ParkingSolution.XamarinApp.Models.Serialized;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace ParkingSolution.XamarinApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
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
            IsRefreshing = true;
            string rawPhoneNumber = MaskDeleter.DeleteMask(PhoneNumber);
            SerializedLoginUser loginUser = new SerializedLoginUser
            {
                PhoneNumber = rawPhoneNumber,
                Password = Password,
                IsRememberMe = IsRememberMe
            };
            if (await LoginDataStore.AddItemAsync(loginUser))
            {
                AppShell.SetShellStacksDependingOnRole();
            }
            IsRefreshing = false;
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
                Environment.Exit(0);
            }
        }

        private Command openUrlOptionsCommand;

        public ICommand OpenUrlOptionsCommand
        {
            get
            {
                if (openUrlOptionsCommand == null)
                {
                    openUrlOptionsCommand = new Command(OpenUrlOptionsAsync);
                }

                return openUrlOptionsCommand;
            }
        }

        private async void OpenUrlOptionsAsync()
        {
            Uri currentUrl = App.BaseUrl;
            string url = await AppShell.Current.CurrentPage.DisplayPromptAsync(
                        "Установить URL",
                        "Текущий URL:\n" +
                        $"{currentUrl}\n" +
                        "Введите новый URL",
                        keyboard: Keyboard.Text,
                        initialValue: currentUrl.OriginalString);
            if (url != null)
            {
                App.BaseUrl = new Uri(url);
            }
        }
    }
}
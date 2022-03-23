using ParkingSolution.XamarinApp.Services;
using ParkingSolution.XamarinApp.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ParkingSolution.XamarinApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ParkingPlacesPage), typeof(ParkingPlacesPage));
            Routing.RegisterRoute(nameof(MyCarsPage), typeof(MyCarsPage));
            Routing.RegisterRoute(nameof(AddCarPage), typeof(AddCarPage));

            if (IsLoggedIn())
            {
                SetShellStacksDependingOnRole();
            }
            else
            {
                LoadLoginAndRegisterShell();
            }
        }

        public void LoadLoginAndRegisterShell()
        {
            CommonTabBar.Items.Clear();
            CommonTabBar
                .Items.Add(new ShellContent
                {
                    Route = nameof(LoginPage),
                    Icon = "call_1",
                    Title = "Авторизация",
                    ContentTemplate = new DataTemplate(typeof(LoginPage))
                });
            CommonTabBar
              .Items.Add(new ShellContent
              {
                  Route = nameof(RegisterPage),
                  Icon = "address",
                  Title = "Регистрация",
                  ContentTemplate = new DataTemplate(typeof(RegisterPage))
              });
        }

        private bool IsLoggedIn()
        {
            return SecureStorage.GetAsync("Identity").Result != null;
        }

        public void SetShellStacksDependingOnRole()
        {
            CommonTabBar.Items.Clear();
            switch (AppIdentity.Role)
            {
                case "Администратор":
                    break;
                case "Сотрудник":
                    break;
                case "Клиент":
                    CommonTabBar
                        .Items.Add(new ShellContent
                        {
                            Route = nameof(ParkingsPage),
                            Icon = "location",
                            Title = "Парковки",
                            ContentTemplate = new DataTemplate(typeof(ParkingsPage))
                        });
                    break;
                default:
                    break;
            }
            CommonTabBar
                       .Items.Add(new ShellContent
                       {
                           Route = nameof(AccountPage),
                           Icon = "address",
                           Title = "Аккаунт",
                           ContentTemplate = new DataTemplate(typeof(AccountPage))
                       });
        }
    }
}

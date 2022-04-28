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
            Routing.RegisterRoute(nameof(AddParkingPage), typeof(AddParkingPage));

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
            return AppIdentity.AuthorizationValue != null;
        }

        public void SetShellStacksDependingOnRole()
        {
            CommonTabBar.Items.Clear();
            CommonTabBar
                       .Items.Add(new ShellContent
                       {
                           Route = nameof(ParkingsPage),
                           Icon = "location",
                           Title = "Парковки",
                           ContentTemplate = new DataTemplate(typeof(ParkingsPage))
                       });
            switch (AppIdentity.Role)
            {
                case "Администратор":
                    CommonTabBar
                    .Items.Add(new ShellContent
                    {
                        Route = nameof(EmployeesPage),
                        Icon = "address",
                        Title = "Сотрудники",
                        ContentTemplate = new DataTemplate(typeof(EmployeesPage))
                    });
                    CommonTabBar
                    .Items.Add(new ShellContent
                    {
                        Route = nameof(MyParkingPlacesPage),
                        Icon = "write_letter",
                        Title = "Все бронирования",
                        ContentTemplate = new DataTemplate(typeof(MyParkingPlacesPage))
                    });
                    break;
                case "Сотрудник":
                    CommonTabBar
                     .Items.Add(new ShellContent
                     {
                         Route = nameof(MyParkingPlacesPage),
                         Icon = "write_letter",
                         Title = "Все бронирования",
                         ContentTemplate = new DataTemplate(typeof(MyParkingPlacesPage))
                     });
                    break;
                case "Клиент":
                    CommonTabBar
                     .Items.Add(new ShellContent
                     {
                         Route = nameof(MyParkingPlacesPage),
                         Icon = "write_letter",
                         Title = "Мои бронирования",
                         ContentTemplate = new DataTemplate(typeof(MyParkingPlacesPage))
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

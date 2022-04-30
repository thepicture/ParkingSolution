using ParkingSolution.XamarinApp.Services;
using System;
using System.Net.Http;
using Xamarin.Forms;

namespace ParkingSolution.XamarinApp
{
    public partial class App : Application
    {
        public static string Role { get; set; }
        public static string AuthorizationValue { get; set; }
        public static Uri BaseUrl { get; } =
            new Uri("https://parkingsolution-webapi.conveyor.cloud/api/");
        public static HttpClientHandler ClientHandler = new HttpClientHandler();
        public App()
        {
            ClientHandler.ServerCertificateCustomValidationCallback += (_, __, ___, ____) => true;
            InitializeComponent();
            XF.Material.Forms
                .Material
                .Init(
                this,
                (XF
                .Material
                .Forms
                .Resources
                .MaterialConfiguration)Resources["CommonMaterial"]);

            DependencyService.Register<AndroidFeedbackService>();
            DependencyService.Register<LoginDataStore>();
            DependencyService.Register<RegistrationDataStore>();
            DependencyService.Register<CarDataStore>();
            DependencyService.Register<ReservationDataStore>();
            DependencyService.Register<ParkingDataStore>();
            DependencyService.Register<PaymentHistoryDataStore>();
            DependencyService.Register<UserDataStore>();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

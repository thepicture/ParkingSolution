using ParkingSolution.XamarinApp.Services;
using System;
using System.Net.Http;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ParkingSolution.XamarinApp
{
    public partial class App : Application
    {
        public static string Role { get; set; }
        public static string AuthorizationValue { get; set; }
        public static Uri BaseUrl
        {
            get => new Uri(Preferences.Get(
                nameof(BaseUrl), baseUrl));

            set => Preferences.Set(
                nameof(BaseUrl), value.OriginalString);
        }
        private static readonly string baseUrl =
            "https://parkingsolution-webapi.conveyor.cloud/api/";
        public static HttpClientHandler ClientHandler
        {
            get
            {
                HttpClientHandler _handler = new HttpClientHandler();
                _handler.ServerCertificateCustomValidationCallback += (_, __, ___, ____) => true;
                return _handler;
            }
        }
        public App()
        {
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
            DependencyService.Register<ParkingParkingPlaceDataStore>();

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

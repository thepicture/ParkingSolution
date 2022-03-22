using ParkingSolution.XamarinApp.Services;
using Xamarin.Forms;

namespace ParkingSolution.XamarinApp
{
    public partial class App : Application
    {
        public string Role { get; set; }
        public string Identity { get; set; }
        public string BaseUrl { get; } = "https://parkingsolution-webapi.conveyor.cloud/api/";

        public App()
        {
            InitializeComponent();
            XF.Material.Forms.Material.Init(this);

            DependencyService.Register<AndroidFeedbackService>();
            DependencyService.Register<ApiAuthenticatorService>();

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

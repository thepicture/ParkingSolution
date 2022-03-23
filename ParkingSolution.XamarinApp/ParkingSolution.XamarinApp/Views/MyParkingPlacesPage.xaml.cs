using ParkingSolution.XamarinApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ParkingSolution.XamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyParkingPlacesPage : ContentPage
    {
        private readonly MyParkingPlacesViewModel _viewModel;

        public MyParkingPlacesPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new MyParkingPlacesViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}
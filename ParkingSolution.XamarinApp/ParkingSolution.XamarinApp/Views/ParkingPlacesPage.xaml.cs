using ParkingSolution.XamarinApp.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ParkingSolution.XamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ParkingPlacesPage : ContentPage
    {
        private readonly ParkingPlacesViewModel _viewModel;

        public ParkingPlacesPage()
        {
            InitializeComponent();
        }

        public ParkingPlacesPage(ParkingPlacesViewModel parkingPlacesViewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = parkingPlacesViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}
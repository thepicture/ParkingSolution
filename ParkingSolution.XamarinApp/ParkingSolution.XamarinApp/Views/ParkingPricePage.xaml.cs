using ParkingSolution.XamarinApp.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ParkingSolution.XamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ParkingPricePage : ContentPage
    {
        private readonly ParkingPriceViewModel _viewModel;

        public ParkingPricePage()
        {
            InitializeComponent();
        }

        public ParkingPricePage(ParkingPriceViewModel parkingPriceViewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = parkingPriceViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}
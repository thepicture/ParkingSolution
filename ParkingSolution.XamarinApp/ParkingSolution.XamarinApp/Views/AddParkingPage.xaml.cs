using ParkingSolution.XamarinApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ParkingSolution.XamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddParkingPage : ContentPage
    {
        private readonly AddParkingViewModel _viewModel;

        public AddParkingPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new AddParkingViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}
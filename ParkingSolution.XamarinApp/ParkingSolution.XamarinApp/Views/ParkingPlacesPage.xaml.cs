using ParkingSolution.XamarinApp.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ParkingSolution.XamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ParkingPlacesPage : ContentPage
    {
        public ParkingPlacesPage()
        {
            InitializeComponent();
        }

        public ParkingPlacesPage(ParkingPlacesViewModel parkingPlacesViewModel)
        {
            InitializeComponent();
            BindingContext = parkingPlacesViewModel;
        }
    }
}
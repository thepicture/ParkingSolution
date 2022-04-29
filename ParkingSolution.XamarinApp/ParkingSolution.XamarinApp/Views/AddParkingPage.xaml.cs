using ParkingSolution.XamarinApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ParkingSolution.XamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddParkingPage : ContentPage
    {
        public AddParkingPage()
        {
            InitializeComponent();
            BindingContext = new AddParkingViewModel();
        }

        public AddParkingPage(AddParkingViewModel addParkingViewModel)
        {
            InitializeComponent();
            BindingContext = addParkingViewModel;
        }
    }
}
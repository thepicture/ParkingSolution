using ParkingSolution.XamarinApp.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ParkingSolution.XamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentPage : ContentPage
    {

        public PaymentPage()
        {
            InitializeComponent();
        }

        public PaymentPage(PaymentViewModel paymentViewModel)
        {
            InitializeComponent();
            BindingContext = paymentViewModel;
        }
    }
}
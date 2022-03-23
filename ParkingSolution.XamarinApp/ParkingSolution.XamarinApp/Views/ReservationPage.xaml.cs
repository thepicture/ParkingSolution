using ParkingSolution.XamarinApp.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI;

namespace ParkingSolution.XamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReservationPage : ContentPage
    {
        private readonly ReservationViewModel _viewModel;

        public ReservationPage()
        {
            InitializeComponent();
        }

        public ReservationPage(ReservationViewModel reservationViewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = reservationViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        private void OnSelectedChanged(object sender, SelectedChangedEventArgs e)
        {
            CommonLayout.ResolveLayoutChanges();
        }
    }
}
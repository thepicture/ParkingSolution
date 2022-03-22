using ParkingSolution.XamarinApp.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ParkingSolution.XamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyCarsPage : ContentPage
    {
        private readonly MyCarsViewModel _viewModel;

        public MyCarsPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new MyCarsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}
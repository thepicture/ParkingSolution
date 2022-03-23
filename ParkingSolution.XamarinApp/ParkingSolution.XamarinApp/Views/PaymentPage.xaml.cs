using ParkingSolution.XamarinApp.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ParkingSolution.XamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentPage : ContentPage
    {
        private readonly PaymentViewModel _viewModel;

        public PaymentPage()
        {
            InitializeComponent();
        }

        public PaymentPage(PaymentViewModel paymentViewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = paymentViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        private void OnRefreshing(object sender, EventArgs e)
        {
            if (!_viewModel.IsBusy)
            {
                (sender as RefreshView).IsRefreshing = false;
            }
        }
    }
}
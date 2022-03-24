using ParkingSolution.XamarinApp.Models.Helpers;
using ParkingSolution.XamarinApp.ViewModels;
using Plugin.Geolocator;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace ParkingSolution.XamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ParkingsPage : ContentPage
    {
        private readonly ParkingsViewModel _viewModel;
        public ParkingsPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new ParkingsViewModel();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                Plugin.Geolocator.Abstractions.Position position =
                    await CrossGeolocator.Current.GetPositionAsync();
                ParkingsMap.MoveToRegion(
                    MapSpan.FromCenterAndRadius(
                        new Position(position.Latitude, position.Longitude),
                        Distance.FromKilometers(1)));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
            }
            _viewModel.OnAppearing();
        }

        private void OnPinClicked(object sender, PinClickedEventArgs e)
        {
            (BindingContext as ParkingsViewModel)
            .SelectedParking = (sender as Pin)
            .BindingContext as ParkingHelper;
        }

        private void OnListItemClicked(object sender, EventArgs e)
        {
            (BindingContext as ParkingsViewModel)
          .SelectedParking = (sender as View)
          .BindingContext as ParkingHelper;
        }
    }
}
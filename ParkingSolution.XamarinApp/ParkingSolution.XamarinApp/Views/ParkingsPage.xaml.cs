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
            _viewModel.OnAppearing();
            try
            {
                Plugin.Geolocator.Abstractions.Position position =
                    await CrossGeolocator.Current.GetPositionAsync();
                ParkingsMap.MoveToRegion(
                    MapSpan.FromCenterAndRadius(
                        new Position(
                            Math.Round(position.Latitude, 4), Math.Round(position.Longitude, 4)),
                        Distance.FromKilometers(1)));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private void OnPinClicked(object sender, PinClickedEventArgs e)
        {
            _viewModel.SelectedParking = (sender as Pin)
                .BindingContext as ParkingHelper;
        }

        private void OnListItemClicked(object sender, EventArgs e)
        {
            _viewModel.SelectedParking = (sender as View)
                .BindingContext as ParkingHelper;
        }

        private void OnParkingEditClick(object sender, EventArgs e)
        {
            ParkingHelper parkingHelper = (sender as View)
                .BindingContext as ParkingHelper;
            _viewModel.GoToParkingEditCommand.Execute(parkingHelper.Parking);
        }
    }
}
using ParkingSolution.XamarinApp.Models.Helpers;
using ParkingSolution.XamarinApp.Models.Serialized;
using ParkingSolution.XamarinApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace ParkingSolution.XamarinApp.ViewModels
{
    public class ParkingsViewModel : BaseViewModel
    {
        private bool showAsMap = true;
        internal void OnAppearing()
        {
            IsRefreshing = true;
        }

        private async void LoadParkingsAsync()
        {
            SelectedParking = null;
            Parkings.Clear();
            IEnumerable<SerializedParking> parkings =
                await ParkingDataStore.GetItemsAsync();
            try
            {
                Geocoder geoCoder = new Geocoder();
                foreach (SerializedParking parking in parkings)
                {
                    IEnumerable<Position> approximateLocations =
                        await geoCoder
                        .GetPositionsForAddressAsync(
                            string.Format(parking.Address));
                    Position position = approximateLocations
                        .FirstOrDefault();
                    Parkings.Add(new ParkingHelper
                    {
                        Address = parking.Address,
                        Description = parking.ParkingType,
                        Position = position,
                        Parking = parking
                    });
                }
            }
            catch (Exception ex)
            {
                await FeedbackService
                    .InformError(ex);
            }
            IsRefreshing = false;
        }

        public ObservableCollection<ParkingHelper> Parkings
        {
            get => parkings;
            set => SetProperty(ref parkings, value);
        }

        private ParkingHelper selectedParking;
        private ObservableCollection<ParkingHelper> parkings;

        public ParkingHelper SelectedParking
        {
            get => selectedParking;
            set => SetProperty(ref selectedParking, value);
        }

        private Command goToParkingPlacesCommand;

        public ICommand GoToParkingPlacesCommand
        {
            get
            {
                if (goToParkingPlacesCommand == null)
                {
                    goToParkingPlacesCommand = new Command(GoToParkingPlacesAsync);
                }

                return goToParkingPlacesCommand;
            }
        }

        public bool IsShowAsMap
        {
            get => showAsMap;
            set => SetProperty(ref showAsMap, value);
        }

        private async void GoToParkingPlacesAsync(object param)
        {
            if (param != null)
            {
                SelectedParking = param as ParkingHelper;
            }
            await Shell
              .Current
              .Navigation
              .PushAsync(
                  new ParkingPlacesPage(
                      new ParkingPlacesViewModel(
                          SelectedParking.Parking)
                      )
                  );
        }

        private Command toggleViewTypeCommand;

        public ICommand ToggleViewTypeCommand
        {
            get
            {
                if (toggleViewTypeCommand == null)
                {
                    toggleViewTypeCommand = new Command(ToggleViewType);
                }

                return toggleViewTypeCommand;
            }
        }

        private void ToggleViewType()
        {
            IsShowAsMap = !IsShowAsMap;
        }

        private Command goToParkingPriceCommand;

        public ICommand GoToParkingPriceCommand
        {
            get
            {
                if (goToParkingPriceCommand == null)
                {
                    goToParkingPriceCommand = new Command(GoToParkingPriceAsync);
                }

                return goToParkingPriceCommand;
            }
        }

        private async void GoToParkingPriceAsync(object param)
        {
            if (param != null)
            {
                SelectedParking = param as ParkingHelper;
            }
            await Shell
           .Current
           .Navigation
           .PushAsync(
               new ParkingPricePage(
                   new ParkingPriceViewModel(
                       SelectedParking.Parking)
                   )
               );
        }

        private Command<ParkingHelper> deleteParkingCommand;

        public Command<ParkingHelper> DeleteParkingCommand
        {
            get
            {
                if (deleteParkingCommand == null)
                {
                    deleteParkingCommand = new Command<ParkingHelper>
                        (DeleteParkingAsync);
                }

                return deleteParkingCommand;
            }
        }

        private async void DeleteParkingAsync(ParkingHelper parking)
        {
            if (await ParkingDataStore
                .DeleteItemAsync(parking.Parking.Id
                .ToString()))
            {
                IsRefreshing = true;
            }
        }

        private Command refreshCommand;

        public ParkingsViewModel()
        {
            Parkings = new ObservableCollection<ParkingHelper>();
        }

        public ICommand RefreshCommand
        {
            get
            {
                if (refreshCommand == null)
                {
                    refreshCommand = new Command(Refresh);
                }

                return refreshCommand;
            }
        }

        private void Refresh()
        {
            LoadParkingsAsync();
        }

        private Command goToAddParkingPageCommand;

        public ICommand GoToAddParkingPageCommand
        {
            get
            {
                if (goToAddParkingPageCommand == null)
                {
                    goToAddParkingPageCommand = new Command(GoToAddParkingPageAsync);
                }

                return goToAddParkingPageCommand;
            }
        }

        private async void GoToAddParkingPageAsync()
        {
            await Shell
              .Current
              .GoToAsync(
                  $"{nameof(AddParkingPage)}");
        }

        private Command<SerializedParking> goToParkingEditCommand;

        public Command<SerializedParking> GoToParkingEditCommand
        {
            get
            {
                if (goToParkingEditCommand == null)
                {
                    goToParkingEditCommand = new Command<SerializedParking>
                        (GoToParkingEditAsync);
                }

                return goToParkingEditCommand;
            }
        }

        private async void GoToParkingEditAsync(SerializedParking serializedParking)
        {
            await Shell
                .Current
                .Navigation
                .PushAsync(
                    new AddParkingPage(
                        new AddParkingViewModel(serializedParking)
                        )
                    );
        }
    }
}
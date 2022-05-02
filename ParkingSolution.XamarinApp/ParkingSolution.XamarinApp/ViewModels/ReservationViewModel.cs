using ParkingSolution.XamarinApp.Models.Serialized;
using ParkingSolution.XamarinApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace ParkingSolution.XamarinApp.ViewModels
{
    public class ReservationViewModel : BaseViewModel
    {
        private SerializedParkingPlace parkingPlace;
        private decimal costInRubles;

        public ReservationViewModel
            (SerializedParkingPlace serializedParkingPlace)
        {
            Cars = new ObservableCollection<SerializedUserCar>();
            ParkingPlace = serializedParkingPlace;
            LoadParkingAsync();
        }

        private async void LoadParkingAsync()
        {
            Parking = await ParkingDataStore
                .GetItemAsync(
                    ParkingPlace.ParkingId.ToString());
        }

        public SerializedParkingPlace ParkingPlace
        {
            get => parkingPlace;
            set => SetProperty(ref parkingPlace, value);
        }

        internal void OnAppearing()
        {
            LoadCarsAsync();
        }

        private async void LoadCarsAsync()
        {
            Cars.Clear();
            IEnumerable<SerializedUserCar> userCarsResponse =
                await CarDataStore
                .GetItemsAsync();
            userCarsResponse = userCarsResponse
                .Where(c => c.CarType == ParkingPlace.CarType);
            foreach (SerializedUserCar car in userCarsResponse)
            {
                Cars.Add(car);
            }
        }

        private ObservableCollection<SerializedUserCar> cars;

        public ObservableCollection<SerializedUserCar> Cars
        {
            get => cars;
            set => SetProperty(ref cars, value);
        }

        private SerializedUserCar selectedCar;

        public SerializedUserCar SelectedCar
        {
            get => selectedCar;
            set => SetProperty(ref selectedCar, value);
        }

        private Command saveChangesCommand;

        public ICommand SaveChangesCommand
        {
            get
            {
                if (saveChangesCommand == null)
                {
                    saveChangesCommand = new Command(SaveChangesAsync);
                }

                return saveChangesCommand;
            }
        }

        private async void SaveChangesAsync()
        {
            IsBusy = true;

            SerializedParkingPlaceReservation reservation =
                new SerializedParkingPlaceReservation
                {
                    FromDateTime = FromDateTime,
                    LocalToDateTime = ToDateTime,
                    CarId = SelectedCar?.Id ?? 0,
                    ParkingPlaceId = ParkingPlace.Id,
                    IsKnownToDate = IsKnownToDate
                };

            if (await ReservationDataStore.AddItemAsync(reservation))
            {
                await Shell.Current.GoToAsync($"../..");
            }

            IsBusy = false;
        }

        private bool isKnownToDate;
        private bool isValidDateTimeSpanForCalculating;

        private void InvalidateCalculatedPrice()
        {
            if (Parking != null && (FromDateTime < ToDateTime))
            {
                IsValidDateTimeSpanForCalculating = true;
                CalculatedCostInRubles = 0;
                for (DateTime dateTime = FromDateTime;
                    dateTime < ToDateTime;
                    dateTime = dateTime.AddHours(1))
                {
                    if (dateTime.TimeOfDay > Parking.BeforePaidTime
                        && dateTime.TimeOfDay < Parking.BeforeFreeTime)
                    {
                        CalculatedCostInRubles += Parking.CostInRubles;
                    }
                }
            }
            else
            {
                IsValidDateTimeSpanForCalculating = false;
            }
        }

        public bool IsKnownToDate
        {
            get => isKnownToDate;
            set
            {
                SetProperty(ref isKnownToDate, value);
                InvalidateCalculatedPrice();
            }
        }

        private Command goToAddCarPage;

        public ICommand GoToAddCarPage
        {
            get
            {
                if (goToAddCarPage == null)
                {
                    goToAddCarPage = new Command(PerformGoToAddCarPageAsync);
                }

                return goToAddCarPage;
            }
        }

        private async void PerformGoToAddCarPageAsync()
        {
            await Shell
           .Current
           .Navigation
           .PushAsync(new AddCarPage());
        }

        private SerializedParking parking;

        public SerializedParking Parking
        {
            get => parking;
            set => SetProperty(ref parking, value);
        }
        public decimal CostInRubles
        {
            get => costInRubles;
            set => SetProperty(ref costInRubles, value);
        }

        private decimal calculatedCostInRubles;

        public decimal CalculatedCostInRubles
        {
            get => calculatedCostInRubles;
            set => SetProperty(ref calculatedCostInRubles, value);
        }

        private DateTime fromDateTime = DateTime.Now.AddMinutes(10);

        public DateTime FromDateTime
        {
            get => fromDateTime;
            set
            {
                if (SetProperty(ref fromDateTime, value))
                {
                    InvalidateCalculatedPrice();
                }
            }
        }

        private DateTime toDateTime = DateTime.Now.AddHours(1);

        public DateTime ToDateTime
        {
            get => toDateTime;
            set
            {
                if (SetProperty(ref toDateTime, value))
                {
                    InvalidateCalculatedPrice();
                }
            }
        }

        public bool IsValidDateTimeSpanForCalculating
        {
            get => isValidDateTimeSpanForCalculating;
            set => SetProperty(ref isValidDateTimeSpanForCalculating, value);
        }
    }
}
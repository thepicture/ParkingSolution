using Newtonsoft.Json;
using ParkingSolution.XamarinApp.Models.Serialized;
using ParkingSolution.XamarinApp.Services;
using ParkingSolution.XamarinApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
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
            Task.Run(() =>
            {
                LoadCarsAsync();
            });
        }

        private async void LoadCarsAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                  new AuthenticationHeaderValue("Basic",
                                                AppIdentity.AuthorizationValue);
                client.BaseAddress = new Uri((App.Current as App).BaseUrl);
                try
                {
                    string response = await client
                        .GetAsync($"usercars")
                        .Result
                        .Content
                        .ReadAsStringAsync();
                    IEnumerable<SerializedUserCar> userCarsResponse =
                        JsonConvert.DeserializeObject
                        <IEnumerable<SerializedUserCar>>
                        (response)
                        .Where(c => c.CarType == ParkingPlace.CarType);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Cars.Clear();
                        foreach (SerializedUserCar car in userCarsResponse)
                        {
                            Cars.Add(car);
                        }
                    });
                }
                catch (HttpRequestException ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    await FeedbackService.Inform("Автомобили не подгружены. "
                        + "Перезайдите на страницу");
                }
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
            StringBuilder validationErrors = new StringBuilder();

            if (SelectedCar == null)
            {
                _ = validationErrors.AppendLine("Укажите автомобиль");
            }
            if (FromDateTime < DateTime.Now)
            {
                _ = validationErrors.AppendLine("Парковка должна быть " +
                    "позднее текущей даты и времени");
            }
            if (IsKnownToDate && (FromDateTime >= ToDateTime))
            {
                _ = validationErrors.AppendLine("Дата начала " +
                    "должна быть раньше даты окончания");
            }

            if (validationErrors.Length > 0)
            {
                await FeedbackService.InformError(
                    validationErrors.ToString());
                return;
            }

            IEnumerable<SerializedParkingPlaceReservation> conflictPlaces =
                ParkingPlace.Reservations
                .Where(pp =>
                {
                    return (pp.ToDateTime == null
                    && pp.FromDateTime > FromDateTime)
                    || (pp.FromDateTime < FromDateTime
                    || pp.ToDateTime < ToDateTime);
                });
            if (conflictPlaces
                .Count() > 0)
            {
                await FeedbackService.InformError($"В назначенное время " +
                    "уже есть бронировки. Попробуйте изменить дату начала " +
                    $"начиная с {conflictPlaces.OrderBy(c => c.FromDateTime).Last().FromDateTime}");
                return;
            }

            SerializedParkingPlaceReservation reservation =
                new SerializedParkingPlaceReservation
                {
                    FromDateTime = FromDateTime,
                    CarId = SelectedCar.Id,
                    ParkingPlaceId = ParkingPlace.Id
                };
            if (IsKnownToDate)
            {
                reservation.ToDateTime = ToDateTime;
            }

            if (await ReservationDataStore.AddItemAsync(reservation))
            {
                await FeedbackService.Inform("Парковочное место забронировано");
                await Shell.Current.GoToAsync($"../..");
            }
            else
            {
                await FeedbackService.InformError("В указанное время " +
                    "парковочное место уже забронировано. " +
                    "Измените интервал времени на более поздний");
            }
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

        private DateTime fromDateTime = DateTime.Now.AddHours(1);

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

        private DateTime toDateTime = DateTime.Now.AddHours(2);

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
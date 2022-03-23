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
            if (FromDate + FromTime < DateTime.Now)
            {
                _ = validationErrors.AppendLine("Парковка должна быть " +
                    "позднее текущей даты и времени");
            }
            if (IsKnownToDate && (FromDate + FromTime >= ToDate + ToTime))
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

            SerializedParkingPlaceReservation reservation =
                new SerializedParkingPlaceReservation
                {
                    FromDateTime = FromDate + FromTime,
                    CarId = SelectedCar.Id,
                    ParkingPlaceId = ParkingPlace.Id
                };
            if (IsKnownToDate)
            {
                reservation.ToDateTime = ToDate + ToTime;
            }

            if (await ReservationDataStore.AddItemAsync(reservation))
            {
                await FeedbackService.Inform("Парковочное место забронировано");
                await Shell.Current.GoToAsync($"../..");
            }
            else
            {
                await FeedbackService.Inform("Не удалось " +
                    "забронировать парковочное место. " +
                    "Проверьте подключение к интернету");
            }
        }

        private DateTime fromDate = DateTime.Now.Date;

        public DateTime FromDate
        {
            get => fromDate;
            set => SetProperty(ref fromDate, value);
        }

        private TimeSpan fromTime = DateTime.Now.TimeOfDay;

        public TimeSpan FromTime
        {
            get => fromTime;
            set => SetProperty(ref fromTime, value);
        }

        private DateTime toDate = DateTime.Now.Date;

        public DateTime ToDate
        {
            get => toDate;
            set => SetProperty(ref toDate, value);
        }

        private TimeSpan toTime = DateTime.Now.TimeOfDay;
        private bool isKnownToDate;

        public TimeSpan ToTime
        {
            get => toTime;
            set => SetProperty(ref toTime, value);
        }
        public bool IsKnownToDate
        {
            get => isKnownToDate;
            set => SetProperty(ref isKnownToDate, value);
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
    }
}
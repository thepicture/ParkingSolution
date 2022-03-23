using Microcharts;
using Newtonsoft.Json;
using ParkingSolution.XamarinApp.Models.Helpers;
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
using System.Threading.Tasks;
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
            Parkings = new ObservableCollection<ParkingHelper>();
            Task.Run(() =>
            {
                LoadParkingsAsync();
            });
        }

        private async void LoadParkingsAsync()
        {
            IsBusy = true;

            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(10);
                client.DefaultRequestHeaders.Authorization =
                  new AuthenticationHeaderValue("Basic",
                                                AppIdentity.AuthorizationValue);
                client.BaseAddress = new Uri((App.Current as App).BaseUrl);
                try
                {
                    string response = await client
                        .GetAsync("parkings")
                        .Result
                        .Content
                        .ReadAsStringAsync();
                    IEnumerable<SerializedParking> parkings = JsonConvert
                        .DeserializeObject
                        <IEnumerable<SerializedParking>>
                        (response);
                    Geocoder geoCoder = new Geocoder();
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Parkings.Clear();
                    });
                    foreach (SerializedParking parking in parkings)
                    {
                        IEnumerable<Position> approximateLocations =
                                await geoCoder
                                .GetPositionsForAddressAsync(
                                string.Format(parking.Address)
                                );
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Position position = approximateLocations
                                .FirstOrDefault();
                            Parkings.Add(new ParkingHelper
                            {
                                Address = parking.Address,
                                Description = parking.ParkingType,
                                Position = position,
                                Parking = parking
                            });
                        });
                    }
                }
                catch (HttpRequestException ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    await FeedbackService.Inform("Парковки " +
                        "не загружены в связи с изменением " +
                        "бизнес-процессов. Обратитесь к " +
                        "администратору");
                }
                catch (TimeoutException ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    await FeedbackService.Inform("Время ожидания " +
                        "загрузки истекло. Обновите страницу");
                }
            }
            IsBusy = false;
        }

        internal void OnRefreshing()
        {
            Task.Run(() =>
            {
                LoadParkingsAsync();
            });
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
    }
}
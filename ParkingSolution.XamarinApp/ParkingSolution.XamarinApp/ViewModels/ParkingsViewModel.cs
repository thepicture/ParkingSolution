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
            SelectedParking = null;
            IsRefreshing = true;
        }

        private async void LoadParkingsAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                  new AuthenticationHeaderValue("Basic",
                                                AppIdentity.AuthorizationValue);
                client.BaseAddress = new Uri((App.Current as App).BaseUrl);
                try
                {
                    HttpResponseMessage response = await client
                        .GetAsync("parkings");
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            FeedbackService.InformError("Парковки не "
                                + "подгружены в связи с ошибкой сервера: "
                                + response.StatusCode);
                        });
                        IsRefreshing = false;
                        return;
                    }
                    IEnumerable<SerializedParking> parkings = JsonConvert
                        .DeserializeObject
                        <IEnumerable<SerializedParking>>
                        (
                            await response.Content.ReadAsStringAsync()
                        );
                    Geocoder geoCoder = new Geocoder();
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Parkings.Clear();
                    });
                    foreach (SerializedParking parking in parkings)
                    {
                        try
                        {
                            IEnumerable<Position> approximateLocations =
                                                           await geoCoder
                                                           .GetPositionsForAddressAsync(
                                                           string.Format(parking.Address)
                                                           );
                            Position position = approximateLocations
                                .FirstOrDefault();
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                Parkings.Add(new ParkingHelper
                                {
                                    Address = parking.Address,
                                    Description = parking.ParkingType,
                                    Position = position,
                                    Parking = parking
                                });
                            });
                        }
                        catch (Exception ex)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                FeedbackService.InformError("Геолокация " +
                                    "произошла с ошибкой: " + ex.StackTrace);
                            });
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        FeedbackService.InformError("Парковки " +
                        "не загружены в связи с изменением " +
                        "бизнес-процессов. Обратитесь к " +
                        "администратору");
                    });
                    IsRefreshing = false;
                }
                catch (TaskCanceledException ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        FeedbackService.InformError("Запрос на " +
                        "получение парковок отменён");
                    });
                    IsRefreshing = false;
                }
                catch (InvalidOperationException ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        FeedbackService.InformError("Система " +
                         "попыталась отправить запрос " +
                         "более, чем один раз");
                    });
                    IsRefreshing = false;
                }
                catch (TimeoutException ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        FeedbackService.InformError("Время ожидания " +
                        "загрузки истекло. Обновите страницу");
                    });
                    IsRefreshing = false;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        FeedbackService.InformError("Неизвестная ошибка. " +
                        "Парковки не загружены. " +
                        "Перезапустите приложение или обновите " +
                        "список парковок");
                    });
                    IsRefreshing = false;
                }
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
            if (!await FeedbackService.Ask("Удалить парковку зоны " +
                $"{parking.Parking.Id}?"))
            {
                return;
            }
            if (await ParkingDataStore.DeleteItemAsync(parking.Parking.Id
                .ToString()))
            {
                await FeedbackService.Inform("Парковка удалена");
                IsRefreshing = true;
            }
            else
            {
                await FeedbackService.InformError("Парковка не удалена. " +
                    "Перезайдите на страницу");
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
            try
            {
                Task.Run(() =>
                {
                    LoadParkingsAsync();
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
            }
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
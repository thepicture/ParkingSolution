using Newtonsoft.Json;
using ParkingSolution.XamarinApp.Models.Serialized;
using ParkingSolution.XamarinApp.Services;
using ParkingSolution.XamarinApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ParkingSolution.XamarinApp.ViewModels
{
    public class MyParkingPlacesViewModel : BaseViewModel
    {
        internal void OnAppearing()
        {
            IsRefreshing = true;
        }

        private async void LoadMyParkingPlacesAsync()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                MyReservations.Clear();
            });

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                  new AuthenticationHeaderValue("Basic",
                                                AppIdentity.AuthorizationValue);
                client.BaseAddress = new Uri((App.Current as App).BaseUrl);
                try
                {
                    string response = await client
                        .GetAsync($"users/myparkingplaces")
                        .Result
                        .Content
                        .ReadAsStringAsync();
                    IEnumerable<SerializedParkingPlaceReservation> reservationsResponse =
                        JsonConvert.DeserializeObject
                        <IEnumerable<SerializedParkingPlaceReservation>>
                        (response);
                    foreach (SerializedParkingPlaceReservation reservation
                        in reservationsResponse)
                    {
                        await Task.Delay(500);
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            MyReservations.Add(reservation);
                        });
                    }
                }
                catch (HttpRequestException ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    await FeedbackService.Inform(
                        "Неоплаченные парковки не подгружены. " +
                        "Перезайдите на страницу");
                }
            }
            Device.BeginInvokeOnMainThread(() =>
            {
                IsRefreshing = false;
            });
        }

        private ObservableCollection<SerializedParkingPlaceReservation> myReservatins;

        public ObservableCollection<SerializedParkingPlaceReservation> MyReservations
        {
            get => myReservatins;
            set => SetProperty(ref myReservatins, value);
        }

        private Command<SerializedParkingPlaceReservation> payParkingPlaceCommand;

        public Command<SerializedParkingPlaceReservation> PayParkingPlaceCommand
        {
            get
            {
                if (payParkingPlaceCommand == null)
                {
                    payParkingPlaceCommand =
                        new Command<SerializedParkingPlaceReservation>
                        (PayParkingPlaceAsync);
                }

                return payParkingPlaceCommand;
            }
        }

        private async void PayParkingPlaceAsync
            (SerializedParkingPlaceReservation reservation)
        {
            await Shell
            .Current
            .Navigation
            .PushAsync(
                new PaymentPage(
                    new PaymentViewModel(reservation)
                    )
                );
        }

        private Command refreshCommand;

        public MyParkingPlacesViewModel()
        {
            MyReservations = new ObservableCollection<SerializedParkingPlaceReservation>();
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
            Task.Run(() =>
            {
                LoadMyParkingPlacesAsync();
            });
        }
    }
}
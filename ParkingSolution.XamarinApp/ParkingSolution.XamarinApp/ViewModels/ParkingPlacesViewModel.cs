﻿using Newtonsoft.Json;
using ParkingSolution.XamarinApp.Models.Serialized;
using ParkingSolution.XamarinApp.Services;
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
    public class ParkingPlacesViewModel : BaseViewModel
    {
        internal void OnAppearing()
        {

        }

        private async void LoadParkingPlacesAsync()
        {
            if (ParkingPlaces.Count != 0)
            {
                ParkingPlaces.Clear();
            }

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                  new AuthenticationHeaderValue("Basic",
                                                AppIdentity.AuthorizationValue);
                client.BaseAddress = new Uri((App.Current as App).BaseUrl);
                try
                {
                    string response = await client
                        .GetAsync($"parkingplaces?parkingId={Parking.Id}")
                        .Result
                        .Content
                        .ReadAsStringAsync();
                    IEnumerable<SerializedParkingPlace> parkingPlacesResponse =
                        JsonConvert.DeserializeObject
                        <IEnumerable<SerializedParkingPlace>>
                        (response);
                    foreach (SerializedParkingPlace parkingPlace
                        in parkingPlacesResponse)
                    {
                        await Task.Delay(500);
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            ParkingPlaces.Add(parkingPlace);
                        });
                    }
                }
                catch (HttpRequestException ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    await FeedbackService.Inform("Парковки не подгружены. " +
                        "Перезайдите на страницу");
                }
            }
        }

        private ObservableCollection<SerializedParkingPlace> parkingPlaces;

        public SerializedParking Parking { get; set; }
        public ObservableCollection<SerializedParkingPlace> ParkingPlaces
        {
            get => parkingPlaces;
            set => SetProperty(ref parkingPlaces, value);
        }

        private Command reserveParkingPlaceCommand;

        public ParkingPlacesViewModel(SerializedParking serializedParking)
        {
            Parking = serializedParking;
            ParkingPlaces = new ObservableCollection<SerializedParkingPlace>();
            Task.Run(() =>
            {
                LoadParkingPlacesAsync();
            });
        }

        public ICommand ReserveParkingPlaceCommand
        {
            get
            {
                if (reserveParkingPlaceCommand == null)
                {
                    reserveParkingPlaceCommand = new Command(ReserveParkingPlaceAsync);
                }

                return reserveParkingPlaceCommand;
            }
        }

        private async void ReserveParkingPlaceAsync(object param)
        {

        }
    }
}
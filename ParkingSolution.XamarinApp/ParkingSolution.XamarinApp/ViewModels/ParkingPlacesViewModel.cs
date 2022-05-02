using AsyncCommands;
using ParkingSolution.XamarinApp.Models.Serialized;
using ParkingSolution.XamarinApp.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ParkingSolution.XamarinApp.ViewModels
{
    public class ParkingPlacesViewModel : BaseViewModel
    {
        private async void LoadParkingPlacesAsync()
        {
            ParkingPlaces.Clear();
            IEnumerable<SerializedParkingPlace> parkingPlacesResponse =
                await ParkingParkingPlaceDataStore.GetItemAsync(
                    Parking.Id.ToString());
            foreach (SerializedParkingPlace parkingPlace in parkingPlacesResponse)
            {
                await Task.Delay(500);
                ParkingPlaces.Add(parkingPlace);
            }
        }

        private ObservableCollection<SerializedParkingPlace> parkingPlaces;

        public SerializedParking Parking { get; set; }
        public ObservableCollection<SerializedParkingPlace> ParkingPlaces
        {
            get => parkingPlaces;
            set => SetProperty(ref parkingPlaces, value);
        }

        private AsyncCommand<SerializedParkingPlace> reserveParkingPlaceCommand;

        public ParkingPlacesViewModel(SerializedParking serializedParking)
        {
            ParkingPlaces = new ObservableCollection<SerializedParkingPlace>();
            Parking = serializedParking;
            LoadParkingPlacesAsync();
        }

        public AsyncCommand<SerializedParkingPlace> ReserveParkingPlaceCommand
        {
            get
            {
                if (reserveParkingPlaceCommand == null)
                {
                    reserveParkingPlaceCommand = new AsyncCommand<SerializedParkingPlace>
                        (ReserveParkingPlaceAsync,
                        CanReserveParkingPlaceExecute);
                }

                return reserveParkingPlaceCommand;
            }
        }

        private bool CanReserveParkingPlaceExecute(SerializedParkingPlace arg)
        {
            return arg.IsFree;
        }

        private async Task ReserveParkingPlaceAsync(object param)
        {
            await Shell
              .Current
              .Navigation
              .PushAsync(
                  new ReservationPage(
                      new ReservationViewModel(
                          param as SerializedParkingPlace)
                      )
                  );
        }
    }
}
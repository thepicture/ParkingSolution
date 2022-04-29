using ParkingSolution.XamarinApp.Models.Serialized;
using ParkingSolution.XamarinApp.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            MyReservations.Clear();

            IEnumerable<SerializedParkingPlaceReservation> reservationsResponse =
                await ReservationDataStore.GetItemsAsync();
            foreach (SerializedParkingPlaceReservation reservation
                in reservationsResponse)
            {
                await Task.Delay(500);
                MyReservations.Add(reservation);
            }
            IsRefreshing = false;
        }

        private ObservableCollection<SerializedParkingPlaceReservation> myReservations;

        public ObservableCollection<SerializedParkingPlaceReservation> MyReservations
        {
            get => myReservations;
            set => SetProperty(ref myReservations, value);
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
            LoadMyParkingPlacesAsync();
        }
    }
}
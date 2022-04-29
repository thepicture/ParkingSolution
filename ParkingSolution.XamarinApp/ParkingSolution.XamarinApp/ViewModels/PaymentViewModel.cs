using ParkingSolution.XamarinApp.Models.Serialized;
using System.Windows.Input;
using Xamarin.Forms;

namespace ParkingSolution.XamarinApp.ViewModels
{
    public class PaymentViewModel : BaseViewModel
    {
        private Command payCommand;

        public ICommand PayCommand
        {
            get
            {
                if (payCommand == null)
                {
                    payCommand = new Command(PayAsync);
                }

                return payCommand;
            }
        }

        private async void PayAsync()
        {
            IsBusy = true;
            SerializedPaymentHistory history = new SerializedPaymentHistory
            {
                Sum = Reservation.TotalPrice,
                ReservationId = Reservation.Id,
                CardNumber = CardNumber
            };
            if (await PaymentHistoryDataStore.AddItemAsync(history))
            {
                await Shell.Current.GoToAsync($"..");
            }
            IsBusy = false;
        }

        private string cardNumber;
        private SerializedParkingPlaceReservation reservation;

        public PaymentViewModel(SerializedParkingPlaceReservation reservation)
        {
            Reservation = reservation;
        }

        public string CardNumber
        {
            get => cardNumber;
            set => SetProperty(ref cardNumber, value);
        }
        public SerializedParkingPlaceReservation Reservation
        {
            get => reservation;
            set => SetProperty(ref reservation, value);
        }

        private Command refreshCommand;

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
            if (IsNotBusy && IsRefreshing)
            {
                IsRefreshing = false;
            }
        }
    }
}
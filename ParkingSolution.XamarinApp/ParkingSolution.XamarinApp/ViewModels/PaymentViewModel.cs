using ParkingSolution.XamarinApp.Models.Serialized;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ParkingSolution.XamarinApp.ViewModels
{
    public class PaymentViewModel : BaseViewModel
    {
        internal void OnAppearing()
        {

        }

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

            StringBuilder validationErrors = new StringBuilder();
            if (CardNumber == null || CardNumber.Length != 27)
            {
                _ = validationErrors.AppendLine("Укажите " +
                    "корректный номер карты");
            }

            if (validationErrors.Length > 0)
            {
                await FeedbackService.InformError(validationErrors);
                return;
            }

            IsBusy = true;

            SerializedPaymentHistory history =
                new SerializedPaymentHistory
                {
                    Sum = Reservation.TotalPrice,
                    ReservationId = Reservation.Id,
                    CardNumber = CardNumber
                        .Replace("(", "")
                        .Replace(")", "")
                        .Replace("-", ""),
                };

            if (await PaymentHistoryDataStore.AddItemAsync(history))
            {
                await FeedbackService.Inform("Платёж успешен");
                await Shell.Current.GoToAsync($"..");
            }
            else
            {
                await FeedbackService.InformError("При транзакции " +
                    "произошла ошибка. Попробуйте ещё раз");
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
    }
}
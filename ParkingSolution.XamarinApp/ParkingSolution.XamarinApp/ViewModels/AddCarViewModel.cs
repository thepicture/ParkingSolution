using ParkingSolution.XamarinApp.Models.Serialized;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ParkingSolution.XamarinApp.ViewModels
{
    public class AddCarViewModel : BaseViewModel
    {
        internal void OnAppearing()
        {
            CurrentCar = new SerializedUserCar();
        }

        private string selectedCarType;
        private SerializedUserCar currentCar;

        public string SelectedCarType
        {
            get => selectedCarType;
            set => SetProperty(ref selectedCarType, value);
        }
        public SerializedUserCar CurrentCar
        {
            get => currentCar;
            set => SetProperty(ref currentCar, value);
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
            if (SelectedCarType == null)
            {
                _ = validationErrors.AppendLine("Выберите тип " +
                    "автомобиля");
            }
            if (string.IsNullOrWhiteSpace(SeriesPartOne))
            {
                _ = validationErrors.AppendLine("Введите первую часть " +
                    "серии");
            }
            if (string.IsNullOrWhiteSpace(RegistrationCode)
                || !int.TryParse(RegistrationCode, out _)
                || int.Parse(RegistrationCode) <= 0)
            {
                _ = validationErrors.AppendLine("Введите " +
                    "код регистрации в формате <nnn> (например, 002)");
            }
            if (string.IsNullOrWhiteSpace(SeriesPartTwo))
            {
                _ = validationErrors.AppendLine("Введите вторую часть " +
                    "серии");
            }
            if (string.IsNullOrWhiteSpace(RegionCode)
              || !int.TryParse(RegionCode, out int code)
              || code <= 0)
            {
                _ = validationErrors.AppendLine("Введите " +
                    "код региона в формате <nn> (например, 05)");
            }
            if (string.IsNullOrWhiteSpace(Country))
            {
                _ = validationErrors.AppendLine("Введите регион " +
                    "(например, RUS)");
            }

            if (validationErrors.Length > 0)
            {
                await FeedbackService.InformError(validationErrors);
                IsBusy = false;
                return;
            }
            IsBusy = true;
            CurrentCar.SeriesPartOne = SeriesPartOne.ToUpper();
            CurrentCar.RegistrationCode = registrationCode;
            CurrentCar.SeriesPartTwo = SeriesPartTwo.ToUpper();
            CurrentCar.RegionCode = int.Parse(RegionCode);
            CurrentCar.Country = Country.ToUpper();
            CurrentCar.CarType = SelectedCarType;
            if (await CarDataStore.AddItemAsync(currentCar))
            {
                await Shell.Current.GoToAsync("..");
            }
            IsBusy = false;
        }

        private string seriesPartOne;

        public string SeriesPartOne
        {
            get => seriesPartOne;
            set => SetProperty(ref seriesPartOne, value);
        }

        private string seriesPartTwo;

        public string SeriesPartTwo
        {
            get => seriesPartTwo;
            set => SetProperty(ref seriesPartTwo, value);
        }

        private string registrationCode;

        public string RegistrationCode
        {
            get => registrationCode;
            set => SetProperty(ref registrationCode, value);
        }

        private string regionCode;

        public string RegionCode
        {
            get => regionCode;
            set => SetProperty(ref regionCode, value);
        }

        private string country;

        public string Country
        {
            get => country;
            set => SetProperty(ref country, value);
        }
    }
}
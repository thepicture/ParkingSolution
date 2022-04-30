using ParkingSolution.XamarinApp.Models.Serialized;
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
            IsBusy = true;
            CurrentCar.SeriesPartOne = SeriesPartOne?.ToUpper();
            CurrentCar.RegistrationCode = RegistrationCode;
            CurrentCar.SeriesPartTwo = SeriesPartTwo?.ToUpper();
            CurrentCar.RegionCodeAsString = RegionCode;
            CurrentCar.Country = Country?.ToUpper();
            CurrentCar.CarType = SelectedCarType;
            if (await CarDataStore.AddItemAsync(CurrentCar))
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
using ParkingSolution.XamarinApp.Models.Serialized;
using ParkingSolution.XamarinApp.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ParkingSolution.XamarinApp.ViewModels
{
    public class MyCarsViewModel : BaseViewModel
    {

        private Command goToAddCarPage;

        public ICommand GoToAddCarPage
        {
            get
            {
                if (goToAddCarPage == null)
                {
                    goToAddCarPage = new Command(PerformGoToAddCarPage);
                }

                return goToAddCarPage;
            }
        }

        private async void PerformGoToAddCarPage()
        {
            await Shell
            .Current
            .GoToAsync(
                $"{nameof(AddCarPage)}");
        }

        internal void OnAppearing()
        {
            LoadCarsAsync();
        }

        private async void LoadCarsAsync()
        {
            Cars.Clear();
            IEnumerable<SerializedUserCar> dataStoreCars =
                await CarDataStore.GetItemsAsync();
            foreach (SerializedUserCar car in dataStoreCars)
            {
                await Task.Delay(200);
                Cars.Add(car);
            }
        }

        private ObservableCollection<SerializedUserCar> cars;

        public MyCarsViewModel()
        {
            Cars = new ObservableCollection<SerializedUserCar>();
        }

        public ObservableCollection<SerializedUserCar> Cars
        {
            get => cars;
            set => SetProperty(ref cars, value);
        }
    }
}
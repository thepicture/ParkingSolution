using System;
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

        private void PerformGoToAddCarPage()
        {
        }

        internal void OnAppearing()
        {
            Task.Run(() =>
            {
                LoadCarsAsync();
            });
        }

        private async void LoadCarsAsync()
        {
        }
    }
}
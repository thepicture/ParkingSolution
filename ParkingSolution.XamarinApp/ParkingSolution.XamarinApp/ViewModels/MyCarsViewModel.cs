using Newtonsoft.Json;
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
            Cars = new ObservableCollection<SerializedUserCar>();
            Task.Run(() =>
            {
                LoadCarsAsync();
            });
        }

        private async void LoadCarsAsync()
        {
            if (Cars.Count != 0)
            {
                Cars.Clear();
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
                        .GetAsync($"usercars")
                        .Result
                        .Content
                        .ReadAsStringAsync();
                    IEnumerable<SerializedUserCar> userCarsResponse =
                        JsonConvert.DeserializeObject
                        <IEnumerable<SerializedUserCar>>
                        (response);
                    foreach (SerializedUserCar userCar
                        in userCarsResponse)
                    {
                        await Task.Delay(500);
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Cars.Add(userCar);
                        });
                    }
                }
                catch (HttpRequestException ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    await FeedbackService.Inform("Автомобили не подгружены. "
                        + "Перезайдите на страницу");
                }
            }
        }

        private ObservableCollection<SerializedUserCar> cars;

        public ObservableCollection<SerializedUserCar> Cars
        {
            get => cars;
            set => SetProperty(ref cars, value);
        }
    }
}
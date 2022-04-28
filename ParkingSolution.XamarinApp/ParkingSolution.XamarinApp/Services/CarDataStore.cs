using Newtonsoft.Json;
using ParkingSolution.XamarinApp.Models.Serialized;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ParkingSolution.XamarinApp.Services
{
    public class CarDataStore : IDataStore<SerializedUserCar>
    {
        public async Task<bool> AddItemAsync(SerializedUserCar item)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic",
                                                  AppIdentity.AuthorizationValue);
                client.BaseAddress = App.BaseUrl;
                try
                {
                    string carJson = JsonConvert.SerializeObject(item);
                    HttpResponseMessage response = await client
                        .PostAsync(new Uri(client.BaseAddress + "usercars"),
                                   new StringContent(carJson,
                                                     Encoding.UTF8,
                                                     "application/json"));
                    string content = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            _ = DependencyService.Get<IFeedbackService>()
                            .Inform("Автомобиль добавлен");
                        });
                        return true;
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            _ = DependencyService
                            .Get<IFeedbackService>()
                            .Inform("Не удалось "
                            + "добавить автомобиль. "
                            + "Проверьте подключение к интернету");
                        });
                        return false;
                    }
                }
                catch (HttpRequestException ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    return await Task.FromResult(false);
                }
            }
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<SerializedUserCar> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SerializedUserCar>> GetItemsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(SerializedUserCar item)
        {
            throw new NotImplementedException();
        }
    }
}

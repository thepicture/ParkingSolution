using Newtonsoft.Json;
using ParkingSolution.XamarinApp.Models.Serialized;
using System;
using System.Collections.Generic;
using System.Net;
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
                    StringContent content =
                        new StringContent(carJson,
                                          Encoding.UTF8,
                                          "application/json");
                    HttpResponseMessage response = await client
                        .PostAsync("userCars", content);
                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        await DependencyService
                            .Get<IFeedbackService>()
                            .Inform("Автомобиль добавлен");
                    }
                    else
                    {
                        await DependencyService
                            .Get<IFeedbackService>()
                            .Inform(response);
                    }
                    return response.StatusCode == HttpStatusCode.Created;
                }
                catch (Exception ex)
                {
                    await DependencyService
                        .Get<IFeedbackService>()
                        .Inform(ex);
                    return false;
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

        public async Task<IEnumerable<SerializedUserCar>> GetItemsAsync(
            bool forceRefresh = false)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                  new AuthenticationHeaderValue("Basic",
                                                AppIdentity.AuthorizationValue);
                client.BaseAddress = App.BaseUrl;
                try
                {
                    HttpResponseMessage response = await client.GetAsync($"userCars");
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject
                            <IEnumerable<SerializedUserCar>>(
                                await response.Content.ReadAsStringAsync());
                    }
                    else
                    {
                        await DependencyService
                            .Get<IFeedbackService>()
                            .Inform(response);
                    }
                }
                catch (Exception ex)
                {
                    await DependencyService
                        .Get<IFeedbackService>()
                        .Inform(ex);
                }
            }
            return new List<SerializedUserCar>();
        }

        public Task<bool> UpdateItemAsync(SerializedUserCar item)
        {
            throw new NotImplementedException();
        }
    }
}

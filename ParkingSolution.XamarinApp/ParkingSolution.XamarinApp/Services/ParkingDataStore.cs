using Newtonsoft.Json;
using ParkingSolution.XamarinApp.Models.Serialized;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ParkingSolution.XamarinApp.Services
{
    public class ParkingDataStore : IDataStore<SerializedParking>
    {
        public async Task<bool> AddItemAsync(SerializedParking item)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic",
                                                  AppIdentity.AuthorizationValue);
                client.BaseAddress = App.BaseUrl;
                try
                {
                    string parkingJson = JsonConvert.SerializeObject(item);
                    HttpResponseMessage response = await client
                        .PostAsync(new Uri(client.BaseAddress + "parkings"),
                                   new StringContent(parkingJson,
                                                     Encoding.UTF8,
                                                     "application/json"));
                    string content = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        string action = item.Id == 0 ? "добавлена" : "изменена";
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            _ = DependencyService.Get<IFeedbackService>()
                            .Inform($"Парковка {action}");
                        });
                    }
                    else
                    {
                        string action = item.Id == 0 ? "добавить" : "изменить";
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            _ = DependencyService
                            .Get<IFeedbackService>()
                            .Inform("Не удалось "
                            + $"{action} парковку. "
                            + "Проверьте подключение к интернету");
                        });
                    }
                    return response.StatusCode ==
                        System.Net.HttpStatusCode.Created;
                }
                catch (HttpRequestException ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    return await Task.FromResult(false);
                }
            }
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                  new AuthenticationHeaderValue("Basic",
                                                AppIdentity.AuthorizationValue);
                client.BaseAddress = App.BaseUrl;
                try
                {
                    HttpResponseMessage response = await client
                     .DeleteAsync($"parkings/{id}");
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        _ = DependencyService.Get<IFeedbackService>()
                        .Inform("Парковка удалена");
                    });
                    return response.StatusCode == HttpStatusCode.NoContent;
                }
                catch (HttpRequestException ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        _ = DependencyService
                            .Get<IFeedbackService>()
                            .InformError("Парковка "
                            + "не удалена. Проверьте подлкючение "
                            + "к интернету");
                    });
                    return false;
                }
            }
        }

        public async Task<SerializedParking> GetItemAsync(string id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                  new AuthenticationHeaderValue("Basic",
                                                AppIdentity.AuthorizationValue);
                client.BaseAddress = App.BaseUrl;
                try
                {
                    string response = await client
                        .GetAsync($"parkings/{id}")
                        .Result
                        .Content
                        .ReadAsStringAsync();
                    SerializedParking parking = JsonConvert
                        .DeserializeObject
                        <SerializedParking>
                        (response);
                    return parking;
                }
                catch (HttpRequestException ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    return null;
                }
            }
        }

        public Task<IEnumerable<SerializedParking>> GetItemsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(SerializedParking item)
        {
            throw new NotImplementedException();
        }
    }
}

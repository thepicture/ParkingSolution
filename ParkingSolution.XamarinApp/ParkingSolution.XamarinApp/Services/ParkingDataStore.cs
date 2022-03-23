using Newtonsoft.Json;
using ParkingSolution.XamarinApp.Models.Serialized;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ParkingSolution.XamarinApp.Services
{
    public class ParkingDataStore : IDataStore<SerializedParking>
    {
        public Task<bool> AddItemAsync(SerializedParking item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<SerializedParking> GetItemAsync(string id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                  new AuthenticationHeaderValue("Basic",
                                                AppIdentity.AuthorizationValue);
                client.BaseAddress = new Uri((App.Current as App).BaseUrl);
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

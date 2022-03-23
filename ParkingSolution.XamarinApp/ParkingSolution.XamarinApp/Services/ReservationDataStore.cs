using Newtonsoft.Json;
using ParkingSolution.XamarinApp.Models.Serialized;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ParkingSolution.XamarinApp.Services
{
    public class ReservationDataStore : IDataStore<SerializedParkingPlaceReservation>
    {
        public async Task<bool> AddItemAsync(SerializedParkingPlaceReservation item)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic",
                                                  AppIdentity.AuthorizationValue);
                client.BaseAddress = new Uri((App.Current as App).BaseUrl);
                try
                {
                    string reservationJson = JsonConvert.SerializeObject(item);
                    HttpResponseMessage response = await client
                        .PostAsync(new Uri(client.BaseAddress + "parkingplacereservations"),
                                   new StringContent(reservationJson,
                                                     Encoding.UTF8,
                                                     "application/json"));
                    string content = await response.Content.ReadAsStringAsync();
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

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<SerializedParkingPlaceReservation> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SerializedParkingPlaceReservation>> GetItemsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(SerializedParkingPlaceReservation item)
        {
            throw new NotImplementedException();
        }
    }
}

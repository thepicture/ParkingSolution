using Newtonsoft.Json;
using ParkingSolution.XamarinApp.Models.Serialized;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ParkingSolution.XamarinApp.Services
{
    public class PaymentHistoryDataStore : IDataStore<SerializedPaymentHistory>
    {
        public async Task<bool> AddItemAsync(SerializedPaymentHistory item)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic",
                                                  AppIdentity.AuthorizationValue);
                client.BaseAddress = new Uri((App.Current as App).BaseUrl);
                try
                {
                    string paymentJson = JsonConvert.SerializeObject(item);
                    HttpResponseMessage response = await client
                        .PostAsync(new Uri(client.BaseAddress + "PaymentHistories"),
                                   new StringContent(paymentJson,
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

        public Task<SerializedPaymentHistory> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SerializedPaymentHistory>> GetItemsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(SerializedPaymentHistory item)
        {
            throw new NotImplementedException();
        }
    }
}

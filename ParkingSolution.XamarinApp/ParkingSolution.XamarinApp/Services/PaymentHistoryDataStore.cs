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
    public class PaymentHistoryDataStore : IDataStore<SerializedPaymentHistory>
    {
        public async Task<bool> AddItemAsync(SerializedPaymentHistory item)
        {
            StringBuilder validationErrors = new StringBuilder();
            if (item.CardNumber == null || item.CardNumber.Length != 27)
            {
                _ = validationErrors.AppendLine("Укажите " +
                    "корректный номер карты");
            }
            if (validationErrors.Length > 0)
            {
                await DependencyService
                    .Get<IFeedbackService>()
                    .InformError(validationErrors);
                return false;
            }
            item.CardNumber = item.CardNumber
                .Replace("(", "")
                .Replace(")", "")
                .Replace("-", "");
            using (HttpClient client = new HttpClient(App.ClientHandler))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic",
                                                  AppIdentity.AuthorizationValue);
                client.BaseAddress = App.BaseUrl;
                try
                {
                    string paymentJson = JsonConvert.SerializeObject(item);
                    StringContent content =
                        new StringContent(paymentJson,
                                          Encoding.UTF8,
                                          "application/json");
                    HttpResponseMessage response = await client
                        .PostAsync("PaymentHistories", content);
                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        await DependencyService
                            .Get<IFeedbackService>()
                            .Inform("Платёж успешен");
                    }
                    else
                    {
                        await DependencyService
                            .Get<IFeedbackService>()
                            .InformError(response);
                    }
                    return response.StatusCode == HttpStatusCode.Created;
                }
                catch (Exception ex)
                {
                    await DependencyService
                        .Get<IFeedbackService>()
                        .InformError(ex);
                    return false;
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

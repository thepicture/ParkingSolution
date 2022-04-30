using Newtonsoft.Json;
using ParkingSolution.XamarinApp.Models.Serialized;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ParkingSolution.XamarinApp.Services
{
    public class UserDataStore : IDataStore<SerializedUser>
    {
        public Task<bool> AddItemAsync(SerializedUser item)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            if (!await DependencyService
                .Get<IFeedbackService>()
                .Ask("Удалить сотрудника?"))
            {
                return false;
            }
            using (HttpClient client = new HttpClient(App.ClientHandler))
            {
                client.DefaultRequestHeaders.Authorization =
                  new AuthenticationHeaderValue("Basic",
                                                AppIdentity.AuthorizationValue);
                client.BaseAddress = App.BaseUrl;
                try
                {
                    HttpResponseMessage response = await client
                     .DeleteAsync($"users/{id}");
                    if (response.StatusCode == HttpStatusCode.NoContent)
                    {
                        await DependencyService
                            .Get<IFeedbackService>()
                            .Inform("Сотрудник удалён");
                    }
                    else
                    {
                        await DependencyService
                               .Get<IFeedbackService>()
                               .InformError(response);

                    }
                    return response.StatusCode == HttpStatusCode.NoContent;
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

        public Task<SerializedUser> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<SerializedUser>> GetItemsAsync(
            bool forceRefresh = false)
        {
            using (HttpClient client = new HttpClient(App.ClientHandler))
            {
                client.DefaultRequestHeaders.Authorization =
                  new AuthenticationHeaderValue("Basic",
                                                AppIdentity.AuthorizationValue);
                client.BaseAddress = App.BaseUrl;
                try
                {
                    HttpResponseMessage response = await client
                        .GetAsync($"users/employees");
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject
                            <IEnumerable<SerializedUser>>(
                            await response.Content.ReadAsStringAsync());
                    }
                    else
                    {
                        await DependencyService
                            .Get<IFeedbackService>()
                            .InformError(response);
                    }
                }
                catch (Exception ex)
                {
                    await DependencyService
                        .Get<IFeedbackService>()
                        .InformError(ex);
                }
                return new List<SerializedUser>();
            }
        }

        public Task<bool> UpdateItemAsync(SerializedUser item)
        {
            throw new NotImplementedException();
        }
    }
}

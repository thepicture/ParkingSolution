using Newtonsoft.Json;
using ParkingSolution.XamarinApp.Models.Serialized;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ParkingSolution.XamarinApp.Services
{
    public class RegistrationDataStore : IDataStore<SerializedRegistrationUser>
    {
        public async Task<bool> AddItemAsync(SerializedRegistrationUser item)
        {
            StringBuilder validationErrors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(item.PhoneNumber)
                || item.PhoneNumber.Length != 11)
            {
                _ = validationErrors.AppendLine("Введите корректный номер телефона");
            }
            if (string.IsNullOrWhiteSpace(item.Password))
            {
                _ = validationErrors.AppendLine("Введите пароль");
            }
            if (item.UserTypeId == 0)
            {
                _ = validationErrors.AppendLine("Укажите тип пользователя");
            }
            if (validationErrors.Length > 0)
            {
                await DependencyService
                    .Get<IFeedbackService>()
                    .InformError(validationErrors);
                return false;
            }
            string jsonIdentity = JsonConvert.SerializeObject(item);
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = App.BaseUrl;
                try
                {
                    StringContent content =
                        new StringContent(jsonIdentity,
                                          Encoding.UTF8,
                                          "application/json");
                    HttpResponseMessage response = await client
                       .PostAsync("users/register",
                                  content);
                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        await DependencyService
                            .Get<IFeedbackService>()
                            .InformError("Вы зарегистрированы");
                    }
                    else if (response.StatusCode == HttpStatusCode.Conflict)
                    {
                        await DependencyService
                            .Get<IFeedbackService>()
                            .InformError("Введите другой номер. Существующий " +
                            "уже используется");
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

        public Task<SerializedRegistrationUser> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SerializedRegistrationUser>> GetItemsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(SerializedRegistrationUser item)
        {
            throw new NotImplementedException();
        }
    }
}

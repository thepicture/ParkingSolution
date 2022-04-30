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
    public class LoginDataStore : IDataStore<SerializedLoginUser>
    {
        public async Task<bool> AddItemAsync(SerializedLoginUser item)
        {
            StringBuilder validationErrors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(item.PhoneNumber)
                || item.PhoneNumber.Length != 11)
            {
                _ = validationErrors.AppendLine("Введите номер телефона");
            }
            if (string.IsNullOrWhiteSpace(item.Password))
            {
                _ = validationErrors.AppendLine("Введите пароль");
            }
            if (validationErrors.Length > 0)
            {
                await DependencyService
                    .Get<IFeedbackService>()
                    .InformError(validationErrors);
                return false;
            }

            string jsonLoginUser = JsonConvert.SerializeObject(item);
            using (HttpClient client = new HttpClient(App.ClientHandler))
            {
                client.BaseAddress = App.BaseUrl;
                try
                {
                    StringContent content =
                        new StringContent(jsonLoginUser,
                                          Encoding.UTF8,
                                          "application/json");
                    HttpResponseMessage response = await client
                        .PostAsync("users/login", content);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string authorizationValue =
                            PhoneNumberAndPasswordToBasicEncoder
                            .Encode(item.PhoneNumber, item.Password);
                        string role = JsonConvert
                                .DeserializeObject<SerializedLoginUser>(
                                    await response.Content.ReadAsStringAsync())
                                .UserType;
                        if (item.IsRememberMe)
                        {
                            AppIdentity.AuthorizationValue = authorizationValue;
                            AppIdentity.Role = role;
                        }
                        else
                        {
                            App.AuthorizationValue = authorizationValue;
                            App.Role = role;
                        }
                        await DependencyService
                            .Get<IFeedbackService>()
                            .Inform($"Вы авторизованы с ролью {role.ToLower()}");
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        await DependencyService
                            .Get<IFeedbackService>()
                            .InformError("Неверный логин или пароль");
                    }
                    else
                    {
                        await DependencyService
                            .Get<IFeedbackService>()
                            .InformError(response);
                    }
                    return response.StatusCode == HttpStatusCode.OK;
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

        public Task<SerializedLoginUser> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SerializedLoginUser>> GetItemsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(SerializedLoginUser item)
        {
            throw new NotImplementedException();
        }
    }
}

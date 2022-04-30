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
            StringBuilder validationErrors = new StringBuilder();
            if (item.CarType == null)
            {
                _ = validationErrors.AppendLine("Выберите тип " +
                    "автомобиля");
            }
            if (string.IsNullOrWhiteSpace(item.SeriesPartOne))
            {
                _ = validationErrors.AppendLine("Введите первую часть " +
                    "серии");
            }
            if (string.IsNullOrWhiteSpace(item.RegistrationCode)
                || !int.TryParse(item.RegistrationCode, out _)
                || int.Parse(item.RegistrationCode) <= 0)
            {
                _ = validationErrors.AppendLine("Введите " +
                    "код регистрации в формате <nnn> (например, 002)");
            }
            if (string.IsNullOrWhiteSpace(item.SeriesPartTwo))
            {
                _ = validationErrors.AppendLine("Введите вторую часть " +
                    "серии");
            }
            if (string.IsNullOrWhiteSpace(item.RegionCodeAsString)
              || !int.TryParse(item.RegionCodeAsString, out int code)
              || code <= 0)
            {
                _ = validationErrors.AppendLine("Введите " +
                    "код региона в формате <nn> (например, 05)");
            }
            if (string.IsNullOrWhiteSpace(item.Country))
            {
                _ = validationErrors.AppendLine("Введите регион " +
                    "(например, RUS)");
            }

            if (validationErrors.Length > 0)
            {
                await DependencyService
                    .Get<IFeedbackService>()
                    .InformError(validationErrors);
                return false;
            }
            item.RegionCode = int.Parse(item.RegionCodeAsString);
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

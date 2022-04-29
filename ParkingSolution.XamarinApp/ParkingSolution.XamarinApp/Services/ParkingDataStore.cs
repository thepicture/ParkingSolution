using Newtonsoft.Json;
using ParkingSolution.XamarinApp.Models.Serialized;
using System;
using System.Collections.Generic;
using System.Linq;
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
            StringBuilder validationErrors = new StringBuilder();
            if (item.ParkingTypeId == 0)
            {
                _ = validationErrors.AppendLine("Выберите тип " +
                    "парковки");
            }
            if (string.IsNullOrWhiteSpace(item.City))
            {
                _ = validationErrors.AppendLine("Укажите город");
            }
            if (string.IsNullOrWhiteSpace(item.Street))
            {
                _ = validationErrors.AppendLine("Укажите улицу");
            }
            if (string.IsNullOrWhiteSpace(item.CostInRublesAsString)
                || !decimal.TryParse(item.CostInRublesAsString, out decimal price)
                || price <= 0)
            {
                _ = validationErrors.AppendLine("Стоимость должна " +
                    "быть положительной и в рублях");
            }
            if (item.BeforePaidTime >= item.BeforeFreeTime)
            {
                _ = validationErrors.AppendLine("Дата начала " +
                    "платной парковки " +
                    "должна быть раньше " +
                    "даты окончания платной парковки");
            }
            if (item.ParkingPlacesCarTypes == null
                || item.ParkingPlacesCarTypes.Count() == 0)
            {
                _ = validationErrors.AppendLine("Необходимо " +
                    "хотя бы одно парковочное место");
            }

            if (validationErrors.Length > 0)
            {
                await DependencyService
                    .Get<IFeedbackService>()
                    .InformError(validationErrors);
                return false;
            }
            item.CostInRubles = decimal.Parse(item.CostInRublesAsString);
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic",
                                                  AppIdentity.AuthorizationValue);
                client.BaseAddress = App.BaseUrl;
                try
                {
                    string parkingJson = JsonConvert.SerializeObject(item);
                    StringContent content =
                        new StringContent(parkingJson,
                                          Encoding.UTF8,
                                          "application/json");
                    HttpResponseMessage response = await client
                        .PostAsync("parkings", content);
                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        string action = item.Id == 0 ? "добавлена" : "изменена";
                        await DependencyService
                            .Get<IFeedbackService>()
                            .Inform($"Парковка {action}");
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

        public async Task<bool> DeleteItemAsync(string id)
        {
            if (!await DependencyService
                .Get<IFeedbackService>()
                .Ask("Удалить парковку?"))
            {
                return false;
            }
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
                    if (response.StatusCode == HttpStatusCode.NoContent)
                    {
                        await DependencyService
                            .Get<IFeedbackService>()
                            .Inform("Парковка удалена");
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
                    HttpResponseMessage response =
                        await client.GetAsync($"parkings/{id}");
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<SerializedParking>(
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
            }
            return null;
        }

        public async Task<IEnumerable<SerializedParking>> GetItemsAsync(
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
                    HttpResponseMessage response = await client
                        .GetAsync("parkings");
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert
                            .DeserializeObject<IEnumerable<SerializedParking>>(
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
            }
            return new List<SerializedParking>();
        }

        public Task<bool> UpdateItemAsync(SerializedParking item)
        {
            throw new NotImplementedException();
        }
    }
}

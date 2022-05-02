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
    public class ReservationDataStore : IDataStore<SerializedParkingPlaceReservation>
    {
        public async Task<bool> AddItemAsync(SerializedParkingPlaceReservation item)
        {
            StringBuilder validationErrors = new StringBuilder();

            if (item.CarId == 0)
            {
                _ = validationErrors.AppendLine("Укажите автомобиль");
            }
            if (item.FromDateTime < DateTime.Now)
            {
                _ = validationErrors.AppendLine("Парковка должна быть " +
                    "позднее текущей даты и времени");
            }
            if (item.IsKnownToDate && (item.FromDateTime >= item.LocalToDateTime))
            {
                _ = validationErrors.AppendLine("Дата начала " +
                    "должна быть раньше даты окончания");
            }
            if (validationErrors.Length > 0)
            {
                await DependencyService
                    .Get<IFeedbackService>()
                    .InformError(validationErrors);
                return false;
            }

            item.ToDateTime = item.IsKnownToDate
                ? item.LocalToDateTime
                : item.FromDateTime.AddHours(1);

            using (HttpClient client = new HttpClient(App.ClientHandler))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic",
                                                  AppIdentity.AuthorizationValue);
                client.BaseAddress = App.BaseUrl;
                try
                {
                    string reservationJson = JsonConvert.SerializeObject(item);
                    StringContent content =
                        new StringContent(reservationJson,
                                          Encoding.UTF8,
                                          "application/json");
                    HttpResponseMessage response = await client
                        .PostAsync("parkingPlaceReservations",
                                   content);
                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        await DependencyService
                            .Get<IFeedbackService>()
                            .Inform("Парковочное место забронировано");
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

        public Task<SerializedParkingPlaceReservation> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<SerializedParkingPlaceReservation>> GetItemsAsync(
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
                    HttpResponseMessage response =
                        await client.GetAsync("users/myparkingplaces");
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject
                            <IEnumerable<SerializedParkingPlaceReservation>>(
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
            return new List<SerializedParkingPlaceReservation>();
        }

        public Task<bool> UpdateItemAsync(SerializedParkingPlaceReservation item)
        {
            throw new NotImplementedException();
        }
    }
}

using Newtonsoft.Json;
using ParkingSolution.XamarinApp.Models.Serialized;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ParkingSolution.XamarinApp.Services
{
    public class ApiRegistrationService : IRegistrationService<SerializedUser>
    {
        public string ValidationResult { get; private set; }

        public async Task<bool> IsRegisteredAsync(SerializedUser identity)
        {
            string jsonIdentity = JsonConvert.SerializeObject(identity);
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri((App.Current as App).BaseUrl);
                try
                {
                    HttpResponseMessage response = await client
                       .PostAsync(new Uri(client.BaseAddress + "users/register"),
                                  new StringContent(jsonIdentity,
                                                    Encoding.UTF8,
                                                    "application/json"));
                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        ValidationResult = "Вы успешно зарегистрированы";
                        return true;
                    }
                    else if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        ValidationResult = "Не удалось "
                            + "зарегистрировать. "
                            + "Вероятно, политика компании изменилась. "
                            + "Обратитесь к системному администратору";
                        return false;
                    }
                    else if (response.StatusCode == HttpStatusCode.Conflict)
                    {
                        ValidationResult = "Пользователь "
                            + "с таким номером уже есть";
                        return false;
                    }
                }
                catch (HttpRequestException ex)
                {
                    ValidationResult = "Подключение к сети отсутствует";
                    Debug.WriteLine(ex.StackTrace);
                    return false;
                }
            }
            ValidationResult = "Ошибка сервера";
            return false;
        }
    }
}

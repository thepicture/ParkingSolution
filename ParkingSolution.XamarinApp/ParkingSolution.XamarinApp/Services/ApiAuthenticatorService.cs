using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ParkingSolution.XamarinApp.Services
{
    public class ApiAuthenticatorService : IAuthenticatorService
    {
        public string Role
        {
            get;
            private set;
        }

        public async Task<bool> IsCorrectAsync(string phoneNumber, string password)
        {
            string encodedPhoneNumberAndPassword =
                PhoneNumberAndPasswordToBasicEncoder
                .Encode(phoneNumber, password);
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(10);
                client.DefaultRequestHeaders.Authorization =
                     new AuthenticationHeaderValue("Basic",
                                                   encodedPhoneNumberAndPassword);
                client.BaseAddress = new Uri((App.Current as App).BaseUrl);
                try
                {
                    HttpResponseMessage response = await client
                        .GetAsync(new Uri(client.BaseAddress + "users/login"));
                    if (response.StatusCode != HttpStatusCode.Unauthorized)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        Role = content.Replace("\"", "");
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (HttpRequestException ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    throw;
                }
            }
        }
    }
}

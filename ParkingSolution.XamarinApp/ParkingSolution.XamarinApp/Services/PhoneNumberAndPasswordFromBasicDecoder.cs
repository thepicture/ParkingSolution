using System;
using System.Text;

namespace ParkingSolution.XamarinApp.Services
{
    public static class PhoneNumberAndPasswordFromBasicDecoder
    {
        public static string[] Decode()
        {
            string phoneNumberAndPassword = Encoding.UTF8.GetString(
                Convert.FromBase64String(AppIdentity.AuthorizationValue));
            return phoneNumberAndPassword.Split(':');
        }
    }
}

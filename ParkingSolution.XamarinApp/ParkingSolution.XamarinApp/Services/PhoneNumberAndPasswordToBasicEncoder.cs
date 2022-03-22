using System;
using System.Text;

namespace ParkingSolution.XamarinApp.Services
{
    public static class PhoneNumberAndPasswordToBasicEncoder
    {
        public static string Encode(string phoneNumber, string password)
        {
            string phoneNumberAndPassword = string.Format("{0}:{1}",
                                           phoneNumber,
                                           password);
            string encodedPhoneNumberAndPassword = Convert.ToBase64String(
                Encoding.UTF8.GetBytes(phoneNumberAndPassword));
            return encodedPhoneNumberAndPassword;
        }
    }
}

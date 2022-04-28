using Xamarin.Essentials;

namespace ParkingSolution.XamarinApp.Services
{
    public static class AppIdentity
    {
        public static void Reset()
        {
            App.AuthorizationValue = null;
            App.Role = null;
            SecureStorage.RemoveAll();
        }
        public static string Role
        {
            get
            {
                if (App.Role != null)
                {
                    return App.Role;
                }
                else
                {
                    return SecureStorage.GetAsync("Role").Result;
                }
            }
            set
            {
                App.Role = value;
                if (value == null)
                {
                    _ = SecureStorage.Remove("Role");
                }
                else
                {
                    _ = SecureStorage.SetAsync("Role", value);
                }
            }
        }
        public static string AuthorizationValue
        {
            get
            {
                if (App.AuthorizationValue != null)
                {
                    return App.AuthorizationValue;
                }
                else
                {
                    return SecureStorage.GetAsync("AuthorizationValue").Result;
                }
            }
            set
            {
                App.AuthorizationValue = value;
                _ = SecureStorage.SetAsync("AuthorizationValue", value);
            }
        }
    }
}

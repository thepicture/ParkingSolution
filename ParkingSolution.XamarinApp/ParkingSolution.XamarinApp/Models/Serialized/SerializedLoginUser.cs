using Newtonsoft.Json;

namespace ParkingSolution.XamarinApp.Models.Serialized
{
    public class SerializedLoginUser : SerializedUser
    {
        [JsonIgnore]
        public bool IsRememberMe { get; set; }
    }
}

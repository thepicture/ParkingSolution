using ParkingSolution.XamarinApp.Models.Serialized;
using Xamarin.Forms.Maps;

namespace ParkingSolution.XamarinApp.Models.Helpers
{
    public class ParkingHelper
    {
        public string Address { get; set; }
        public string Description { get; set; }
        public Position Position { get; set; }
        public SerializedParking Parking { get; set; }
    }
}

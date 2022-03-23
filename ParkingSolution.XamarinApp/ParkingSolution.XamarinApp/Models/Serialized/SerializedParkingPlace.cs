using System.Collections.Generic;

namespace ParkingSolution.XamarinApp.Models.Serialized
{
    public class SerializedParkingPlace
    {
        public int Id { get; set; }
        public int ParkingId { get; set; }
        public string CarType { get; set; }
        public bool IsFree { get; set; }
        public IEnumerable<SerializedParkingPlaceReservation> Reservations { get; set; }
    }
}
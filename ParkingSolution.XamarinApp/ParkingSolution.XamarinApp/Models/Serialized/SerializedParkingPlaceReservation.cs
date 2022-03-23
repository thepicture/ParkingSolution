using System;

namespace ParkingSolution.XamarinApp.Models.Serialized
{
    public class SerializedParkingPlaceReservation
    {
        public int Id { get; set; }
        public int ParkingPlaceId { get; set; }
        public DateTime FromDateTime { get; set; }
        public DateTime ToDateTime { get; set; }
        public int CarId { get; set; }
        public bool IsPayed { get; set; }
        public string CarType { get; set; }
        public decimal TotalPrice { get; set; }
    }
}

using ParkingSolution.WebAPI.Models.Entities;
using System;

namespace ParkingSolution.WebAPI.Models.Serialized
{
    public class SerializedParkingPlaceReservation
    {
        public SerializedParkingPlaceReservation(ParkingPlaceReservation reservation) 
        {
            Id = reservation.Id;
            ParkingPlaceId = reservation.ParkingPlaceId;
            FromDateTime = reservation.FromDateTime;
            ToDateTime = reservation.ToDateTime;
            CarId = reservation.CarId;
            IsPayed = reservation.IsPayed;
        }

        public int Id { get; set; }
        public int ParkingPlaceId { get; set; }
        public DateTime FromDateTime { get; set; }
        public DateTime ToDateTime { get; set; }
        public int CarId { get; set; }
        public bool IsPayed { get; set; }
    }
}

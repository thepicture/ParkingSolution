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

            CarType = reservation.ParkingPlace.CarType;
            for (DateTime dateTime = FromDateTime;
                dateTime < ToDateTime;
                dateTime = dateTime.AddHours(1))
            {
                if (dateTime.TimeOfDay > reservation.ParkingPlace.Parking.BeforePaidTime
                    && dateTime.TimeOfDay < reservation.ParkingPlace.Parking.BeforeFreeTime)
                {
                    TotalPrice += reservation.ParkingPlace.Parking.CostInRubles;
                }
            }
            ReservationFullAddress = reservation
                .ParkingPlace.Parking.Address.StreetName
                + ", "
                + reservation.ParkingPlace.Parking.Address.City.Name;
        }

        public int Id { get; set; }
        public int ParkingPlaceId { get; set; }
        public DateTime FromDateTime { get; set; }
        public DateTime ToDateTime { get; set; }
        public int CarId { get; set; }
        public bool IsPayed { get; set; }

        public string CarType { get; set; }
        public decimal TotalPrice { get; set; }
        public string ReservationFullAddress { get; set; }
    }
}

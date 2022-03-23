﻿using ParkingSolution.WebAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParkingSolution.WebAPI.Models.Serialized
{
    public class SerializedParkingPlace
    {
        public SerializedParkingPlace()
        {
        }

        public SerializedParkingPlace(ParkingPlace parkingPlace)
        {
            Id = parkingPlace.Id;
            ParkingId = parkingPlace.ParkingId;
            CarType = parkingPlace.CarType;
            IsFree = parkingPlace.ParkingPlaceReservation.Where(pp =>
            {
                if (pp.ToDateTime != null)
                {
                    return DateTime.Now >= pp.FromDateTime
                           || DateTime.Now <= pp.ToDateTime;
                }
                else
                {
                    return DateTime.Now >= pp.FromDateTime;
                }
            })
                .Count() == 0;
            Reservations = parkingPlace.ParkingPlaceReservation
                .Where(pp =>
                {
                    return (pp.ToDateTime == null && pp.FromDateTime > DateTime.Now)
                    || (pp.FromDateTime < DateTime.Now && pp.ToDateTime > DateTime.Now);
                })
                .ToList()
                .ConvertAll(pp =>
                {
                    return new SerializedParkingPlaceReservation(pp);
                });
        }
        public int Id { get; set; }
        public int ParkingId { get; set; }
        public string CarType { get; set; }
        public IEnumerable<SerializedParkingPlaceReservation> Reservations { get; set; }
        public bool IsFree { get; set; }
    }
}
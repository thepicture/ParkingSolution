﻿using System;

namespace ParkingSolution.XamarinApp.Models.Serialized
{
    public class SerializedParking
    {
        public int Id { get; set; }
        public int AddressId { get; set; }
        public int ParkingTypeId { get; set; }
        public TimeSpan BeforePaidTime { get; set; }
        public TimeSpan BeforeFreeTime { get; set; }
        public decimal CostInRubles { get; set; }
        public string Address { get; set; }
        public string ParkingType { get; set; }
        public int NumberOfParkingPlaces { get; set; }
    }
}
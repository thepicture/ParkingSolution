using ParkingSolution.WebAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParkingSolution.WebAPI.Models.Serialized
{
    public class SerializedParking
    {
        public SerializedParking()
        {
        }

        public SerializedParking(Parking parking)
        {
            Id = parking.Id;
            AddressId = parking.AddressId;
            ParkingTypeId = parking.ParkingTypeId;
            BeforePaidTime = parking.BeforePaidTime;
            BeforeFreeTime = parking.BeforeFreeTime;
            CostInRubles = parking.CostInRubles;
            Address = parking.Address.StreetName
                      + ", "
                      + parking.Address.City.Name;
            ParkingType = parking.ParkingType.Name;
            NumberOfParkingPlaces = parking
                .ParkingPlace
                .Where(pp => new SerializedParkingPlace(pp).IsFree)
                .Count();
            ParkingPlacesCarTypes = parking.ParkingPlace
                .Select(p => p.CarType);
            City = parking.Address.City.Name;
            Street = parking.Address.StreetName;
        }

        public int Id { get; set; }
        public int AddressId { get; set; }
        public int ParkingTypeId { get; set; }
        public TimeSpan BeforePaidTime { get; set; }
        public TimeSpan BeforeFreeTime { get; set; }
        public decimal CostInRubles { get; set; }
        public string Address { get; set; }
        public string ParkingType { get; set; }
        public int NumberOfParkingPlaces { get; set; }
        public IEnumerable<int> ParkingPlacesIds { get; set; }
        public IEnumerable<string> ParkingPlacesCarTypes { get; set; }

        public string Street { get; set; }
        public string City { get; set; }
    }
}
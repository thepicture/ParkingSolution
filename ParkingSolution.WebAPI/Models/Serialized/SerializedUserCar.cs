using ParkingSolution.WebAPI.Models.Entities;

namespace ParkingSolution.WebAPI.Models.Serialized
{
    public class SerializedUserCar
    {
        public SerializedUserCar()
        {
        }

        public SerializedUserCar(UserCar userCar)
        {
            Id = userCar.Id;
            CarType = userCar.CarType;
            UserId = userCar.UserId;
            SeriesPartOne = userCar.SeriesPartOne;
            SeriesPartTwo = userCar.SeriesPartTwo;
            RegistrationCode = userCar.RegistrationCode;
            RegionCode = userCar.RegionCode;
            Country = userCar.Country;
        }
        public int Id { get; set; }
        public string CarType { get; set; }
        public int UserId { get; set; }
        public string SeriesPartOne { get; set; }
        public string SeriesPartTwo { get; set; }
        public string RegistrationCode { get; set; }
        public int RegionCode { get; set; }
        public string Country { get; set; }
    }
}

namespace ParkingSolution.XamarinApp.Models.Serialized
{
    public class SerializedUserCar
    {
        public int Id { get; set; }
        public string CarType { get; set; }
        public int UserId { get; set; }
        public string SeriesPartOne { get; set; }
        public string SeriesPartTwo { get; set; }
        public string RegistrationCode { get; set; }
        public int RegionCode { get; set; }
        public string Country { get; set; }
        public string StringRepresentation
        {
            get
            {
                return SeriesPartOne
                    + RegistrationCode
                    + SeriesPartTwo
                    + ", "
                    + Country + ", тип " + CarType;
            }
        }
    }
}
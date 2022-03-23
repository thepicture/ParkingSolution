using System;

namespace ParkingSolution.WebAPI.Models.Serialized
{
    public class SerializedPaymentHistory
    {
        public SerializedPaymentHistory()
        {
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public decimal Sum { get; set; }
        public int ParkingPlaceId { get; set; }
        public string CardNumber { get; set; }
    }
}
using System;

namespace ParkingSolution.XamarinApp.Models.Serialized
{
    public class SerializedPaymentHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public decimal Sum { get; set; }
        public int ReservationId { get; set; }
        public string CardNumber { get; set; }
    }
}

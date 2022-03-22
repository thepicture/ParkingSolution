namespace ParkingSolution.XamarinApp.Models.Serialized
{
    public class SerializedUser
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public int UserTypeId { get; set; }
        public string UserType { get; set; }
    }
}

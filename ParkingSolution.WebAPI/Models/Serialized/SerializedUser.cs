using ParkingSolution.WebAPI.Models.Entities;

namespace ParkingSolution.WebAPI.Models.Serialized
{
    public class SerializedUser
    {
        public SerializedUser()
        {
        }

        public SerializedUser(User user)
        {
            Id = user.Id;
            PhoneNumber = user.PhoneNumber;
            UserTypeId = user.UserTypeId;
            UserType = user.UserType.Name;
        }
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public int UserTypeId { get; set; }
        public string UserType { get; set; }
    }
}
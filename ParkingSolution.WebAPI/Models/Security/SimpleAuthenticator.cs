using ParkingSolution.WebAPI.Models.Entities;
using System;
using System.Data.Entity;
using System.Linq;

namespace ParkingSolution.WebAPI.Models.Security
{
    public class SimpleAuthenticator
    {
        public static bool IsAuthenticated(string phoneNumber,
                                           string password,
                                           out User user)
        {
            using (ParkingBaseEntities context =
                new ParkingBaseEntities())
            {
                user = context
                    .User
                    .Include(u => u.UserType)
                    .FirstOrDefault(
                        u => u.PhoneNumber.Equals(phoneNumber,
                                            StringComparison.OrdinalIgnoreCase)
                              && u.Password == password);
                return user != null;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ParkingSolution.ImportApp
{
    class Program
    {
        private static readonly IList<string> _userTypes = new List<string>
        {
            "Клиент",
            "Сотрудник",
            "Администратор"
        };
        private static readonly IList<string> _carTypes = new List<string>
        {
            "A",
            "B",
            "C",
            "D",
            "E",
        };
        private static readonly IList<string> _parkingTypes = new List<string>
        {
            "Придорожная",
            "Плоскостная",
        };

        private static readonly Random random = new Random();
        static void Main()
        {
            InsertUserTypes();
            InsertUsers(10);
            InsertUserCars();
            InsertParkingTypes();
            InsertParkings();
            InsertParkingPlaces(5, 10);
        }

        private static void InsertUserCars()
        {
            using (ParkingBaseEntities entities = new ParkingBaseEntities())
            {
                foreach (User user in entities.User)
                {
                    user.UserCar.Add(new UserCar
                    {
                        CarNumber = string.Join("", Enumerable.Repeat(new string[] { "Р", "В", "Н", "У" }.ElementAt(random.Next(0, 4)), 2))
                        + random.Next(100, 1000) + new string[] { "Р", "В", "Н", "У" }.ElementAt(random.Next(0, 4)) 
                        + new string[] { "105", "54" }.ElementAt(random.Next(0, 2)),
                        CarType = _carTypes
                            .ElementAt(
                                random.Next(0, _carTypes.Count)),
                    });
                }
                entities.SaveChanges();
            }
        }

        private static void InsertParkingTypes()
        {
            using (ParkingBaseEntities entities = new ParkingBaseEntities())
            {
                foreach (string parkingType in _parkingTypes)
                {
                    entities.ParkingType.Add(new ParkingType
                    {
                        Id = _parkingTypes.IndexOf(parkingType) + 1,
                        Name = parkingType
                    });
                }
                entities.SaveChanges();
            }
        }

        private static void InsertParkingPlaces(int minCount, int maxCount)
        {
            using (ParkingBaseEntities entities = new ParkingBaseEntities())
            {
                foreach (Parking parking in entities.Parking)
                {
                    int count = random.Next(minCount, maxCount + 1);
                    for (int i = 0; i < count; i++)
                    {
                        parking.ParkingPlace.Add(new ParkingPlace
                        {
                            CarType = _carTypes
                            .ElementAt(
                                random.Next(0, _carTypes.Count)),
                        });
                    }
                }
                entities.SaveChanges();
            }
        }

        private static void InsertParkings()
        {
            using (ParkingBaseEntities entities = new ParkingBaseEntities())
            {
                DbSet<Address> adresses = entities.Address;
                foreach (Address address in adresses)
                {
                    TimeSpan beforePaidTime = TimeSpan.FromHours(
                        random.Next(6, 10));
                    TimeSpan beforeFreeTime = TimeSpan.FromHours(
                        random.Next(19, 22));
                    entities.Parking.Add(new Parking
                    {
                        AddressId = address.Id,
                        ParkingTypeId = random.Next(1, _parkingTypes.Count + 1),
                        BeforePaidTime = beforePaidTime,
                        BeforeFreeTime = beforeFreeTime,
                        CostInRubles = random.Next(30, 90),
                    });
                }
                entities.SaveChanges();
            }
        }

        private static void InsertUsers(int count)
        {
            using (ParkingBaseEntities entities = new ParkingBaseEntities())
            {
                for (int i = 0; i < count; i++)
                {
                    entities.User.Add(new User
                    {
                        PhoneNumber = (74950000000 + random
                            .Next(0, 9999999))
                            .ToString(),
                        Password = Guid
                            .NewGuid()
                            .ToString(),
                        UserTypeId = random.Next(1, _userTypes.Count + 1)
                    });
                }
                entities.SaveChanges();
            }
        }

        private static void InsertUserTypes()
        {
            using (ParkingBaseEntities entities = new ParkingBaseEntities())
            {
                foreach (string userType in _userTypes)
                {
                    entities.UserType.Add(new UserType
                    {
                        Id = _userTypes.IndexOf(userType) + 1,
                        Name = userType
                    });
                }
                entities.SaveChanges();
            }
        }
    }
}

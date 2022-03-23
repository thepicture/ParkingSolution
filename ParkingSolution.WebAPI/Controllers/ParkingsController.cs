using ParkingSolution.WebAPI.Models.Entities;
using ParkingSolution.WebAPI.Models.Serialized;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ParkingSolution.WebAPI.Controllers
{
    public class ParkingsController : ApiController
    {
        private readonly ParkingBaseEntities db = new ParkingBaseEntities();

        // GET: api/Parkings
        [ResponseType(typeof(List<SerializedParking>))]
        [Authorize(Roles = "Администратор, Сотрудник, Клиент")]
        public IHttpActionResult GetParking()
        {
            return Ok(db.Parking
                .ToList()
                .ConvertAll(p => new SerializedParking(p)));
        }

        // GET: api/Parkings/5
        [ResponseType(typeof(SerializedParking))]
        [Authorize(Roles = "Администратор, Сотрудник, Клиент")]
        public async Task<IHttpActionResult> GetParking(int id)
        {
            Parking parking = await db.Parking.FindAsync(id);
            if (parking == null)
            {
                return NotFound();
            }

            return Ok(
                new SerializedParking(parking)
            );
        }

        // PUT: api/Parkings/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutParking(int id, Parking parking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != parking.Id)
            {
                return BadRequest();
            }

            db.Entry(parking).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParkingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Parkings
        [ResponseType(typeof(int))]
        public async Task<IHttpActionResult> PostParking
            (SerializedParking serializedParking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            City city = await db.City
                .FirstOrDefaultAsync(c =>
                    c.Name.ToLower()
                    .Contains(
                        serializedParking.City.ToLower()));
            City newCity = null;
            if (city == null)
            {
                int lastCityId = db.City.Max(c => c.Id);
                newCity = new City
                {
                    Id = lastCityId + 1,
                    Name = serializedParking.City
                };
                db.City.Add(newCity);
                await db.SaveChangesAsync();
            }

            Address address = await db.Address
                .FirstOrDefaultAsync(c =>
                    c.StreetName.ToLower()
                    .Contains(
                        serializedParking.Street.ToLower()));
            Address newAddress = null;
            if (address == null)
            {
                int lastAddressId = db.Address.Max(a => a.Id);
                newAddress = new Address
                {
                    Id = lastAddressId + 1,
                    StreetName = serializedParking.Street,
                    CityId = newCity == null ? city.Id : newCity.Id
                };
                db.Address.Add(newAddress);
                await db.SaveChangesAsync();
            }

            Parking parking = new Parking
            {
                AddressId = newAddress == null ? address.Id : newAddress.Id,
                ParkingTypeId = serializedParking.ParkingTypeId,
                BeforeFreeTime = serializedParking.BeforeFreeTime,
                BeforePaidTime = serializedParking.BeforePaidTime,
                CostInRubles = serializedParking.CostInRubles
            };

            foreach (string carType in serializedParking.ParkingPlacesCarTypes)
            {
                ParkingPlace parkingPlace = new ParkingPlace
                {
                    Parking = parking,
                    CarType = carType
                };
            }

            db.Parking.Add(parking);
            await db.SaveChangesAsync();

            return Content(HttpStatusCode.Created, parking.Id);
        }

        // DELETE: api/Parkings/5
        [ResponseType(typeof(Nullable))]
        [Authorize(Roles = "Администратор")]
        public async Task<IHttpActionResult> DeleteParking(int id)
        {
            Parking parking = await db.Parking.FindAsync(id);
            if (parking == null)
            {
                return NotFound();
            }

            db.Parking.Remove(parking);
            await db.SaveChangesAsync();

            return Content(HttpStatusCode.NoContent, parking.Id);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ParkingExists(int id)
        {
            return db.Parking.Count(e => e.Id == id) > 0;
        }
    }
}
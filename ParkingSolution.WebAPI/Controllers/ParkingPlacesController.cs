using ParkingSolution.WebAPI.Models.Entities;
using ParkingSolution.WebAPI.Models.Serialized;
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
    public class ParkingPlacesController : ApiController
    {
        private readonly ParkingBaseEntities db = new ParkingBaseEntities();

        // GET: api/ParkingPlaces
        public IQueryable<ParkingPlace> GetParkingPlace()
        {
            return db.ParkingPlace;
        }

        // GET: api/ParkingPlaces/5
        [ResponseType(typeof(SerializedParkingPlace))]
        [Authorize(Roles = "Администратор, Сотрудник, Клиент")]
        public async Task<IHttpActionResult> GetParkingPlace(int id)
        {
            ParkingPlace parkingPlace = await db.ParkingPlace.FindAsync(id);
            if (parkingPlace == null)
            {
                return NotFound();
            }

            return Ok(
                new SerializedParkingPlace(parkingPlace)
            );
        }

        // GET: api/ParkingPlaces?parkingId=1
        [ResponseType(typeof(IEnumerable<SerializedParkingPlace>))]
        [Authorize(Roles = "Администратор, Сотрудник, Клиент")]
        public async Task<IHttpActionResult> GetParkingPlaces(int parkingId)
        {
            Parking parking = await db.Parking.FindAsync(parkingId);
            if (parking == null)
            {
                return NotFound();
            }

            return Ok(
                parking.ParkingPlace
                .ToList()
                .ConvertAll(pp =>
                {
                    return new SerializedParkingPlace(pp);
                })
            );
        }

        // PUT: api/ParkingPlaces/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutParkingPlace(int id, ParkingPlace parkingPlace)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != parkingPlace.Id)
            {
                return BadRequest();
            }

            db.Entry(parkingPlace).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParkingPlaceExists(id))
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

        // POST: api/ParkingPlaces
        [ResponseType(typeof(ParkingPlace))]
        public async Task<IHttpActionResult> PostParkingPlace(ParkingPlace parkingPlace)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ParkingPlace.Add(parkingPlace);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = parkingPlace.Id }, parkingPlace);
        }

        // DELETE: api/ParkingPlaces/5
        [ResponseType(typeof(ParkingPlace))]
        public async Task<IHttpActionResult> DeleteParkingPlace(int id)
        {
            ParkingPlace parkingPlace = await db.ParkingPlace.FindAsync(id);
            if (parkingPlace == null)
            {
                return NotFound();
            }

            db.ParkingPlace.Remove(parkingPlace);
            await db.SaveChangesAsync();

            return Ok(parkingPlace);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ParkingPlaceExists(int id)
        {
            return db.ParkingPlace.Count(e => e.Id == id) > 0;
        }
    }
}
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
        [Authorize(Roles ="Администратор, Сотрудник, Клиент")]
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
        [ResponseType(typeof(Parking))]
        public async Task<IHttpActionResult> PostParking(Parking parking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Parking.Add(parking);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = parking.Id }, parking);
        }

        // DELETE: api/Parkings/5
        [ResponseType(typeof(Parking))]
        public async Task<IHttpActionResult> DeleteParking(int id)
        {
            Parking parking = await db.Parking.FindAsync(id);
            if (parking == null)
            {
                return NotFound();
            }

            db.Parking.Remove(parking);
            await db.SaveChangesAsync();

            return Ok(parking);
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
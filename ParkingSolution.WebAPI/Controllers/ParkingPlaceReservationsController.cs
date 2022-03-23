using ParkingSolution.WebAPI.Models.Entities;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ParkingSolution.WebAPI.Controllers
{
    public class ParkingPlaceReservationsController : ApiController
    {
        private readonly ParkingBaseEntities db = new ParkingBaseEntities();

        // GET: api/ParkingPlaceReservations
        public IQueryable<ParkingPlaceReservation> GetParkingPlaceReservation()
        {
            return db.ParkingPlaceReservation;
        }

        // GET: api/ParkingPlaceReservations/5
        [ResponseType(typeof(ParkingPlaceReservation))]
        public async Task<IHttpActionResult> GetParkingPlaceReservation(int id)
        {
            ParkingPlaceReservation parkingPlaceReservation = 
                await db.ParkingPlaceReservation.FindAsync(id);
            if (parkingPlaceReservation == null)
            {
                return NotFound();
            }

            return Ok(parkingPlaceReservation);
        }

        // PUT: api/ParkingPlaceReservations/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutParkingPlaceReservation(int id, ParkingPlaceReservation parkingPlaceReservation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != parkingPlaceReservation.Id)
            {
                return BadRequest();
            }

            db.Entry(parkingPlaceReservation).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParkingPlaceReservationExists(id))
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

        // POST: api/ParkingPlaceReservations
        [ResponseType(typeof(ParkingPlaceReservation))]
        [Authorize(Roles = "Клиент")]
        public async Task<IHttpActionResult> PostParkingPlaceReservation
            (ParkingPlaceReservation parkingPlaceReservation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            parkingPlaceReservation.IsPayed = false;

            db.ParkingPlaceReservation.Add(parkingPlaceReservation);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                return BadRequest();
            }

            return Content(HttpStatusCode.Created, parkingPlaceReservation.Id);
        }

        // DELETE: api/ParkingPlaceReservations/5
        [ResponseType(typeof(ParkingPlaceReservation))]
        public async Task<IHttpActionResult> DeleteParkingPlaceReservation(int id)
        {
            ParkingPlaceReservation parkingPlaceReservation = await db.ParkingPlaceReservation.FindAsync(id);
            if (parkingPlaceReservation == null)
            {
                return NotFound();
            }

            db.ParkingPlaceReservation.Remove(parkingPlaceReservation);
            await db.SaveChangesAsync();

            return Ok(parkingPlaceReservation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ParkingPlaceReservationExists(int id)
        {
            return db.ParkingPlaceReservation.Count(e => e.Id == id) > 0;
        }
    }
}
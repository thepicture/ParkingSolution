using ParkingSolution.WebAPI.Models.Entities;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace ParkingSolution.WebAPI.Controllers
{
    public class PaymentHistoriesController : ApiController
    {
        private readonly ParkingBaseEntities db = new ParkingBaseEntities();

        // GET: api/PaymentHistories
        public IQueryable<PaymentHistory> GetPaymentHistory()
        {
            return db.PaymentHistory;
        }

        // GET: api/PaymentHistories/5
        [ResponseType(typeof(PaymentHistory))]
        public async Task<IHttpActionResult> GetPaymentHistory(int id)
        {
            PaymentHistory paymentHistory = await db.PaymentHistory.FindAsync(id);
            if (paymentHistory == null)
            {
                return NotFound();
            }

            return Ok(paymentHistory);
        }

        // PUT: api/PaymentHistories/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPaymentHistory(int id, PaymentHistory paymentHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != paymentHistory.Id)
            {
                return BadRequest();
            }

            db.Entry(paymentHistory).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentHistoryExists(id))
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

        // POST: api/PaymentHistories
        [ResponseType(typeof(int))]
        [Authorize(Roles = "Клиент")]
        public async Task<IHttpActionResult> PostPaymentHistory(PaymentHistory paymentHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = await db.User.FirstAsync(u =>
            u.PhoneNumber == HttpContext.Current.User.Identity.Name);

            paymentHistory.Date = System.DateTime.Now;
            paymentHistory.UserId = user.Id;
            db.ParkingPlaceReservation
                .Find(paymentHistory.ReservationId)
                .IsPayed = true;

            db.PaymentHistory.Add(paymentHistory);
            await db.SaveChangesAsync();

            return Content(HttpStatusCode.Created, paymentHistory.Id);
        }

        // DELETE: api/PaymentHistories/5
        [ResponseType(typeof(PaymentHistory))]
        public async Task<IHttpActionResult> DeletePaymentHistory(int id)
        {
            PaymentHistory paymentHistory = await db.PaymentHistory.FindAsync(id);
            if (paymentHistory == null)
            {
                return NotFound();
            }

            db.PaymentHistory.Remove(paymentHistory);
            await db.SaveChangesAsync();

            return Ok(paymentHistory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PaymentHistoryExists(int id)
        {
            return db.PaymentHistory.Count(e => e.Id == id) > 0;
        }
    }
}
using ParkingSolution.WebAPI.Models.Entities;
using ParkingSolution.WebAPI.Models.Serialized;
using System.Collections.Generic;
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
    public class UserCarsController : ApiController
    {
        private readonly ParkingBaseEntities db = new ParkingBaseEntities();

        // GET: api/UserCars
        [Authorize(Roles = "Клиент")]
        [ResponseType(typeof(List<SerializedUserCar>))]
        public async Task<IHttpActionResult> GetUserCar()
        {
            string phoneNumber = HttpContext.Current.User.Identity.Name;
            User user = await db.User
                .FirstAsync(u => u.PhoneNumber == phoneNumber);
            return Ok(
                user.UserCar.ToList()
                .ConvertAll(c =>
                {
                    return new SerializedUserCar(c);
                }));
        }

        // GET: api/UserCars/5
        [ResponseType(typeof(UserCar))]
        public async Task<IHttpActionResult> GetUserCar(int id)
        {
            UserCar userCar = await db.UserCar.FindAsync(id);
            if (userCar == null)
            {
                return NotFound();
            }

            return Ok(userCar);
        }

        // PUT: api/UserCars/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUserCar(int id, UserCar userCar)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userCar.Id)
            {
                return BadRequest();
            }

            db.Entry(userCar).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserCarExists(id))
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

        // POST: api/UserCars
        [ResponseType(typeof(int))]
        [Authorize(Roles = "Клиент")]
        public async Task<IHttpActionResult> PostUserCar(UserCar userCar)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string phoneNumber = HttpContext.Current.User.Identity.Name;
            User user = await db.User
                .FirstAsync(u => u.PhoneNumber == phoneNumber);

            userCar.UserId = user.Id;

            db.UserCar.Add(userCar);
            await db.SaveChangesAsync();

            return Content(HttpStatusCode.Created, userCar.Id);
        }

        // DELETE: api/UserCars/5
        [ResponseType(typeof(UserCar))]
        public async Task<IHttpActionResult> DeleteUserCar(int id)
        {
            UserCar userCar = await db.UserCar.FindAsync(id);
            if (userCar == null)
            {
                return NotFound();
            }

            db.UserCar.Remove(userCar);
            await db.SaveChangesAsync();

            return Ok(userCar);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserCarExists(int id)
        {
            return db.UserCar.Count(e => e.Id == id) > 0;
        }
    }
}
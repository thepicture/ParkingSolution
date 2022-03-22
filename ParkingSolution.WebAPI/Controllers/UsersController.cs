﻿using ParkingSolution.WebAPI.Models.Entities;
using ParkingSolution.WebAPI.Models.Serialized;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace ParkingSolution.WebAPI.Controllers
{
    public class UsersController : ApiController
    {
        private readonly ParkingBaseEntities db = new ParkingBaseEntities();

        // GET: api/Users
        public IQueryable<User> GetUser()
        {
            return db.User;
        }

        // GET: api/Users/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> GetUser(int id)
        {
            User user = await db.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.User.Add(user);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> DeleteUser(int id)
        {
            User user = await db.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            db.User.Remove(user);
            await db.SaveChangesAsync();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.User.Count(e => e.Id == id) > 0;
        }

        // POST: api/Users/register
        [ResponseType(typeof(int))]
        [Route("api/users/register")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> RegisterUser(SerializedUser serializedUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            User userToValidate = await db
                .User
                .FirstOrDefaultAsync(u =>
                    u.PhoneNumber == serializedUser.PhoneNumber);
            if (userToValidate != null)
            {
                return Conflict();
            }

            User user = new User
            {
                PhoneNumber = serializedUser.PhoneNumber,
                Password = serializedUser.Password,
                UserTypeId = serializedUser.UserTypeId
            };
            db.User.Add(user);

            await db.SaveChangesAsync();

            return Content(HttpStatusCode.Created, user.Id);
        }

        // GET: api/Users/login
        [ResponseType(typeof(string))]
        [Route("api/users/login")]
        [HttpGet]
        [Authorize(Roles = "Администратор, Сотрудник, Клиент")]
        public IHttpActionResult LoginUser()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return Ok(
                    (HttpContext.Current.User.Identity as ClaimsIdentity)
                        .FindFirst(ClaimTypes.Role)
                            .Value
                );
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
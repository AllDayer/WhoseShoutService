using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WhoseShoutFormsPrism.Models;
using WhoseShoutWebService.Models;

namespace WhoseShoutWebService.Controllers
{
    public class ShoutUsersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ShoutUsers
        public IQueryable<ShoutUser> GetShoutUsers()
        {
            return db.ShoutUsers;
        }

        // GET: api/ShoutUsers/5
        [ResponseType(typeof(ShoutUserDto))]
        public IHttpActionResult GetShoutUser(Guid id)
        {
            ShoutUser shoutUser = db.ShoutUsers.Find(id);

            if (shoutUser == null)
            {
                return NotFound();
            }

            return Ok(shoutUser);
        }

        // GET: api/ShoutUsers?socialId={socialId}&authType={authType}
        [ResponseType(typeof(ShoutUserDto))]
        public IHttpActionResult GetShoutUserBySocial(string socialId, AuthType authType)
        {
            ShoutUser shoutUser = null;

            if (!String.IsNullOrEmpty(socialId))
            {
                switch (authType)
                {
                    case AuthType.Facebook:
                        shoutUser = db.ShoutUsers.FirstOrDefault(x => x.FacebookID == socialId);
                        break;

                }
            }

            if (shoutUser == null)
            {
                return NotFound();
            }

            ShoutUserDto ret = new ShoutUserDto()
            {
                ID = shoutUser.ID,
                Email = shoutUser.Email,
                UserName = shoutUser.UserName,
                AuthType = AuthType.Facebook,
                ShoutSocialID = shoutUser.FacebookID
                
            };

            return Ok(ret);
        }

        // GET: api/ShoutUsers/email={email}
        [ResponseType(typeof(ShoutUserDto))]
        public IHttpActionResult GetShoutUserByEmail(string email)
        {
            ShoutUser shoutUser = null;

            if (!String.IsNullOrEmpty(email))
            {
                db.ShoutUsers.FirstOrDefault(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            }

            if (shoutUser == null)
            {
                return NotFound();
            }

            return Ok(shoutUser);
        }

        // PATCH: api/ShoutUsers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PatchShoutUser(Guid id, ShoutUser shoutUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != shoutUser.ID)
            {
                return BadRequest();
            }

            db.Entry(shoutUser).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShoutUserExists(id))
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

        // PUT: api/ShoutUsers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutShoutUser(Guid id, ShoutUser shoutUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != shoutUser.ID)
            {
                return BadRequest();
            }

            db.Entry(shoutUser).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShoutUserExists(id))
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

        // POST: api/ShoutUsers
        [ResponseType(typeof(ShoutUser))]
        public IHttpActionResult PostShoutUser(ShoutUser shoutUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ShoutUsers.Add(shoutUser);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ShoutUserExists(shoutUser.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = shoutUser.ID }, shoutUser);
        }

        // DELETE: api/ShoutUsers/5
        [ResponseType(typeof(ShoutUser))]
        public IHttpActionResult DeleteShoutUser(Guid id)
        {
            ShoutUser shoutUser = db.ShoutUsers.Find(id);
            if (shoutUser == null)
            {
                return NotFound();
            }

            db.ShoutUsers.Remove(shoutUser);
            db.SaveChanges();

            return Ok(shoutUser);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ShoutUserExists(Guid id)
        {
            return db.ShoutUsers.Count(e => e.ID == id) > 0;
        }
    }
}
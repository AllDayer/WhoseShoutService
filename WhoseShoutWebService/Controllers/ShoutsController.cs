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
    public class ShoutsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Shouts
        public IQueryable<ShoutDto> GetShouts()
        {
            //return db.Shouts.Include(g => g.ShoutGroup);

            var shouts = from s in db.Shouts
                         select new ShoutDto()
                         {
                             ID = s.ID,
                             PurchaseTimeUtc = s.PurchaseTimeUtc,
                             Cost = s.Cost,
                             ShoutCategory = s.ShoutGroup.Category,
                             ShoutGroupID = s.ShoutGroupID,
                             ShoutGroupName = s.ShoutGroup.Name,
                             ShoutUserID = s.ShoutUser.ID,
                             ShoutUserName = s.ShoutUser.UserName,
                         };
            return shouts;
        }

        // GET: api/Shouts/5
        [ResponseType(typeof(Shout))]
        public IHttpActionResult GetShout(Guid id)
        {
            //Shout shout = db.Shouts.Find(id);

            var shout = from s in db.Shouts
                        where s.ID == id
                        select new ShoutDto()
                        {
                            ID = s.ID,
                            PurchaseTimeUtc = s.PurchaseTimeUtc,
                            Cost = s.Cost,
                            ShoutCategory = s.ShoutGroup.Category,
                            ShoutGroupID = s.ShoutGroupID,
                            ShoutGroupName = s.ShoutGroup.Name,
                            ShoutUserID = s.ShoutUser.ID,
                            ShoutUserName = s.ShoutUser.UserName,
                        };
            if (shout == null)
            {
                return NotFound();
            }

            return Ok(shout);
        }

        public IQueryable<ShoutDto> ShoutsForGroup(Guid groupID)
        {
            //return db.Shouts.Include(g => g.ShoutGroup);

            var shouts = from s in db.Shouts
                         where s.ShoutGroupID == groupID
                         select new ShoutDto()
                         {
                             ID = s.ID,
                             PurchaseTimeUtc = s.PurchaseTimeUtc,
                             Cost = s.Cost,
                             ShoutCategory = s.ShoutGroup.Category,
                             ShoutGroupID = s.ShoutGroupID,
                             ShoutGroupName = s.ShoutGroup.Name,
                             ShoutUserID = s.ShoutUser.ID,
                             ShoutUserName = s.ShoutUser.UserName,
                         };
            return shouts;
        }


        // PUT: api/Shouts/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutShout(Guid id, Shout shout)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != shout.ID)
            {
                return BadRequest();
            }

            db.Entry(shout).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShoutExists(id))
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

        // POST: api/Shouts
        [ResponseType(typeof(ShoutDto))]
        public IHttpActionResult PostShout(ShoutDto shoutDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Shout shout = new Shout()
            {
                ID = shoutDto.ID,
                Cost = shoutDto.Cost,
                PurchaseTimeUtc = DateTime.UtcNow,

                ShoutGroup = db.ShoutGroups.FirstOrDefault(sg => sg.ID == shoutDto.ShoutGroupID),
                ShoutUser = db.ShoutUsers.FirstOrDefault(su => su.ID == shoutDto.ShoutUserID)
            };

            //Verify the user is in the group?

            db.Shouts.Add(shout);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ShoutExists(shout.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = shoutDto.ID }, shoutDto);
        }

        // DELETE: api/Shouts/5
        [ResponseType(typeof(Shout))]
        public IHttpActionResult DeleteShout(Guid id)
        {
            Shout shout = db.Shouts.Find(id);
            if (shout == null)
            {
                return NotFound();
            }

            db.Shouts.Remove(shout);
            db.SaveChanges();

            return Ok(shout);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ShoutExists(Guid id)
        {
            return db.Shouts.Count(e => e.ID == id) > 0;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
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
    public class ShoutGroupsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ShoutGroups
        public IQueryable<ShoutGroupDto> GetShoutGroups()
        {
            var shoutGroups = from s in db.ShoutGroups
                              select new ShoutGroupDto()
                              {
                                  ID = s.ID,
                                  Category = s.Category,
                                  Name = s.Name,
                                  Users = (from u in db.ShoutUsers
                                           where s.ShoutUsers.Any(sh => sh.ID == u.ID)
                                           select new ShoutUserDto()
                                           {
                                               UserName = u.UserName,
                                               ID = u.ID
                                           }).ToList(),
                                  Shouts = (from shout in db.Shouts
                                            where s.Shouts.Any(sh => sh.ID == shout.ID)
                                            select new ShoutDto()
                                            {
                                                Cost = shout.Cost,
                                                ID = shout.ID,
                                                PurchaseTimeUtc = shout.PurchaseTimeUtc,
                                                ShoutUserName = shout.ShoutUser.UserName,
                                                ShoutGroupName = shout.ShoutGroup.Name
                                            }).ToList()

                              };
            return shoutGroups;
        }

        [ResponseType(typeof(ShoutGroupDto))]
        public IQueryable<ShoutGroupDto> GetShoutGroupForUser(Guid shoutUserId)
        {
            var shoutGroups = from s in db.ShoutGroups
                              from su in s.ShoutUsers
                              where su.ID == shoutUserId
                              select new ShoutGroupDto()
                              {
                                  ID = s.ID,
                                  Category = s.Category,
                                  Name = s.Name,
                                  Shouts = (from shout in db.Shouts
                                            where s.Shouts.Any(sh => sh.ID == shout.ID)
                                            orderby shout.PurchaseTimeUtc descending
                                            select new ShoutDto()
                                            {
                                                Cost = shout.Cost,
                                                ID = shout.ID,
                                                PurchaseTimeUtc = shout.PurchaseTimeUtc,
                                                ShoutUserName = shout.ShoutUser.UserName,
                                                ShoutGroupName = shout.ShoutGroup.Name
                                            }).ToList(),
                                  Users = (from u in db.ShoutUsers
                                           where s.ShoutUsers.Any(sh => sh.ID == u.ID)
                                           select new ShoutUserDto()
                                           {
                                               UserName = u.UserName,
                                               ID = u.ID,
                                               ShoutCount = (from zx in db.Shouts
                                                             where zx.ShoutGroup.ID == s.ID &&
                                                                   zx.ShoutUser.ID == u.ID
                                                             select zx).Count()
                                                             //,
                                           }).ToList()

                              };

            return shoutGroups;
        }


        // GET: api/ShoutGroups/<Guid>
        [ResponseType(typeof(ShoutGroupDto))]
        public IHttpActionResult GetShoutGroup(Guid id)
        {
            var shoutGroup = from s in db.ShoutGroups
                             where s.ID == id
                             select new ShoutGroupDto()
                             {
                                 ID = s.ID,
                                 Category = s.Category,
                                 Name = s.Name,
                                 Users = (from u in db.ShoutUsers
                                          where s.ShoutUsers.Any(sh => sh.ID == u.ID)
                                          select new ShoutUserDto()
                                          {
                                              UserName = u.UserName,
                                              ID = u.ID
                                          }).ToList(),
                                 Shouts = (from shout in db.Shouts
                                           where s.Shouts.Any(sh => sh.ID == shout.ID)
                                           select new ShoutDto()
                                           {
                                               Cost = shout.Cost,
                                               ID = shout.ID,
                                               PurchaseTimeUtc = shout.PurchaseTimeUtc,
                                               ShoutUserName = shout.ShoutUser.UserName,
                                               ShoutGroupName = shout.ShoutGroup.Name
                                           }).ToList()

                             };
            if (shoutGroup == null)
            {
                return NotFound();
            }

            return Ok(shoutGroup);
        }

        // PUT: api/ShoutGroups/<guid>
        // Not tested
        [ResponseType(typeof(void))]
        public IHttpActionResult PutShoutGroup(Guid id, ShoutGroup shoutGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != shoutGroup.ID)
            {
                return BadRequest();
            }

            db.Entry(shoutGroup).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShoutGroupExists(id))
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

        // POST: api/ShoutGroups
        [ResponseType(typeof(ShoutGroupDto))]
        public IHttpActionResult PostShoutGroup(ShoutGroupDto shoutGroupDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ShoutGroup shoutGro = new ShoutGroup()
            {
                ID = shoutGroupDto.ID,
                Name = shoutGroupDto.Name,
                Category = shoutGroupDto.Category,
                Shouts = new List<Shout>(),
                ShoutUsers = new List<ShoutUser>()
            };

            var shoutUsersForGroup = new List<ShoutUser>();
            foreach (var sh in shoutGroupDto.Users)
            {
                ShoutUser u = null;

                if (sh.ID != null && sh.ID != Guid.Empty)
                {
                    u = db.ShoutUsers.FirstOrDefault(x => x.ID == sh.ID);
                }
                else if (u == null && !String.IsNullOrEmpty(sh.Email))
                {
                    u = db.ShoutUsers.FirstOrDefault(x => x.Email.Equals(sh.Email, StringComparison.OrdinalIgnoreCase));
                }

                if (u != null)
                {
                    shoutGro.ShoutUsers.Add(u);
                    if (u.ShoutGroups == null)
                    {
                        u.ShoutGroups = new List<ShoutGroup>();
                    }
                    u.ShoutGroups.Add(shoutGro);
                    shoutUsersForGroup.Add(u);
                }
                else
                {
                    var newUser = new ShoutUser() { ID = Guid.NewGuid(), Email = sh.Email };
                    if (newUser.ShoutGroups == null)
                    {
                        newUser.ShoutGroups = new List<ShoutGroup>();
                    }
                    newUser.ShoutGroups.Add(shoutGro);
                    shoutGro.ShoutUsers.Add(newUser);

                    db.ShoutUsers.Add(newUser);
                }
            }

            foreach (var sh in shoutUsersForGroup)
            {
                db.ShoutUsers.Attach(sh);
            }

            db.ShoutGroups.Add(shoutGro);


            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ShoutGroupExists(shoutGroupDto.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = shoutGroupDto.ID }, shoutGroupDto);
        }

        // DELETE: api/ShoutGroups/<guid>
        [ResponseType(typeof(ShoutGroup))]
        public IHttpActionResult DeleteShoutGroup(Guid id)
        {
            ShoutGroup shoutGroup = db.ShoutGroups.Find(id);
            if (shoutGroup == null)
            {
                return NotFound();
            }

            db.ShoutGroups.Remove(shoutGroup);
            db.SaveChanges();

            return Ok(shoutGroup);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ShoutGroupExists(Guid id)
        {
            return db.ShoutGroups.Count(e => e.ID == id) > 0;
        }
    }
}
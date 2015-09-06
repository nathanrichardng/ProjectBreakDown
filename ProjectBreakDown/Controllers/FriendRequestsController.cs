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
using ProjectBreakDown.Models;

namespace ProjectBreakDown.Controllers
{
    public class FriendRequestsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/FriendRequests
        public ICollection<FriendRequest> GetFriendRequests()
        {
            var user = db.Users.First(u => u.UserName == User.Identity.Name);
            return user.friendRequestsReceived.Where(fr => fr.status == "Pending approval").ToList();
        }

        // GET: api/FriendRequests/5
        [ResponseType(typeof(FriendRequest))]
        public IHttpActionResult GetFriendRequest(int id)
        {
            FriendRequest friendRequest = db.FriendRequests.Find(id);
            if (friendRequest == null)
            {
                return NotFound();
            }
            else if (friendRequest.FromName != User.Identity.Name && friendRequest.ToName != User.Identity.Name)
            {
                return Unauthorized();
            }

            return Ok(friendRequest);
        }

        // PUT: api/FriendRequests/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFriendRequest(int id, FriendRequest friendRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != friendRequest.FriendRequestId)
            {
                return BadRequest();
            }

            db.Entry(friendRequest).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FriendRequestExists(id))
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

        // POST: api/FriendRequests
        //need to work on getting this to work
        [ResponseType(typeof(FriendRequest))]
        public IHttpActionResult PostFriendRequest(ApplicationUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var fromUser = db.Users.First(u => u.UserName == User.Identity.Name);
            var toUser = db.Users.First(u => u.UserName == user.UserName);


            if (db.FriendRequests.Any(fr => fr.ToName == toUser.UserName && fr.FromName == fromUser.UserName))
            {
                return Conflict();
            }

            var friendRequest = new FriendRequest(fromUser, toUser);
            
            db.FriendRequests.Add(friendRequest);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = friendRequest.FriendRequestId }, friendRequest);
        }

        // DELETE: api/FriendRequests/5
        [ResponseType(typeof(FriendRequest))]
        public IHttpActionResult DeleteFriendRequest(int id)
        {
            FriendRequest friendRequest = db.FriendRequests.Find(id);
            if (friendRequest == null)
            {
                return NotFound();
            }

            db.FriendRequests.Remove(friendRequest);
            db.SaveChanges();

            return Ok(friendRequest);
        }

        //GET: api/FriendRequests/5/accept
        [Route("api/FriendRequests/{friendRequestId}/Accept")]
        [ResponseType(typeof(FriendRequest))]
        [HttpGet]
        public IHttpActionResult AcceptFriendRequest(int friendRequestId)
        {
            var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var friendRequest = db.FriendRequests.Find(friendRequestId);
            if (friendRequest.ToUser != user)
            {
                return Unauthorized();
            }

            friendRequest.AcceptRequest();
            db.SaveChanges();

            return Ok(friendRequest.status);
        }

        //GET: api/FriendRequests/5/Reject
        [Route("api/FriendRequests/{friendRequestId}/Reject")]
        [ResponseType(typeof(FriendRequest))]
        [HttpGet]
        public IHttpActionResult RejectFreindRequest(int friendRequestId)
        {
            var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var friendRequest = db.FriendRequests.Find(friendRequestId);
            if (friendRequest.ToUser != user)
            {
                return Unauthorized();
            }

            friendRequest.RejectRequest();
            db.SaveChanges();

            return Ok(friendRequest.status);
        }

        //GET: api/Friends
        [Route("api/Friends")]
        [ResponseType(typeof(FriendRequest))]
        [HttpGet]
        public IHttpActionResult GetFriends()
        {
            var friends = db.FriendRequests.Where(fr => fr.status == "Accepted" && (fr.ToName == User.Identity.Name || fr.FromName == User.Identity.Name));
            return Ok(friends);
        }

        //GET: api/Friends/5
        [Route("api/Friends/{friendRequestId}")]
        [ResponseType(typeof(FriendRequest))]
        [HttpGet]
        public IHttpActionResult GetFriend(int friendRequestId)
        {
            var friend = db.FriendRequests.Find(friendRequestId);

            if (friend == null)
            {
                return BadRequest();
            }


            if (friend.FromName != User.Identity.Name && friend.ToName != User.Identity.Name)
            {
                return Unauthorized();
            }
            return Ok(friend);
        }

        //GET: api/FriendRequests/Rejected
        [Route("api/FriendRequests/Rejected")]
        [ResponseType(typeof(FriendRequest))]
        [HttpGet]
        public IHttpActionResult GetRejectedFriendRequests()
        {
            var friends = db.FriendRequests.Where(fr => fr.status == "Rejected" && (fr.ToName == User.Identity.Name || fr.FromName == User.Identity.Name));
            return Ok(friends);
        }

        //GET: api/FriendRequests/Rejected/5
        [Route("api/FriendRequests/Rejected/{friendRequestId}")]
        [ResponseType(typeof(FriendRequest))]
        [HttpGet]
        public IHttpActionResult GetRejectedFriendRequest(int friendRequestId)
        {
            var friendRequest = db.FriendRequests.Find(friendRequestId);

            if (friendRequest == null)
            {
                return BadRequest();
            }

            if (friendRequest.status != "Rejected")
            {
                return BadRequest();
            }


            if (friendRequest.ToName != User.Identity.Name)
            {
                return Unauthorized();
            }
            return Ok(friendRequest);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FriendRequestExists(int id)
        {
            return db.FriendRequests.Count(e => e.FriendRequestId == id) > 0;
        }
    }
}
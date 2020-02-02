using CompanyRatesAPI.Models;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace CompanyRatesAPI.Controllers
{
    [EnableCors(origins: "https://companyratesapi.azurewebsites.net", headers: "*", methods: "*")]
    public class VoteReviewsController : ApiController
    {
        private CompanyRatesAPIContext db = new CompanyRatesAPIContext();

        //// GET: api/VoteReviews
        //public IQueryable<VoteReview> GetVoteReviews()
        //{
        //    return db.VoteReviews;
        //}

        //// GET: api/VoteReviews/5
        //[ResponseType(typeof(VoteReview))]
        //public IHttpActionResult GetVoteReview(int id)
        //{
        //    VoteReview voteReview = db.VoteReviews.Find(id);
        //    if (voteReview == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(voteReview);
        //}

        //// PUT: api/VoteReviews/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutVoteReview(int id, VoteReview voteReview)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != voteReview.VoteReviewID)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(voteReview).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!VoteReviewExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        // POST: api/VoteReviews
        [ResponseType(typeof(bool))]
        public IHttpActionResult PostVoteReview(string sessionkey, VoteReview voteReview)
        {
            if (Helper.isSessionValid(sessionkey) && Helper.isAdmin(sessionkey) || Helper.isUser(sessionkey))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var isOk = Helper.VoteReview(voteReview);

                return Ok(isOk);
            }
            else
                return Unauthorized();
        }

        //// DELETE: api/VoteReviews/5
        //[ResponseType(typeof(VoteReview))]
        //public IHttpActionResult DeleteVoteReview(int id)
        //{
        //    VoteReview voteReview = db.VoteReviews.Find(id);
        //    if (voteReview == null)
        //    {
        //        return NotFound();
        //    }

        //    db.VoteReviews.Remove(voteReview);
        //    db.SaveChanges();

        //    return Ok(voteReview);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VoteReviewExists(int id)
        {
            return db.VoteReviews.Count(e => e.VoteReviewID == id) > 0;
        }
    }
}
using CompanyRatesAPI.Models;
using System.Web.Http;
using System.Web.Http.Cors;

namespace CompanyRatesAPI.Controllers
{
    [EnableCors(origins: "https://companyratesapi.azurewebsites.net", headers: "*", methods: "*")]
    public class ReviewsController : ApiController
    {
        // GET: api/Companies
        public IHttpActionResult GetReviews()
        {
            return Ok(Helper.getReviews());
        }

        // GET: api/Companies/5
        public IHttpActionResult GetReview(int id)
        {
            var reviews = Helper.getReviewByCompany(id);

            return Ok(reviews);
        }

        // PUT: api/Companies/5
        public IHttpActionResult PutReview(int id, string sessionkey, Review review)
        {
            if (Helper.isSessionValid(sessionkey) && Helper.isAdmin(sessionkey) || Helper.isUser(sessionkey))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != review.ReviewID)
                {
                    return BadRequest();
                }

                Helper.updateReview(id, review);
                return Ok();
            }
            else
                return Unauthorized();
        }

        // POST: api/Companies
        public IHttpActionResult PostReview(string sessionkey, Review review)
        {
            if (Helper.isSessionValid(sessionkey) && Helper.isAdmin(sessionkey) || Helper.isUser(sessionkey))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Review _review = Helper.createReview(review);
                return Ok(_review);
            }
            else
                return Unauthorized();
        }

        // DELETE: api/Reviews/5
        public IHttpActionResult DeleteReview(int id, string sessionkey)
        {
            if (Helper.isSessionValid(sessionkey) && Helper.isAdmin(sessionkey) || Helper.isUser(sessionkey))
            {
                bool result = Helper.removeReview(id);
                if (result)
                    return Ok(result);
                else
                    return NotFound();
            }
            else
                return Unauthorized();
        }
    }
}
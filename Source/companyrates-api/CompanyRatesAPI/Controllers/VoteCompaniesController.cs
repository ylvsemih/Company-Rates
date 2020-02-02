using CompanyRatesAPI.Models;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace CompanyRatesAPI.Controllers
{
    [EnableCors(origins: "https://companyratesapi.azurewebsites.net", headers: "*", methods: "*")]
    public class VoteCompaniesController : ApiController
    {
        //// GET: api/VoteCompanies
        //public IQueryable<VoteCompany> GetVoteCompanies()
        //{
        //    return db.VoteCompanies;
        //}

        //// GET: api/VoteCompanies/5
        //[ResponseType(typeof(VoteCompany))]
        //public IHttpActionResult GetVoteCompany(int id)
        //{
        //    VoteCompany voteCompany = db.VoteCompanies.Find(id);
        //    if (voteCompany == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(voteCompany);
        //}

        //// PUT: api/VoteCompanies/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutVoteCompany(int id, VoteCompany voteCompany)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != voteCompany.VoteCompanyID)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(voteCompany).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!VoteCompanyExists(id))
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

        // POST: api/VoteCompanies
        [ResponseType(typeof(bool))]
        public IHttpActionResult PostVoteCompany(string sessionkey, VoteCompany voteCompany)
        {
            if (Helper.isSessionValid(sessionkey) && Helper.isAdmin(sessionkey) || Helper.isUser(sessionkey))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var isOk = Helper.VoteCompany(voteCompany);

                return Ok(isOk);
            }
            else
                return Unauthorized();
        }

        //// DELETE: api/VoteCompanies/5
        //[ResponseType(typeof(VoteCompany))]
        //public IHttpActionResult DeleteVoteCompany(int id)
        //{
        //    VoteCompany voteCompany = db.VoteCompanies.Find(id);
        //    if (voteCompany == null)
        //    {
        //        return NotFound();
        //    }

        //    db.VoteCompanies.Remove(voteCompany);
        //    db.SaveChanges();

        //    return Ok(voteCompany);
        //}

        private bool VoteCompanyExists(int id)
        {
            using (var db = new CompanyRatesAPIContext())
            {
                return db.VoteCompanies.Count(e => e.VoteCompanyID == id) > 0;
            }
        }
    }
}
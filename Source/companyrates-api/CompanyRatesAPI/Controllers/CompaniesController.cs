using CompanyRatesAPI.Models;
using System.Web.Http;
using System.Web.Http.Cors;

namespace CompanyRatesAPI.Controllers
{
    [EnableCors(origins: "https://companyratesapi.azurewebsites.net", headers: "*", methods: "*")]
    public class CompaniesController : ApiController
    {
        // GET: api/Companies
        public IHttpActionResult GetCompanies()
        {
            return Ok(Helper.getCompanies());
        }

        // GET: api/Companies/5
        public IHttpActionResult GetCompany(int id)
        {
            var company = Helper.getCompany(id);

            if (company == null)
            {
                return NotFound();
            }

            return Ok(company);
        }

        // PUT: api/Companies/5
        public IHttpActionResult PutCompany(int id, string sessionkey, Company company)
        {
            if (Helper.isSessionValid(sessionkey) && Helper.isAdmin(sessionkey) || Helper.isCompany(sessionkey))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != company.CompanyID)
                {
                    return BadRequest();
                }

                var _company = Helper.updateCompany(id, company);
                return Ok(_company);
            }
            else
                return Unauthorized();
        }

        // POST: api/Companies
        public IHttpActionResult PostCompany(string sessionkey, Company company)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Company _company = Helper.createCompany(company);
            return Ok(_company);
        }

        // DELETE: api/Companies/5

        public IHttpActionResult DeleteCompany(int id, string sessionkey)
        {
            if (Helper.isSessionValid(sessionkey) && Helper.isAdmin(sessionkey))
            {
                bool result = Helper.removeCompany(id);
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
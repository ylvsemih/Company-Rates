using CompanyRatesAPI.Models;
using System.Web.Http;

namespace CompanyRatesAPI.Controllers
{
    public class EmailsController : ApiController
    {
        // POST: api/Emails
        public IHttpActionResult Post(Email email)
        {
            Helper.sendEmail(email);
            return Ok();
        }
    }
}
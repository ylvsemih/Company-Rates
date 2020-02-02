using CompanyRatesAPI.Models;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace CompanyRatesAPI.Controllers
{
    [EnableCors(origins: "https://companyratesapi.azurewebsites.net", headers: "*", methods: "*")]
    public class AccountsController : ApiController
    {
        [ActionName("login")]
        [ResponseType(typeof(LoginSession))]
        [Route("api/accounts/login")]
        // POST: api/Users/login
        public IHttpActionResult PostLogin(Login _login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (Helper.isAuthenticated(_login.email, _login.passwordHash))
            {
                var session = Helper.createSession(_login.email);

                return Ok(session);
            }
            return NotFound();
        }

        [ActionName("logout")]
        [Route("api/accounts/logout")]
        // POST: api/Users
        public IHttpActionResult PostLogout(LoginSession session)

        {
            if (Helper.removeSession(session.SessionKey))
            {
                return StatusCode(System.Net.HttpStatusCode.NoContent);
            }

            return NotFound();
        }

        [ActionName("removeall")]
        [Route("api/accounts/removeall")]
        // POST: api/Users
        public IHttpActionResult Deleteremoveall()

        {
            Helper.removeCompanyAll();

            return Ok();
        }
    }
}
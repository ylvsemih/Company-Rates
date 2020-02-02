using CompanyRatesAPI.Models;
using System.Web.Http;
using System.Web.Http.Cors;

namespace CompanyRatesAPI.Controllers
{
    [EnableCors(origins: "https://companyratesapi.azurewebsites.net", headers: "*", methods: "*")]
    public class UsersController : ApiController
    {
        // GET: api/Users
        public IHttpActionResult GetUsers(string sessionkey)
        {
            if (Helper.isSessionValid(sessionkey) && Helper.isAdmin(sessionkey))
            {
                return Ok(Helper.GetUsers());
            }
            else
                return Unauthorized();
        }

        // GET: api/Users/5
        //[Route("api/users/{id}")]
        public IHttpActionResult GetUser(int id, string sessionkey)
        {
            if (Helper.isSessionValid(sessionkey))
            {
                var user = Helper.GetUser(id);

                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            else
                return Unauthorized();
        }

        // PUT: api/Users/5

        public IHttpActionResult PutUser(int id, string sessionkey, User user)
        {
            if (Helper.isSessionValid(sessionkey) && Helper.isAdmin(sessionkey) || Helper.isUser(sessionkey))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != user.UserID)
                {
                    return BadRequest();
                }

                var _user = Helper.UpdateUser(id, user);
                return Ok(_user);
            }
            else
                return Unauthorized();
        }

        // DELETE: api/Users/5
        public IHttpActionResult PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!Helper.EmailExists(user.Email.ToLower()))
            {
                User _user = Helper.createUser(user);
                return Ok(_user);
            }

            return BadRequest("Email exists");
        }
    }
}
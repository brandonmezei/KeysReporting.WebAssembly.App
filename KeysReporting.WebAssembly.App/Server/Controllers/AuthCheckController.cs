using KeysReporting.WebAssembly.App.Shared.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KeysReporting.WebAssembly.App.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthCheckController : ControllerBase
    {
        // GET: api/<AuthCheckController>
        [HttpGet]
        public ActionResult<AuthCheck> Get()
        {
            return new AuthCheck { Authorized = true };
        }
    }
}

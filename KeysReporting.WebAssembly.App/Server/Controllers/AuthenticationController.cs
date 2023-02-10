using KeysReporting.WebAssembly.App.Server.Services.LiveVoxAPI;
using KeysReporting.WebAssembly.App.Server.Static;
using KeysReporting.WebAssembly.App.Shared.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KeysReporting.WebAssembly.App.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILiveVoxAPIService _liveVoxAPI;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(ILiveVoxAPIService liveVoxAPI, ILogger<AuthenticationController> logger)
        {
            _liveVoxAPI = liveVoxAPI;
            _logger = logger;
        }

        // POST api/<Authentication>
        [HttpPost]
        public async Task<ActionResult<UserLoginResponseDto>> Post(UserLoginDto userLoginDto)
        {
            try
            {
                return Ok(await _liveVoxAPI.LoginAsync(userLoginDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{Messages.SomethingWentWrong}{nameof(AuthenticationController)}{ex.Message}");
                return Problem($"{Messages.SomethingWentWrong}{nameof(AuthenticationController)}{ex.Message}");
            }
        }
    }
}

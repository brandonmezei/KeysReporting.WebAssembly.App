using KeysReporting.WebAssembly.App.Server.Data;
using KeysReporting.WebAssembly.App.Server.Services.System.FTP;
using KeysReporting.WebAssembly.App.Server.Static;
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
        private readonly IFTPService _ftpService;
        private readonly ILogger<AuthCheckController> _logger;
        public AuthCheckController(IFTPService fTPService, ILogger<AuthCheckController> logger)
        {
            _ftpService = fTPService;
            _logger = logger;
        }

        // GET: api/<AuthCheckController>
        [HttpGet]
        public async Task<ActionResult<AuthCheckDto>> Get()
        {
            try
            {
                await _ftpService.ProcessFileAsync();

                return new AuthCheckDto { Authorized = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{Messages.SomethingWentWrong}{nameof(AuthCheckController)}{ex.Message}");
                return Problem($"{Messages.SomethingWentWrong}{nameof(AuthCheckController)}{ex.Message}");

            }
        }
    }
}

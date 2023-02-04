using KeysReporting.WebAssembly.App.Server.Services.CPHReport;
using KeysReporting.WebAssembly.App.Server.Static;
using KeysReporting.WebAssembly.App.Shared.Auth;
using KeysReporting.WebAssembly.App.Shared.CPH;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KeysReporting.WebAssembly.App.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CPHController : ControllerBase
    {
        private ICPHReport _CPHReport;
        private readonly ILogger<CPHController> _logger;

        public CPHController(ICPHReport CPHReport, ILogger<CPHController> logger) 
        { 
            _CPHReport= CPHReport;
            _logger = logger;
        }


        // GET: api/<CPH>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<CPH>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CPH>
        [HttpPost]
        public async Task<ActionResult<CPHReportDto>> Post(SearchDto searchDto)
        {
            try
            {
                return Ok(await _CPHReport.GetReportAsync(searchDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{Messages.SomethingWentWrong}{nameof(AuthenticationController)}{ex.Message}");
                return Problem($"{Messages.SomethingWentWrong}{nameof(AuthenticationController)}{ex.Message}");
            }
        }

        // PUT api/<CPH>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CPH>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

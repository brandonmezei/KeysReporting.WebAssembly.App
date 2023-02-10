using KeysReporting.WebAssembly.App.Server.Services.Lists;
using KeysReporting.WebAssembly.App.Server.Static;
using KeysReporting.WebAssembly.App.Shared.Lists;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KeysReporting.WebAssembly.App.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SourceTableController : ControllerBase
    {
        private readonly ILogger<CPHController> _logger;
        private readonly ISourceTableService _sourceTableService;

        public SourceTableController(ILogger<CPHController> logger, ISourceTableService sourceTableService) 
        {
            _logger = logger;
            _sourceTableService = sourceTableService;
        }

        // GET: api/<SourceTableController>
        [HttpGet]
        public async Task<ActionResult<SourceTableListDto>> Get()
        {
            try
            {
                return Ok(await _sourceTableService.GetSourceTableAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{Messages.SomethingWentWrong}{nameof(SourceTableController)}{ex.Message}");
                return Problem($"{Messages.SomethingWentWrong}{nameof(SourceTableController)}{ex.Message}");
            }
        }

        
    }
}

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
    public class AgentController : ControllerBase
    {
        private readonly ILogger<AgentController> _logger;
        private readonly IAgentService _agentService;

        public AgentController(ILogger<AgentController> logger, IAgentService agentService)
        {
            _logger = logger;
            _agentService = agentService;
        }

        [HttpGet]
        public async Task<ActionResult<AgentListDto>> Get()
        {
            try
            {
                return Ok(await _agentService.GetAgentAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{Messages.SomethingWentWrong}{nameof(AgentController)}{ex.Message}");
                return Problem($"{Messages.SomethingWentWrong}{nameof(AgentController)}{ex.Message}");
            }
        }
    }
}

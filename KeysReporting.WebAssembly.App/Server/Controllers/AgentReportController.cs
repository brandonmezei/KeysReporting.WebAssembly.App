using KeysReporting.WebAssembly.App.Server.Services.Reports.AgentReport;
using KeysReporting.WebAssembly.App.Server.Static;
using KeysReporting.WebAssembly.App.Shared.Agent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KeysReporting.WebAssembly.App.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AgentReportController : ControllerBase
    {
        private readonly IAgentReportService _agentReportService;
        private readonly ILogger<AgentReportController> _logger;

        public AgentReportController(IAgentReportService agentReportService, ILogger<AgentReportController> logger)
        {
            _agentReportService = agentReportService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<AgentReportDto>> GetReport(SearchDto searchDto)
        {
            try
            {
                return Ok(await _agentReportService.GetReportAsync(searchDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{Messages.SomethingWentWrong}{nameof(AuthenticationController)}{ex.Message}");
                return Problem($"{Messages.SomethingWentWrong}{nameof(AuthenticationController)}{ex.Message}");
            }
        }
    }
}

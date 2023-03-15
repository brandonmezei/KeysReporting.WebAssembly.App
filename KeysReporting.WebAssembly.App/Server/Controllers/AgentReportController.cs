using KeysReporting.WebAssembly.App.Server.Services.Reports.AgentReport;
using KeysReporting.WebAssembly.App.Server.Static;
using KeysReporting.WebAssembly.App.Shared.Agent;
using KeysReporting.WebAssembly.App.Shared.File;
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

        [HttpPost("GetReportDownload")]
        public async Task<ActionResult<ServiceFileDto>> GetReportDownload(SearchDto searchDto)
        {
            try
            {
                var response = await _agentReportService.GetReportDownloadAsync(searchDto);

                if(response != null)
                    return Ok(new ServiceFileDto() { Name = "AgentReport.xlsx", Content = response });

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{Messages.SomethingWentWrong}{nameof(AuthenticationController)}{ex.Message}");
                return Problem($"{Messages.SomethingWentWrong}{nameof(AuthenticationController)}{ex.Message}");
            }
        }

        [HttpPost("GetReportTotals")]
        public async Task<ActionResult<ServiceFileDto>> GetReportDownloadTotals(SearchDto searchDto)
        {
            try
            {
                var response = await _agentReportService.GetReportDownloadTotalsAsync(searchDto);

                if (response != null)
                    return Ok(new ServiceFileDto() { Name = "AgentReportTotals.xlsx", Content = response });

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{Messages.SomethingWentWrong}{nameof(AuthenticationController)}{ex.Message}");
                return Problem($"{Messages.SomethingWentWrong}{nameof(AuthenticationController)}{ex.Message}");
            }
        }

        [HttpPost("GetReportCompareTotals")]
        public async Task<ActionResult<ServiceFileDto>> GetReportDownloadCompareTotals(SearchDto searchDto)
        {
            try
            {
                var response = await _agentReportService.GetReportDownloadCompareTotalsAsync(searchDto);

                if (response != null)
                    return Ok(new ServiceFileDto() { Name = "AgentReportTotalsCompare.xlsx", Content = response });

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{Messages.SomethingWentWrong}{nameof(AuthenticationController)}{ex.Message}");
                return Problem($"{Messages.SomethingWentWrong}{nameof(AuthenticationController)}{ex.Message}");
            }
        }
    }
}

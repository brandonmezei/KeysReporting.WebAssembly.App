using KeysReporting.WebAssembly.App.Server.Services.Lists;
using KeysReporting.WebAssembly.App.Server.Services.Reports.TermCodes;
using KeysReporting.WebAssembly.App.Server.Static;
using KeysReporting.WebAssembly.App.Shared.Lists;
using KeysReporting.WebAssembly.App.Shared.TermCodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KeysReporting.WebAssembly.App.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TermCodesController : ControllerBase
    {

        private readonly ITermCodeReportService _termCodeReportService;
        private readonly ITermCodeService _termCodeService;
        private readonly ILogger<TermCodesController> _logger;

        public TermCodesController(ITermCodeReportService termCodeReportService, ILogger<TermCodesController> logger,
            ITermCodeService termCodeService)
        {
            _termCodeReportService = termCodeReportService;
            _termCodeService = termCodeService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<TermCodeDto>> Get()
        {
            try
            {
                return Ok(await _termCodeService.GetTermCodesAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{Messages.SomethingWentWrong}{nameof(ProjectController)}{ex.Message}");
                return Problem($"{Messages.SomethingWentWrong}{nameof(ProjectController)}{ex.Message}");
            }
        }

        [HttpPost("TermCodeReport")]
        public async Task<ActionResult<List<TermCodeReportDto>>> GetReport(TermCodeSearchDto searchDto)
        {
            try
            {
                return Ok(await _termCodeReportService.GetReportAsync(searchDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{Messages.SomethingWentWrong}{nameof(TermCodesController)}{ex.Message}");
                return Problem($"{Messages.SomethingWentWrong}{nameof(TermCodesController)}{ex.Message}");
            }
        }

        [HttpPost("TermCodeUpdate")]
        public async Task<ActionResult<List<TermCodeReportDto>>> EditTermCode(TermCodeEditDto editDto)
        {
            try
            {
                return Ok(await _termCodeReportService.UpdateReport(editDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{Messages.SomethingWentWrong}{nameof(TermCodesController)}{ex.Message}");
                return Problem($"{Messages.SomethingWentWrong}{nameof(TermCodesController)}{ex.Message}");
            }
        }

    }
}

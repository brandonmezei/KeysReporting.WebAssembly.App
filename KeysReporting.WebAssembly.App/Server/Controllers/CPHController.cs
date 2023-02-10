using KeysReporting.WebAssembly.App.Server.Services.CPHReport;
using KeysReporting.WebAssembly.App.Server.Static;
using KeysReporting.WebAssembly.App.Shared.Auth;
using KeysReporting.WebAssembly.App.Shared.CPH;
using KeysReporting.WebAssembly.App.Shared.Lists;
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
        private readonly ICPHReportService _CPHReport;
        private readonly ILogger<CPHController> _logger;

        public CPHController(ICPHReportService CPHReport, ILogger<CPHController> logger)
        {
            _CPHReport = CPHReport;
            _logger = logger;
        }


        // GET: api/<CPH>
        [HttpGet("GetProjects")]
        public async Task<ActionResult<ProjectListDto>> GetProjects(DateTime reportDate)
        {
            try
            {
                return Ok(await _CPHReport.GetProjectListAsync(reportDate));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{Messages.SomethingWentWrong}{nameof(AuthenticationController)}{ex.Message}");
                return Problem($"{Messages.SomethingWentWrong}{nameof(AuthenticationController)}{ex.Message}");
            }
        }

        // POST api/<CPH>
        [HttpPost]
        public async Task<ActionResult<CPHReportDto>> GetReport(SearchDto searchDto)
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

        [HttpPost("CreateProject")]
        public async Task<ActionResult<ProjectListDto>> CreateNewCampaign(AddProjectDto addProjectDto)
        {
            try
            {
                return Ok(await _CPHReport.CreateNewProjectAsync(addProjectDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{Messages.SomethingWentWrong}{nameof(AuthenticationController)}{ex.Message}");
                return Problem($"{Messages.SomethingWentWrong}{nameof(AuthenticationController)}{ex.Message}");
            }
        }

        [HttpPost("EditTimeLine")]
        public async Task<ActionResult<CPHReportDto>> EditTimeLine(EditTimeDto EditLine)
        {
            try
            {
                return Ok(await _CPHReport.EditTimeLineAsync(EditLine));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{Messages.SomethingWentWrong}{nameof(AuthenticationController)}{ex.Message}");
                return Problem($"{Messages.SomethingWentWrong}{nameof(AuthenticationController)}{ex.Message}");
            }
        }

        [HttpPost("DeleteProject")]
        public async Task<ActionResult<CPHReportDto>> DeleteProject(DeleteProjectDto deleteProjectDto)
        {
            try
            {
                return Ok(await _CPHReport.DeleteProjectAsync(deleteProjectDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{Messages.SomethingWentWrong}{nameof(AuthenticationController)}{ex.Message}");
                return Problem($"{Messages.SomethingWentWrong}{nameof(AuthenticationController)}{ex.Message}");
            }
        }

        [HttpPost("EditCPH")]
        public async Task<ActionResult<CPHReportDto>> EditCPH(EditCPHDto editCPHDto)
        {
            try
            {
                return Ok(await _CPHReport.EditCPH(editCPHDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{Messages.SomethingWentWrong}{nameof(AuthenticationController)}{ex.Message}");
                return Problem($"{Messages.SomethingWentWrong}{nameof(AuthenticationController)}{ex.Message}");
            }
        }
    }
}

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
    public class ProjectController : ControllerBase
    {
        private readonly ILogger<ProjectController> _logger;
        private readonly IProjectService _projectService;

        public ProjectController(ILogger<ProjectController> logger, IProjectService projectService)
        {
            _logger = logger;
            _projectService = projectService;
        }

        // GET: api/<SourceTableController>
        [HttpGet]
        public async Task<ActionResult<ProjectListDto>> Get()
        {
            try
            {
                return Ok(await _projectService.GetProjectAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{Messages.SomethingWentWrong}{nameof(ProjectController)}{ex.Message}");
                return Problem($"{Messages.SomethingWentWrong}{nameof(ProjectController)}{ex.Message}");
            }
        }
    }
}

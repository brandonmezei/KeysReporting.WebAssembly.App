using KeysReporting.WebAssembly.App.Server.Services.Reports.ApiErrors;
using KeysReporting.WebAssembly.App.Server.Static;
using KeysReporting.WebAssembly.App.Shared.Agent;
using KeysReporting.WebAssembly.App.Shared.ApiError;
using KeysReporting.WebAssembly.App.Shared.VirtualResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KeysReporting.WebAssembly.App.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApiErrorController : ControllerBase
    {
        private readonly IApiErrorService _apiErrorService;
        private readonly ILogger<ApiErrorController> _logger;

        public ApiErrorController(IApiErrorService apiErrorService, ILogger<ApiErrorController> logger)
        {
            _apiErrorService = apiErrorService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<VirtualResponseDto<ApiErrorDto>>> GetReport(QueryParamDto queryParamDto)
        {
            try
            {
                return Ok(await _apiErrorService.GetApiErrorsAsync(queryParamDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{Messages.SomethingWentWrong}{nameof(ApiErrorController)}{ex.Message}");
                return Problem($"{Messages.SomethingWentWrong}{nameof(ApiErrorController)}{ex.Message}");
            }
        }
    }
}

using KeysReporting.WebAssembly.App.Shared.VirtualResponse;
using KeysReporting.WebAssembly.App.Server.Data;
using KeysReporting.WebAssembly.App.Server.Services.VirtualResponse;
using KeysReporting.WebAssembly.App.Shared.ApiError;

namespace KeysReporting.WebAssembly.App.Server.Services.Reports.ApiErrors
{
    public class ApiErrorService : IApiErrorService
    {
        private readonly IVirtualResponseService<Apierror> _virtualResponseService;
        
        public ApiErrorService(IVirtualResponseService<Apierror> virtualResponseService)
        {
            _virtualResponseService = virtualResponseService;
        }

        public Task<VirtualResponseDto<ApiErrorDto>> GetApiErrorsAsync(QueryParamDto queryParm)
        {
            return _virtualResponseService.GetAllAsync<ApiErrorDto>(queryParm);
        }
    }
}

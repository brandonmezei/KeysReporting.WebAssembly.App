using KeysReporting.WebAssembly.App.Shared.VirtualResponse;
using KeysReporting.WebAssembly.App.Shared.ApiError;

namespace KeysReporting.WebAssembly.App.Server.Services.Reports.ApiErrors
{
    public interface IApiErrorService
    {
        Task<VirtualResponseDto<ApiErrorDto>> GetApiErrorsAsync(QueryParamDto queryParm);
    }
}

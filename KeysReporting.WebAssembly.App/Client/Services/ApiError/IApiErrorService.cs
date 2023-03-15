using KeysReporting.WebAssembly.App.Shared.ApiError;
using KeysReporting.WebAssembly.App.Shared.File;
using KeysReporting.WebAssembly.App.Shared.VirtualResponse;

namespace KeysReporting.WebAssembly.App.Client.Services.ApiError
{
    public interface IApiErrorService
    {
        Task<VirtualResponseDto<ApiErrorDto>> GetApiErrorAsync(QueryParamDto queryParamDto);
    }
}

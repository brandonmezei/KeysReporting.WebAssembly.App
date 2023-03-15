using Blazored.LocalStorage;
using KeysReporting.WebAssembly.App.Client.Services.Auth;
using KeysReporting.WebAssembly.App.Client.Services.Base;
using KeysReporting.WebAssembly.App.Shared.ApiError;
using KeysReporting.WebAssembly.App.Shared.VirtualResponse;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace KeysReporting.WebAssembly.App.Client.Services.ApiError
{
    public class ApiErrorService : BaseHttpService, IApiErrorService
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILocalStorageService _localStorage;

        public ApiErrorService(IAuthenticationService authenticationService, ILocalStorageService localStorage, IWebAssemblyHostEnvironment hostEnvironment) : base(localStorage, hostEnvironment)
        {
            _authenticationService = authenticationService;
            _localStorage = localStorage;
        }

        public async Task<VirtualResponseDto<ApiErrorDto>> GetApiErrorAsync(QueryParamDto queryParamDto)
        {
            if (!await _authenticationService.CheckLogin())
                return null;

            await GetBearerToken();

            return await SendRequest<VirtualResponseDto<ApiErrorDto>>("Api/ApiError", HttpMethod.Post, queryParamDto);
        }
    }
}

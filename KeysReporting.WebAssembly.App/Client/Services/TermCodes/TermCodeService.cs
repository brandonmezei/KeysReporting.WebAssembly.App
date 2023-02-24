using Blazored.LocalStorage;
using KeysReporting.WebAssembly.App.Client.Services.Auth;
using KeysReporting.WebAssembly.App.Client.Services.Base;
using KeysReporting.WebAssembly.App.Shared.TermCodes;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace KeysReporting.WebAssembly.App.Client.Services.TermCodes
{
    public class TermCodeService : BaseHttpService, ITermCodeService
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILocalStorageService _localStorage;

        public TermCodeService(IAuthenticationService authenticationService, ILocalStorageService localStorage, IWebAssemblyHostEnvironment hostEnvironment) : base(localStorage, hostEnvironment)
        {
            _authenticationService = authenticationService;
            _localStorage = localStorage;
        }

        public async Task<List<TermCodeReportDto>> GetTermCodeReportAsync(SearchDto searchDto)
        {
            if (!await _authenticationService.CheckLogin())
                return null;

            await GetBearerToken();

            return await SendRequest<List<TermCodeReportDto>>("Api/TermCodes/TermCodeReport", HttpMethod.Post, searchDto);
        }
    }
}

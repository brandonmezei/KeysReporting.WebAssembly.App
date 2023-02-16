using Blazored.LocalStorage;
using KeysReporting.WebAssembly.App.Client.Services.Auth;
using KeysReporting.WebAssembly.App.Client.Services.Base;
using KeysReporting.WebAssembly.App.Shared.Agent;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace KeysReporting.WebAssembly.App.Client.Services.Agent
{
    public class AgentReportService : BaseHttpService, IAgentReportService
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILocalStorageService _localStorage;

        public AgentReportService(IAuthenticationService authenticationService, ILocalStorageService localStorage, IWebAssemblyHostEnvironment hostEnvironment) : base(localStorage, hostEnvironment)
        {
            _authenticationService = authenticationService;
            _localStorage = localStorage;
        }

        public async Task<AgentReportDto> GetAgentReportAsync(SearchDto searchDto)
        {
            if (!await _authenticationService.CheckLogin())
                return null;

            await GetBearerToken();

            return await SendRequest<AgentReportDto>("Api/AgentReport", HttpMethod.Post, searchDto);
        }
    }
}

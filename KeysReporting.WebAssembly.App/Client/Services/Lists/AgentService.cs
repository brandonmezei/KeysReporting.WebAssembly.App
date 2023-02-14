using Blazored.LocalStorage;
using KeysReporting.WebAssembly.App.Client.Services.Auth;
using KeysReporting.WebAssembly.App.Client.Services.Base;
using KeysReporting.WebAssembly.App.Shared.Lists;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace KeysReporting.WebAssembly.App.Client.Services.Lists
{
    public class AgentService : BaseHttpService, IAgentService
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILocalStorageService _localStorage;

        public AgentService(IAuthenticationService authenticationService, ILocalStorageService localStorage, IWebAssemblyHostEnvironment hostEnvironment) : base(localStorage, hostEnvironment)
        {
            _authenticationService = authenticationService;
            _localStorage = localStorage;
        }

        public async Task<List<AgentListDto>> GetAgentAsync()
        {
            if (!await _authenticationService.CheckLogin())
                return null;

            await GetBearerToken();

            return await SendRequest<List<AgentListDto>>("Api/Agent", HttpMethod.Get);
        }
    }
}

using Blazored.LocalStorage;
using KeysReporting.WebAssembly.App.Client.Services.Base;
using KeysReporting.WebAssembly.App.Shared.Auth;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using static System.Net.WebRequestMethods;

namespace KeysReporting.WebAssembly.App.Client.Services.Auth
{
    public class Authentication : BaseHttpService, IAuthentication
    {
        private readonly ILocalStorageService _localStorage;

        public Authentication(ILocalStorageService localStorage, IWebAssemblyHostEnvironment hostEnvironment) : base(localStorage, hostEnvironment)
        {
            _localStorage = localStorage;
        }

        public async Task<bool> LogIn(UserLoginDto userLoginDto)
        {
            var response = await SendRequest<UserLoginResponseDto>("Api/Authentication", HttpMethod.Post, userLoginDto);

            await _localStorage.SetItemAsync("AccessToken", response.Token);

            return !string.IsNullOrEmpty(response.Token);
        }
    }
}

using Blazored.LocalStorage;
using KeysReporting.WebAssembly.App.Client.Providers;
using KeysReporting.WebAssembly.App.Client.Services.Base;
using KeysReporting.WebAssembly.App.Shared.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace KeysReporting.WebAssembly.App.Client.Services.Auth
{
    public class AuthenticationService : BaseHttpService, IAuthenticationService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AuthenticationService(ILocalStorageService localStorage, IWebAssemblyHostEnvironment hostEnvironment,
            AuthenticationStateProvider authenticationStateProvider) : base(localStorage, hostEnvironment)
        {
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;
        }
        public async Task<bool> LogIn(UserLoginDto userLoginDto)
        {
            var response = await SendRequest<UserLoginResponseDto>("Api/Authentication", HttpMethod.Post, userLoginDto);

            if (response != null)
            {
                await _localStorage.SetItemAsync("AccessToken", response.Token);

                //Change auth state of app
                await ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedIn();

                return !string.IsNullOrEmpty(response.Token);
            }

            return false;
        }

        public async Task Logout()
        {
            await ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedOut();
        }

        public async Task<bool> CheckLogin()
        {
            var response = await _authenticationStateProvider.GetAuthenticationStateAsync();

            await GetBearerToken();

            var responseDto = await SendRequest<AuthCheckDto>("Api/AuthCheck", HttpMethod.Get);

            if(!response.User.Claims.Any() || responseDto == null || !responseDto.Authorized)
            {
                await Logout();
                return false;
            }

            return true;
        }

    }
}

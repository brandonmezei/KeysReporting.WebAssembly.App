using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace KeysReporting.WebAssembly.App.Client.Providers
{
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly JwtSecurityTokenHandler _JwtSecurityTokenHandler;

        public ApiAuthenticationStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
            _JwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity());
            var savedToken = await _localStorage.GetItemAsync<string>("AccessToken");

            if (savedToken == null)
                return new AuthenticationState(user);

            var tokenContent = _JwtSecurityTokenHandler.ReadJwtToken(savedToken);

            if (tokenContent.ValidTo < DateTime.Now)
                return new AuthenticationState(user);

            var claims = await GetClaims();

            user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));

            return new AuthenticationState(user);
        }

        public async Task LoggedIn()
        {
            var claims = await GetClaims();
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
            var authStat = Task.FromResult(new AuthenticationState(user));

            NotifyAuthenticationStateChanged(authStat);
        }

        public async Task LoggedOut()
        {
            await _localStorage.RemoveItemAsync("AccessToken");
            var nobody = new ClaimsPrincipal(new ClaimsIdentity());
            var authStat = Task.FromResult(new AuthenticationState(nobody));

            NotifyAuthenticationStateChanged(authStat);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var savedToken = await _localStorage.GetItemAsync<string>("AccessToken");
            var tokenContent = _JwtSecurityTokenHandler.ReadJwtToken(savedToken);

            var claims = tokenContent.Claims.ToList();
            claims.Add(new Claim(ClaimTypes.Name, tokenContent.Subject));
            return claims;
        }
    }
}

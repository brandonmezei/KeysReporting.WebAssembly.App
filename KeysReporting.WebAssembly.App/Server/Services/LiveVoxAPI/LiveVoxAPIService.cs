using Azure.Core;
using KeysReporting.WebAssembly.App.Shared.Auth;
using Microsoft.Identity.Client;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;
using KeysReporting.WebAssembly.App.Server.Models.API;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using KeysReporting.WebAssembly.App.Server.Static;

namespace KeysReporting.WebAssembly.App.Server.Services.LiveVoxAPI
{

    public class LiveVoxAPIService : ILiveVoxAPIService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        private readonly string? _apiLink;
        private readonly string? _AccessToken;
        private readonly JsonSerializerOptions _jsonSetting = new JsonSerializerOptions { WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, PropertyNameCaseInsensitive = true };

        public LiveVoxAPIService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;

            _apiLink = _configuration["APIConfig:Link"];
            _AccessToken = _configuration["APIConfig:Token"];
        }

        private async Task<T> SendRequest<T>(string url, string session = null, object contentPayload = null)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(contentPayload == null ? HttpMethod.Get : HttpMethod.Post, url);

            request.Headers.Add("LV-Access", _AccessToken);

            if (!string.IsNullOrEmpty(session))
                request.Headers.Add("LV-Session", session);

            if (contentPayload != null)
            {
#if DEBUG
                var jsonContent = JsonSerializer.Serialize(contentPayload, _jsonSetting);
#endif
                request.Content = new StringContent(JsonSerializer.Serialize(contentPayload, _jsonSetting), Encoding.UTF8, "application/json");
            }

            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync(), _jsonSetting);
            }
            else
            {
                return default;
            }
        }

        public async Task<UserLoginResponseDto> LoginAsync(UserLoginDto userLoginDto)
        {
            var response = await SendRequest<APILogInResponse>($@"{_apiLink}/session/login", null, userLoginDto);

            if (response != null && !string.IsNullOrEmpty(response.sessionId))
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, response.userId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, response.sessionId.ToString()),
                    new Claim(CustomClaimTypes.Uid, response.userId.ToString())
                };

                var newToken = new JwtSecurityToken(
                    issuer: _configuration["JwtSettings:Issuer"],
                    audience: _configuration["JwtSettings:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddDays(Convert.ToInt32(_configuration["JwtSettings:DurationDays"])),
                    signingCredentials: credentials
                );

                return new UserLoginResponseDto { Token = new JwtSecurityTokenHandler().WriteToken(newToken) };
            }

            return new UserLoginResponseDto { Token = string.Empty };
        }
    }
}

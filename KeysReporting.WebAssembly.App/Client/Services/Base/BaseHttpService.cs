using Blazored.LocalStorage;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace KeysReporting.WebAssembly.App.Client.Services.Base
{
    public class BaseHttpService
    {
        private readonly HttpClient _client;
        private readonly ILocalStorageService _localStorage;
        private readonly IWebAssemblyHostEnvironment _hostEnvironment;

        public BaseHttpService(ILocalStorageService localStorage, IWebAssemblyHostEnvironment hostEnvironment)
        {
            _client = new HttpClient();
            _localStorage = localStorage;
            _hostEnvironment = hostEnvironment;
        }

        protected async Task<T> SendRequest<T>(string url, HttpMethod httpMethod, object contentPayload = null)
        {
            var request = new HttpRequestMessage(httpMethod, $"{_hostEnvironment.BaseAddress}{url}");

            if (contentPayload != null)
            {
#if DEBUG
                var jsonContent = JsonSerializer.Serialize(contentPayload);
#endif
                request.Content = new StringContent(JsonSerializer.Serialize(contentPayload), Encoding.UTF8, "application/json");
            }

            var response = await _client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                return default;
            }
        }

        protected async Task GetBearerToken()
        {
            var token = await _localStorage.GetItemAsync<string>("AccessToken");

            if (token != null)
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

        }
    }
}

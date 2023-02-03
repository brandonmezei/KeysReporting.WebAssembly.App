using Microsoft.Identity.Client;
using System.Net.Http;
using System.Net;

namespace KeysReporting.WebAssembly.App.Server.Providers
{
    public class HttpFactoryWithProxy : IHttpClientFactory
    {
        private static HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpFactoryWithProxy(IConfiguration configuration)
        {
            _configuration = configuration;

            if (_httpClient == null)
            {
#if DEBUG
                _httpClient = new HttpClient();
#else
                var proxy = new WebProxy
                {
                    Address = new Uri(_configuration["APIConfig:Proxy"]),
                    BypassProxyOnLocal = true

                };

                // Now create a client handler which uses that proxy
                var httpClientHandler = new HttpClientHandler
                {
                    Proxy = proxy,
                };

                _httpClient = new HttpClient(handler: httpClientHandler);
#endif
            }


        }

        public HttpClient CreateClient(string name)
        {
            return _httpClient;
        }
    }
}

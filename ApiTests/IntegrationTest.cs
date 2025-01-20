using System;
using System.Net.Http;

namespace ApiTests
{
    public class IntegrationTest : IDisposable
    {
        private HttpClient? _httpClient;

        protected HttpClient HttpClient
        {
            get
            {
                if (_httpClient == default)
                {
                    // Set up a custom HttpClientHandler to bypass SSL validation
                    var handler = new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
                    };

                    _httpClient = new HttpClient(handler)
                    {
                        // Update the base address if necessary
                        BaseAddress = new Uri("https://localhost:7124")
                    };
                    _httpClient.DefaultRequestHeaders.Add("accept", "text/plain");
                }

                return _httpClient;
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}

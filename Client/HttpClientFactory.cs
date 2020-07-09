using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Client
{
    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly ClientOptions _clientOptions;

        public HttpClientFactory(IOptions<ClientOptions> optionsAccessor)
        {
            _clientOptions = optionsAccessor?.Value ?? new ClientOptions();
        }

        public HttpClient CreateHttpClient()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(_clientOptions.BaseUri),
                Timeout = TimeSpan.FromSeconds(_clientOptions.Timeout)
            };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
    }
}

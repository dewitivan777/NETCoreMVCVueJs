using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    public abstract class RESTResourceClientBase
    {
        protected readonly HttpClient _httpClient;

        public RESTResourceClientBase(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        protected async Task<IResponse<T>> SendAsync<T>(
            HttpRequestMessage httpRequestMessage,
            CancellationToken cancellationToken = default)
        {
            HttpResponseMessage httpResponseMessage;

            try
            {
                httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage, cancellationToken);
            }
            catch (Exception ex)
            {
                return new Response<T>(ex);
            }

            string raw = null;
            if (httpResponseMessage.Content != null)
            {
                raw = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            }

            return new Response<T>(
                raw,
                httpResponseMessage.StatusCode,
                httpResponseMessage.ReasonPhrase,
                httpResponseMessage.Headers);
        }
    }
}
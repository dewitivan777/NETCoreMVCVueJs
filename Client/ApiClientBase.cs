using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    public abstract class ApiClientBase
    {
        protected readonly HttpClient _client;
        private readonly IHttpContextAccessor _contextAccessor;

        public ApiClientBase(HttpClient client, IHttpContextAccessor contextAccessor)
        {
            _client = client;
            _contextAccessor = contextAccessor;
        }

        protected async Task<IResponse<T>> SendRequestAsync<T>(
            HttpRequestMessage httpRequestMessage,
            string token = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            HttpResponseMessage httpResponseMessage;

            try
            {
                httpResponseMessage = await _client.SendAsync(httpRequestMessage, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                //await _notifier.Notify(ex);
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

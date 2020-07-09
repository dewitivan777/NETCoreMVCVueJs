using Client;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBlocksWeb.Handlers
{
    public class CommandHandler<T> : ICommandHandler<T> where T : class
    {
        private readonly IApiClient _client;
        private readonly ClientOptions _options;

        public CommandHandler(IApiClient client, IOptions<ClientOptions> options)
        {
            _options = options.Value;
            _client = client;
        }


        public async Task<IResponse<T>> DeleteAsync(
            string path,
            string id)
        {
            string requestUri;

            if (!string.IsNullOrWhiteSpace(id))
            {
                requestUri = $"{path}/{id}";
            }
            else
            {
                requestUri = path;
            }

            return await _client.DeleteAsync<T>(requestUri);
        }

        public async Task<IResponse<T>> PostAsync<TContent>(
            string path,
            TContent content) where TContent : class
        {
            string requestUri = path;

            return await _client.CreateAsync<T, TContent>(requestUri, content);
        }

        public async Task<IResponse<T>> PutAsync<TContent>(
            string path,
            TContent content,
            string id) where TContent : class
        {
            string requestUri;

            if (!string.IsNullOrWhiteSpace(id))
            {
                requestUri = $"{path}/{id}";
            }
            else
            {
                requestUri = path;
            }

            return await _client.EditAsync<T, TContent>(requestUri, content);
        }
    }
}

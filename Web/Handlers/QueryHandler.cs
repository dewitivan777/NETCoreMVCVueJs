using Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eBlocksWeb.Handlers
{
    public class QueryHandler<TDomain> : IQueryHandler<TDomain>
        where TDomain : class
    {
        protected readonly IApiClient _client;

        public QueryHandler(IApiClient client)
        {
            _client = client;
        }

        public async Task<IResponse<IEnumerable<TDomain>>> GetAllAsync(string requestUri)
        {
            var response = _client.GetAsync<IEnumerable<TDomain>>(requestUri);

            return await response;
        }

        public async Task<IResponse<IEnumerable<TEntity>>> GetAllAsync<TEntity>(string requestUri)
        {
            var response = _client.GetAsync<IEnumerable<TEntity>>(requestUri);

            return await response;
        }

        public async Task<IResponse<TDomain>> GetAsync(string requestUri, string token = null)
        {
            var response = _client.GetAsync<TDomain>(requestUri, token: token);

            return await response;
        }

        public async Task<IResponse<TEntity>> GetAsync<TEntity>(string requestUri)
        {
            var response = _client.GetAsync<TEntity>(requestUri);

            return await response;
        }
    }
}

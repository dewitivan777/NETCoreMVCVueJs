using Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBlocksWeb.Handlers
{
    public interface IQueryHandler<TDomain>
        where TDomain : class
    {
        Task<IResponse<TDomain>> GetAsync(string requestUri, string token = null);
        Task<IResponse<TEntity>> GetAsync<TEntity>(string requestUri);
        Task<IResponse<IEnumerable<TDomain>>> GetAllAsync(string requestUri);
        Task<IResponse<IEnumerable<TEntity>>> GetAllAsync<TEntity>(string requestUri);
    }
}

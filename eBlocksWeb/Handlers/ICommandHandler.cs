using Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBlocksWeb.Handlers
{
    public interface ICommandHandler<T> where T : class
    {
        Task<IResponse<T>> DeleteAsync(string path, string id);
        Task<IResponse<T>> PostAsync<TContent>(string path, TContent content) where TContent : class;
        Task<IResponse<T>> PutAsync<TContent>(string path, TContent content, string id) where TContent : class;

    }
}

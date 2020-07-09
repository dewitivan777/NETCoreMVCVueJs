using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client
{
    /// <summary>
    /// Just duplicating ApiClient to allow call to non-jma REST clients
    /// with this TODO: add an option to add jma headers on demand on current handlers so they can be reused outside jma domain
    /// </summary>
    public interface IRESTResourceClient
    {
        Task<IResponse<T>> PostAsync<T, TContent>(
            string uri,
            TContent content,
            AuditInfo audit = null,
            string token = null,
            IDictionary<string, string> headers = null) where TContent : class;

        Task<IResponse<T>> DeleteAsync<T>(string uri,
            AuditInfo audit = null,
             string token = null,
             IDictionary<string, string> headers = null);

        Task<IResponse<T>> PutAsync<T, TContent>(
            string uri,
            TContent content,
            AuditInfo audit = null,
            string token = null,
            IDictionary<string, string> headers = null) where TContent : class;

        Task<IResponse<T>> GetAsync<T>(
            string uri,
            AuditInfo audit = null,
            string token = null,
            IDictionary<string, string> headers = null);
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client
{
    public interface IApiClient
    {
        Task<IResponse<List<string>>> UploadAsync(
            string uri,
            List<byte[]> files,
            AuditInfo audit = null,
            string token = null);

        Task<IResponse<T>> CreateAsync<T, TContent>(
            string uri,
            TContent content,
            AuditInfo audit = null,
            string token = null) where TContent : class;

        Task<IResponse<T>> DeleteAsync<T>(string uri,
            AuditInfo audit = null,
             string token = null);

        Task<IResponse<T>> EditAsync<T, TContent>(
            string uri,
            TContent content,
            AuditInfo audit = null,
            string token = null) where TContent : class;

        Task<IResponse<T>> GetAsync<T>(
            string uri,
            AuditInfo audit = null,
            string token = null);
    }
}
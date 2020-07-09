using Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBlocksWeb.Handlers
{
    public interface ICommandHandler<T> where T : class
    {
        Task<IResponse<List<string>>> UploadAsync(List<byte[]> files, string userId, AuditInfo audit = null);
        Task<IResponse<T>> DeleteAsync(string id, AuditInfo audit = null);
        Task<IResponse<T>> PostAsync<TContent>(TContent content, AuditInfo audit = null) where TContent : class;
        Task<IResponse<T>> PutAsync<TContent>(TContent content, string id, AuditInfo audit = null) where TContent : class;
        Task<IResponse<T>> DeleteAsync(string path, string id, AuditInfo audit = null);
        Task<IResponse<T>> DeleteMergeAsync(string path, string id, string mergeid, string type, AuditInfo audit = null);
        Task<IResponse<T>> DeleteMergeAsync(string path, string id, string currentType, string mergeid, string type, AuditInfo audit = null);
        Task<IResponse<T>> PostAsync<TContent>(string path, TContent content, AuditInfo audit = null) where TContent : class;
        Task<IResponse<T>> PostAsync<TContent>(string path, TContent content, string id, AuditInfo audit = null) where TContent : class;
        Task<IResponse<T>> PutAsync<TContent>(string path, TContent content, string id, AuditInfo audit = null) where TContent : class;

        Task<IResponse<TResult>> PostAsync<TResult, TContent>(string path, TContent content, AuditInfo audit = null)
            where TResult : class
            where TContent : class;
    }
}

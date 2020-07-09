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

        public async Task<IResponse<T>> DeleteAsync(string id, AuditInfo audit = null)
        {
            var requestUri = $"{typeof(T).Name}/{id}";

            return await _client.DeleteAsync<T>(requestUri, audit);
        }

        public async Task<IResponse<T>> DeleteAsync(
            string path,
            string id,
            AuditInfo audit = null)
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

            return await _client.DeleteAsync<T>(requestUri, audit);
        }

        public async Task<IResponse<T>> DeleteMergeAsync(
        string path,
        string id,
        string mergeid,
        string type,
        AuditInfo audit = null)
        {
            string requestUri;

            if (!string.IsNullOrWhiteSpace(id))
            {
                requestUri = $"{path}/{id}/{mergeid}/{type}";
            }
            else
            {
                requestUri = path;
            }

            return await _client.DeleteAsync<T>(requestUri, audit);
        }

        public async Task<IResponse<T>> DeleteMergeAsync(
        string path,
        string id,
        string currentParent,
        string mergeid,
        string type,
        AuditInfo audit = null)
        {
            string requestUri;

            if (!string.IsNullOrWhiteSpace(id))
            {
                requestUri = $"{path}/{id}/{currentParent}/{mergeid}/{type}";
            }
            else
            {
                requestUri = path;
            }

            return await _client.DeleteAsync<T>(requestUri, audit);
        }

        public async Task<IResponse<List<string>>> UploadAsync(List<byte[]> files, string userId, AuditInfo audit = null)
        {
            var requestUri = $"{_options.ImageAPIBaseUri.TrimEnd('/')}/storage/{userId}/images";

            var response = await _client.UploadAsync(requestUri, files, audit);

            if (response.IsError)
            {
                return response;
            }

            var fileUris = new List<string>();

            foreach (string fileId in response.Content)
            {
                if (fileId.IndexOf("http", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    fileUris.Add(fileId);
                }
                else
                {
                    var fileUri = $"{_options.ImageAPIBaseUri.TrimEnd('/')}/storage/{userId}/images/{fileId}";
                    fileUris.Add(fileUri);
                }
            }

            return new Response<List<string>>(fileUris, response.HttpRawContent, response.HttpStatusCode);
        }

        public async Task<IResponse<T>> PostAsync<TContent>(TContent content, AuditInfo audit = null) where TContent : class
        {
            var requestUri = $"{typeof(T).Name}";

            return await _client.CreateAsync<T, TContent>(requestUri, content, audit);
        }

        public async Task<IResponse<T>> PostAsync<TContent>(string path, TContent content, AuditInfo audit = null) where TContent : class
        {
            return await _client.CreateAsync<T, TContent>(path, content, audit);
        }

        public async Task<IResponse<T>> PostAsync<TContent>(
            string path,
            TContent content,
            string id,
            AuditInfo audit = null) where TContent : class
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

            return await _client.CreateAsync<T, TContent>(requestUri, content, audit);
        }

        public async Task<IResponse<T>> PutAsync<TContent>(TContent content, string id, AuditInfo audit = null) where TContent : class
        {
            var requestUri = $"{typeof(T).Name}/{id}";

            return await _client.EditAsync<T, TContent>(requestUri, content, audit);
        }

        public async Task<IResponse<T>> PutAsync<TContent>(
            string path,
            TContent content,
            string id,
            AuditInfo audit = null) where TContent : class
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

            return await _client.EditAsync<T, TContent>(requestUri, content, audit);
        }

        public async Task<IResponse<TResult>> PostAsync<TResult, TContent>(
            string path,
            TContent content,
            AuditInfo audit = null)
            where TResult : class
            where TContent : class
        {
            return await _client.CreateAsync<TResult, TContent>(path, content, audit);
        }
    }
}

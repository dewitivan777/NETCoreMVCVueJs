using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ApiClient : ApiClientBase, IApiClient
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public ApiClient(
            HttpClient client,
            IHttpContextAccessor contextAccessor) : base(client, contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public async Task<IResponse<List<string>>> UploadAsync(
            string uri,
            List<byte[]> files,
            AuditInfo audit = null,
            string token = null)
        {
            var form = new MultipartFormDataContent();

            foreach (var file in files)
            {
                var content = new ByteArrayContent(file);
                form.Add(content, Guid.NewGuid().ToString(), Guid.NewGuid() + ".jpeg");
            }

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri) { Content = form };
            var response = await SendRequestAsync<List<string>>(httpRequestMessage, token);

            if (response.IsError)
            {

            }

            return response;
        }

        public async Task<IResponse<T>> CreateAsync<T, TContent>(
            string uri,
            TContent content,
            AuditInfo audit = null,
            string token = null) where TContent : class
        {
            if (content is string) return new Response<T>(new ArgumentException("value can not be string", nameof(content)));

            //To catch json data for debugging
            string jsonData = content.ToJsonString();

            var data = new StringContent(
                content: jsonData,
                encoding: Encoding.UTF8,
                mediaType: "application/json");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = data
            };

            var response = await SendRequestAsync<T>(httpRequestMessage, token);

            if (response.IsError)
            {

            }

            return response;
        }

        public async Task<IResponse<T>> DeleteAsync<T>(
            string uri,
            AuditInfo audit = null,
            string token = null)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            var response = await SendRequestAsync<T>(httpRequestMessage, token);

            if (response.IsError)
            {

            }

            return response;
        }

        public async Task<IResponse<T>> EditAsync<T, TContent>(
            string uri,
            TContent content,
            AuditInfo audit = null,
            string token = null) where TContent : class
        {
            if (content is string) return new Response<T>(new ArgumentException("value can not be string", nameof(content)));

            //For debugging
            string jsonContent = content.ToJsonString();

            var data = new StringContent(
               content: jsonContent,
               encoding: Encoding.UTF8,
               mediaType: "application/json");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, uri)
            {
                Content = data
            };

            var response = await SendRequestAsync<T>(httpRequestMessage, token);

            if (response.IsError)
            {

            }

            return response;
        }

        public async Task<IResponse<T>> GetAsync<T>(
            string uri,
            AuditInfo audit = null,
             string token = null)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await SendRequestAsync<T>(httpRequestMessage, token);

            if (response.IsError)
            {

            }

            return response;
        }
    }
}
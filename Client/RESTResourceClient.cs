using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    /// <summary>
    /// Just duplicating ApiClient to allow call to non-jma REST clients
    /// with TODO: add an option to add jma headers on demand on current handlers so they can be reused outside jma calls
    /// </summary>
    public class RESTResourceClient : RESTResourceClientBase, IRESTResourceClient
    {
        public RESTResourceClient(HttpClient httpClient) : base(httpClient) { }

        public async Task<IResponse<T>> PostAsync<T, TContent>(
            string uri,
            TContent content,
            AuditInfo audit = null,
            string token = null,
            IDictionary<string, string> headers = null) where TContent : class
        {
            if (content is string) return new Response<T>(new ArgumentException("value can not be string", nameof(content)));

            string jsonData = content.ToJsonString();

            var data = new StringContent(
                content: jsonData,
                encoding: Encoding.UTF8,
                mediaType: "application/json");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = data
            };

            if (!string.IsNullOrWhiteSpace(token))
            {
                httpRequestMessage.AddBearerAuthorization(token);
            }

            if (audit != null)
            {
                httpRequestMessage.AddAuditInfo(audit);
            }

            if (headers != null)
            {
                httpRequestMessage.MergeHeaders(headers);
            }

            var response = await SendAsync<T>(httpRequestMessage);

            return response;
        }

        public async Task<IResponse<T>> DeleteAsync<T>(
            string uri,
            AuditInfo audit = null,
            string token = null,
            IDictionary<string, string> headers = null)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            if (!string.IsNullOrWhiteSpace(token))
            {
                httpRequestMessage.AddBearerAuthorization(token);
            }

            if (audit != null)
            {
                httpRequestMessage.AddAuditInfo(audit);
            }

            if (headers != null)
            {
                httpRequestMessage.MergeHeaders(headers);
            }

            var response = await SendAsync<T>(httpRequestMessage);

            return response;
        }

        public async Task<IResponse<T>> PutAsync<T, TContent>(
            string uri,
            TContent content,
            AuditInfo audit = null,
            string token = null,
            IDictionary<string, string> headers = null) where TContent : class
        {
            if (content is string) return new Response<T>(new ArgumentException("value can not be string", nameof(content)));

            string jsonContent = content.ToJsonString();

            var data = new StringContent(
               content: jsonContent,
               encoding: Encoding.UTF8,
               mediaType: "application/json");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, uri)
            {
                Content = data
            };

            if (!string.IsNullOrWhiteSpace(token))
            {
                httpRequestMessage.AddBearerAuthorization(token);
            }

            if (audit != null)
            {
                httpRequestMessage.AddAuditInfo(audit);
            }

            if (headers != null)
            {
                httpRequestMessage.MergeHeaders(headers);
            }

            var response = await SendAsync<T>(httpRequestMessage);

            return response;
        }

        public async Task<IResponse<T>> GetAsync<T>(
            string uri,
            AuditInfo audit = null,
            string token = null,
            IDictionary<string, string> headers = null)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            if (!string.IsNullOrWhiteSpace(token))
            {
                httpRequestMessage.AddBearerAuthorization(token);
            }

            if (audit != null)
            {
                httpRequestMessage.AddAuditInfo(audit);
            }

            if (headers != null)
            {
                httpRequestMessage.MergeHeaders(headers);
            }

            var response = await SendAsync<T>(httpRequestMessage);

            return response;
        }
    }
}

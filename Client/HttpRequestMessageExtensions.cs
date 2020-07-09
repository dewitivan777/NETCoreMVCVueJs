using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Client
{
    public static class HttpRequestMessageExtensions
    {
        public static void AddAuditInfo(this HttpRequestMessage httpRequestMessage, AuditInfo auditInfo)
        {
            if (auditInfo != null)
            {
                if (!string.IsNullOrEmpty(auditInfo.Reason))
                {
                    httpRequestMessage.Headers.Add("jma-reason", auditInfo.Reason);
                }

                if (!string.IsNullOrEmpty(auditInfo.Message))
                {
                    httpRequestMessage.Headers.Add("jma-message", auditInfo.Message);
                }

                if (auditInfo.DontSendNotifications)
                {
                    httpRequestMessage.Headers.Add("jma-notify", "false");
                }
            }
        }

        public static void AddBearerAuthorization(this HttpRequestMessage httpRequestMessage, string token)
        {
            if (!string.IsNullOrWhiteSpace(token))
            {
                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public static void MergeHeaders(this HttpRequestMessage httpRequestMessage, IDictionary<string, string> headers)
        {
            foreach (var header in headers)
            {
                httpRequestMessage.Headers.Add(header.Key, header.Value);
            }
        }
    }
}

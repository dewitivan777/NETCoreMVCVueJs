using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;

namespace Client
{
    public abstract class Response
    {
        public bool IsError { get; protected set; }
        public HttpStatusCode HttpStatusCode { get; }
        public string HttpReasonPhrase { get; }
        public string HttpRawContent { get; }
        public Exception Exception { get; protected set; }
        public ResponseError ResponseError { get; protected set; }
            = ResponseError.None;

        /// <summary>
        /// To relay response headers
        /// 
        /// </summary>
        public HttpResponseHeaders Headers { get; }

        protected Response(
            string raw,
            HttpStatusCode statusCode,
            string reason = null,
            HttpResponseHeaders headers = null)
        {
            HttpStatusCode = statusCode;

            if (statusCode != HttpStatusCode.OK)
            {
                IsError = true;
                ResponseError = ResponseError.Http;

                if (!string.IsNullOrWhiteSpace(raw))
                {
                    HttpReasonPhrase = raw;
                }
                else
                {
                    HttpReasonPhrase = reason;
                }
            }

            HttpRawContent = raw;
            Headers = headers;
        }

        protected Response(Exception ex)
        {
            IsError = true;
            Exception = ex;
            ResponseError = ResponseError.Exception;
        }

        public string Error
        {
            get
            {
                if (ResponseError == ResponseError.Http) return HttpReasonPhrase;
                if (ResponseError == ResponseError.Exception) return Exception.Message;
                return string.Empty;
            }
        }
    }

    public class Response<T> :
        Response,
        IResponse<T>
    {
        public Response(Exception ex) : base(ex)
        {
        }

        public Response(
            string raw,
            HttpStatusCode statusCode,
            string reason = null,
            HttpResponseHeaders headers = null) : base(raw, statusCode, reason, headers)
        {
            if (ResponseError != ResponseError.None)
                return;

            try
            {
                Content = raw.ToTypedObject<T>();
            }
            catch (Exception ex)
            {
                IsError = true;
                Exception = ex;
                ResponseError = ResponseError.Exception;
            }
        }

        public Response(
            T content,
            string raw,
            HttpStatusCode statusCode) : base(raw, statusCode)
        {
            Content = content;
        }

        public T Content { get; }
    }
}

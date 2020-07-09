using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;

namespace Client
{
    public interface IResponse<T>
    {
        T Content { get; }
        string Error { get; }
        bool IsError { get; }
        HttpStatusCode HttpStatusCode { get; }
        string HttpReasonPhrase { get; }
        string HttpRawContent { get; }
        Exception Exception { get; }
        ResponseError ResponseError { get; }

        /// <summary>
        /// To relay response headers
        /// 
        /// </summary>
        HttpResponseHeaders Headers { get; }
    }
}

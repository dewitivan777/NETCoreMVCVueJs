using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Client
{
    public static class HttpContextExtensions
    {
        public const string NullIPv6Address = "::1";

        public static string ToUrlString(this HttpRequest request)
        {
            return $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";
        }

        public static Uri ToUri(this HttpRequest request)
        {
            return new Uri(request.ToUrlString());
        }

        //From https://stackoverflow.com/a/36316189/392779
        public static string GetRequestIP(this HttpContext context, bool tryUseXForwardHeader = true)
        {
            string ip = null;

            // todo support new "Forwarded" header (2014) https://en.wikipedia.org/wiki/X-Forwarded-For

            // X-Forwarded-For (csv list):  Using the First entry in the list seems to work
            // for 99% of cases however it has been suggested that a better (although tedious)
            // approach might be to read each IP from right to left and use the first public IP.
            // http://stackoverflow.com/a/43554000/538763
            //
            if (tryUseXForwardHeader)
                ip = context.GetHeaderValueAs<string>("X-Forwarded-For").SplitCsv().FirstOrDefault();

            // RemoteIpAddress is always null in DNX RC1 Update1 (bug).
            if (ip.IsNullOrWhitespace() && context?.Connection?.RemoteIpAddress != null)
                ip = context.Connection.RemoteIpAddress.ToString();

            if (ip.IsNullOrWhitespace())
                ip = context.GetHeaderValueAs<string>("REMOTE_ADDR");

            // _httpContextAccessor.HttpContext?.Request?.Host this is the local host.

            if (ip.IsNullOrWhitespace())
                throw new Exception("Unable to determine caller's IP.");

            if (!context.IsLocal())
            {
                ip = ip.Split(':')[0];
            }

            return ip;
        }

        private static T GetHeaderValueAs<T>(this HttpContext context, string headerName)
        {

            if (context?.Request?.Headers?.TryGetValue(headerName, out StringValues values) ?? false)
            {
                string rawValues = values.ToString();   // writes out as Csv when there are multiple.

                if (!string.IsNullOrEmpty(rawValues))
                {
                    return (T)Convert.ChangeType(values.ToString(), typeof(T));
                }
            }
            return default;
        }

        private static List<string> SplitCsv(this string csvList, bool nullOrWhitespaceInputReturnsNull = false)
        {
            if (string.IsNullOrWhiteSpace(csvList))
                return nullOrWhitespaceInputReturnsNull ? null : new List<string>();

            return csvList
                .TrimEnd(',')
                .Split(',')
                .AsEnumerable<string>()
                .Select(s => s.Trim())
                .ToList();
        }

        private static bool IsNullOrWhitespace(this string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }

        public static bool IsLocal(this ConnectionInfo conn)
        {
            if (!conn.RemoteIpAddress.IsSet())
                return true;

            // we have a remote address set up
            // is local is same as remote, then we are local
            if (conn.LocalIpAddress.IsSet())
                return conn.RemoteIpAddress.Equals(conn.LocalIpAddress);

            // else we are remote if the remote IP address is not a loopback address
            return conn.RemoteIpAddress.IsLoopback();
        }

        public static bool IsLocal(this HttpContext context)
        {
            return context.Connection.IsLocal();
        }

        public static bool IsLocal(this HttpRequest request)
        {
            return request.HttpContext.IsLocal();
        }

        public static bool IsSet(this IPAddress address)
        {
            return address != null && address.ToString() != NullIPv6Address;
        }

        public static bool IsLoopback(this IPAddress address)
        {
            return IPAddress.IsLoopback(address);
        }
    }
}

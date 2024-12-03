using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace NetEnhancements.AspNet
{
    public static class HttpContextExtensions
    {
        [DebuggerStepThrough]
        public static string GetRequestIPAddress(this HttpContext context)
        {
            if (context.Request.Headers.TryGetValue(SharedConstants.XForwardedFor, out var forwardedHeader))
            {
                return forwardedHeader.ToString().Split(',').First();
            }

            return context.Connection.RemoteIpAddress?.MapToIPv4().ToString()
                ?? SharedConstants.UnknownIpAddress;

        }
    }
}

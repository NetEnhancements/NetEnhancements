using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace NetEnhancements.AspNet
{
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Returns the IPv4(-mapped from IPv6 when necessary) address of the client making the request. If the address is not available, returns "(Unknown)".
        /// </summary>
        [DebuggerStepThrough]
        public static string GetRequestIPAddress(this HttpContext context)
        {
            return context.Connection.RemoteIpAddress?.MapToIPv4().ToString()
                ?? SharedConstants.UnknownIpAddress;
        }
    }
}

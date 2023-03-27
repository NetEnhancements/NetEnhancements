using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NetEnhancements.Shared.AspNet
{
    public static class BusinessResultToStatusCodeExtensions
    {
        /// <summary>
        /// Returns a 200 OK, 404 Not Found or 500 Internal Server error, corresponding to <paramref name="result"/>'s properties.
        /// </summary>
        public static StatusCodeResult ToStatusCode(this BusinessResult result)
        {
            if (result.Success)
            {
                return new StatusCodeResult((int)HttpStatusCode.OK);
            }

            if (result.NotFound)
            {
                return new StatusCodeResult((int)HttpStatusCode.NotFound);
            }

            if (result.OtherError)
            {
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }

            throw new ArgumentException("Result cannot be translated to an HTTP status code", nameof(result));
        }

        public static StatusCodeResult BusinessResult(this PageModel _, BusinessResult result) => result.ToStatusCode();

        public static StatusCodeResult BusinessResult(this Controller _, BusinessResult result) => result.ToStatusCode();
    }
}

using System.Net;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NetEnhancements.AspNet.Filters
{
    public class UnauthorizedAccessExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is not UnauthorizedAccessException)
            {
                return;
            }

            context.ExceptionHandled = true;

            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
        }
    }
}

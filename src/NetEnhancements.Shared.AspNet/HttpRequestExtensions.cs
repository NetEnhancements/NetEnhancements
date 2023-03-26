using Microsoft.AspNetCore.Http;

namespace NetEnhancements.Shared.AspNet
{
    public static class HttpRequestExtensions
    {
        public static Guid? GetOrganizationIdFromRoute(this HttpRequest request)
        {
            return Guid.TryParse(request.RouteValues[SharedConstants.OrganizationIdName]?.ToString(), out var organizationGuid)
                ? organizationGuid
                : null;
        }
    }
}

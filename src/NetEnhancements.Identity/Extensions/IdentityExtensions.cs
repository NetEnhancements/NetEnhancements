using System.Security.Claims;
using System.Security.Principal;

namespace NetEnhancements.Identity.Extensions
{
    public static class IdentityExtensions
    {
        /// <summary>
        /// Gets the API key for an API call.
        /// </summary>
        public static string GetTokenKey(this IIdentity identity)
        {
            return ((ClaimsIdentity)identity).FindFirst(ClaimTypes.NameIdentifier)!.Value;
        }

        /// <summary>
        /// Gets the <see cref="Guid"/> User ID for a Web user.
        /// </summary>
        public static Guid? GetUserId(this IIdentity? identity)
        {
            return ParseGuid(identity, ClaimTypes.NameIdentifier);
        }

        private static Guid? ParseGuid(IIdentity? identity, string claimName)
        {
            return Guid.TryParse((identity as ClaimsIdentity)?.FindFirst(claimName)?.Value, out var id)
                ? id
                : null;
        }

        /// <summary>
        /// Gets the Identity Roles for the given user, as specified in their claims.
        /// </summary>
        public static ICollection<string> GetClaimRoles(this ClaimsPrincipal user)
        {
            return user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
        }
    }
}

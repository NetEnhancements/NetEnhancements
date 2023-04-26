using Microsoft.AspNetCore.Identity;

namespace NetEnhancements.Identity.Data;

/// <summary>
/// Junction table entity for linking users and roles with <see cref="Guid"/> PKs.
/// </summary>
public class ApplicationUserRole : IdentityUserRole<Guid>
{
}
using Microsoft.AspNetCore.Identity;
using NetEnhancements.EntityFramework;

namespace NetEnhancements.Identity.Data;

/// <summary>
/// An application role with a <see cref="Guid"/> PK.
/// </summary>
public class ApplicationRole : IdentityRole<Guid>, IGuidIdEntity
{
    public ApplicationRole() : base() { }

    public ApplicationRole(string roleName) : base(roleName) { }

    public ICollection<ApplicationUser>? Users { get; set; }
}
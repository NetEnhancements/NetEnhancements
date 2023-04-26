using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using NetEnhancements.EntityFramework;

namespace NetEnhancements.Identity.Data;

/// <summary>
/// An application user with a <see cref="Guid"/> PK.
/// </summary>
public class ApplicationUser : IdentityUser<Guid>, IGuidIdEntity
{
    public ApplicationUser() : base() { }

    public ApplicationUser(string userName) : base(userName) { }

    public DateTimeOffset Registered { get; set; }

    public DateTimeOffset? EmailConfirmedDate { get; set; }

    [StringLength(100, MinimumLength = 1)]
    public string? FirstName { get; set; }

    [StringLength(100, MinimumLength = 1)]
    public string? LastName { get; set; }

    public ICollection<ApplicationRole>? Roles { get; set; }

    public static Expression<Func<ApplicationUser, object?>> OrderClause(string order)
    {
        return order.ToLowerInvariant() switch
        {
            "email" => u => u.Email,
            "name" => u => u.FirstName + u.LastName,
            "registered" => u => u.Registered,

            _ => throw new ArgumentException(null, nameof(order))
        };
    }

    public string GetFullName()
    {
        var hasFirstName = !string.IsNullOrWhiteSpace(FirstName);
        var hasLastName = !string.IsNullOrWhiteSpace(LastName);

        return FirstName
            + (hasFirstName && hasLastName ? " " : "")
            + LastName;
    }
}

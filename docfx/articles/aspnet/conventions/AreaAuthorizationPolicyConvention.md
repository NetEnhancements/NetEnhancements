# Area Authorization Policy Convention

ASP.NET Core has a concept of "[policies](https://learn.microsoft.com/en-us/aspnet/core/security/authorization/policies?view=aspnetcore-8.0)". Herein you configure policies during startup:

```csharp
builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("RequireAdmin", p =>
    {
        p.RequireAuthenticatedUser();
        p.RequireRole("Admin");
    });
});
```

Which you then apply to controllers or Razor Pages:

```csharp
[Authorize(Policy = "RequireAdmin")]
public class UsersController : Controller
{
}
```

This `[Authorize(Policy = "RequireAdmin")]` needs to be applied to all controllers and pages within that area which you want to secure.

## Not anymore!

Using our [`AreaAuthorizationPolicyConvention`](/api/NetEnhancements.AspNet.Conventions.AreaAuthorizationPolicyConvention.html) you don't have to anymore. 

You still have to tell ASP.NET about the policies you want to recognize as shown before. But for applying them, things have been made somewhat easier.

First, you build the convention using AreaPolicies:

```csharp
AreaPolicy[] areaPolicies = 
[
    // Identity area: uses its own policies, leave as-is.
    new (AreaName: "Identity"),

    // Admin area: requires a logged in admin user.
    new (AreaName: "Admin", new AuthorizeFilter(adminAreaPolicy)),

    // Policy for all other areas (including none).
    new DefaultAreaPolicy(),
];

// Configure policies per Area.
var areaPolicyConvention = new AreaAuthorizationPolicyConvention(areaPolicies);
```

Then configure the conventions:

```csharp
// Configure MVC.
mvcBuilder.AddMvcOptions(options =>
{
    // Apply the policies to controllers.
    options.Conventions.Add(areaPolicyConvention);
});

// Configure Razor Pages.
mvcBuilder.AddRazorPagesOptions(options =>
{
    // Apply the policies to pages.
    options.Conventions.Add(areaPolicyConvention);
});
```

## (Default)AreaPolicy

Each [`AreaPolicy`](/api/NetEnhancements.AspNet.Conventions.AreaPolicy.html) links an area name to a policy to apply it to.

You don't use areas, or have stuff outside of areas? Use the [`DefaultAreaPolicy`](/api/NetEnhancements.AspNet.Conventions.DefaultAreaPolicy.html)!

# NetEnhancements.Identity

A default implementation of ASP.NET Core Identity, with GUID PKs, extension methods to look up users, an Identity database schema and an ApplicationUserManager.

Typical usage:

```csharp
public class YourAppDbContext : NetEnhancements.Identity.IdentityDbContext
{
    public YourAppDbContext(DbContextOptions<YourAppDbContext> options)
        : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        // Apply Modified to ITimestampedEntity implementors.
        optionsBuilder.AddInterceptors(new TimestampedEntityInterceptor());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Default values for Id and Created for all IGuidEntity and ITimestampedEntity.
        modelBuilder.ApplyDefaultValues();
    }
}
```

Dependency injection in an ASP.NET Core web app:
```csharp
var builder = WebApplication.CreateBuilder(args);

// var mvcBuilder = builder.Services.AddMvc()...

builder.Services.AddIdentityService<YourAppDbContext>();
```

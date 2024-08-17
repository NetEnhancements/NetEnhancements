---
description: Caching Physical File Provider
---

# Serving Static Files

When you use ASP.NET Core's `app.UseStaticFiles()`, then every request that hits the server will hit the filesystem.

## Not anymore!

We've created a `CachingPhysicalFileProvider` which caches file metadata _and_ file data in-memory. This should be the web server's task, but lacking support or configuration for that, you can use our helper.

Usage is trivial, just pass the provider to the options of the built-in `UseStaticFiles()`:

```csharp
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new CachingPhysicalFileProvider(app.Environment),
});
```

As for the options you can pass after `app.Environment`:

```csharp
new PhysicalFileCachingOptions
{
    // Enlarge the total cache to 64 MiB
    MaxCacheSize = 64 * 1024 * 1024,
    // Limit cacheable files to 512 KiB
    MaxSingleFileSize = 512 * 1024,
    // Lower the 1-hour expiration to 30 minutes
    Expiration = TimeSpan.FromMinutes(30),
    // Alter the wwwroot default
    Root = "/an/absolute/path/"
}
```

All options are optional and have sensible defaults:

* `Root` defaults to "wwwroot" under your application directory (or rather: WebRootPath under ContentRootPath).
* `MaxCacheSize` is set to 16 MiB.
* `MaxSingleFileSize` lets you cache files up to 1 MiB.
* `Expiration` configures sliding expiration, defaults to 1 hour.
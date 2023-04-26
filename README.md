# NetEnhancements Libraries

This is a collection of helpers, snippets, extension methods and application components built by @Skamiplan and @CodeCasterNL.

These are inteded to be used to build ASP.NET Core / .NET 7+ applications, usually with Microsoft DI, logging, EF, MVC.

# [Documentation](https://netenhancements.github.io/)
See https://netenhancements.github.io/.

# What's in the box
Or how do we convince you to use these libraries?

* **Utils**: extension methods for dates, strings, collections, enums...
* **Shared**: application configuration and logging.
* **AspNet**: conventions for routing, security, validation.
* **EntityFramework**: DateOnly/TimeOnly, entities with GUID PKs, Created/Modified properties, ...
* **Identity**: a ready-to-use ASP.NET Core Identity setup.
* **Imaging**: resizing images.
  * **Imaging.EntityFramework**: saving image metadata in a database.
* **Services**: background services.
* **Business**: combining the above in easy-to-use application parts.

# Ideology
We start at SemVer "version 0", in which everything may break with every release, but we try not to. As soon as possible, we'll release a version 1.0.0, which we'll try to support as long as possible, but we'll have to decide on a release cadence. We'll probably release a major version _at least_ with every major .NET release.

If there's no documentation and no tests, it doesn't exist. Literally. Something may exist in code, but unless it's documented and at least the happy case is covered, it's not meant to be used.

---
title: ASP.NET
---

# ASP.NET Core Enhancements

We offer various extensions for ASP.NET Core.

## Default Area Route
Call `app.MapDefaultAreaControllerRoute()` to register the route `{area:exists}/{controller=Home}/{action=Index}/{id?}`. This enables routing into any existing area.

See also the [Area Controller Routing Convention](conventions/AreaControllerRouting.html).


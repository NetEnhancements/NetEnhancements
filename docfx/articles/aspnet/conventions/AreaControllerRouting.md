# Area Controller Routing Convention

With ASP.NET Core, if you have a controller that lives in an area, say `FooController` in the area `Bar`, you'll have to annotate the controller:

```csharp
namespace MyApp.Areas.Bar
{
    [Area("Bar")]
    public class FooController : Controller
    {
    }
}
```

This `[Area("Bar")]` needs to be applied to all controllers within that area.

## Not anymore!
Using our [`UseAreaControllerNamespace()`](/api/NetEnhancements.AspNet.Conventions.ControllerModelExtensions.html) you don't have to anymore:

```csharp
var mvcBuilder = builder.Services.AddMvc() // or .AddControllers()
    .AddMvcOptions(options =>
    {
        options.UseAreaControllerNamespace();
    }
```

This method registers the [`AreaControllerRoutingConvention`](/api/NetEnhancements.AspNet.Conventions.AreaControllerRoutingConvention.html), which registers controllers within an Areas namespace as if they had the Area attribute applied.

The rules:

* The controller must be in a namespace that contains ".Areas."
* What follows "Areas." will be taken as area name: `MyApp.Areas.Bar.FooController` will be registered as having the area "Bar".
* If the controller has attribute routing applied (`[Route("...")]`), it won't be registered.
* The route for the controller will be registered as "[area]/[controller]/[action]/{id?}", having `[area]` replaced with the detected area name from the namespace.

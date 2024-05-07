using Microsoft.AspNetCore.Mvc.Authorization;
using NetEnhancements.AspNet;
using NetEnhancements.AspNet.Conventions;

var builder = WebApplication.CreateBuilder(args);

// Add MVC to the container.
var mvcBuilder = builder.Services.AddMvc();

// Register the authorization policies.
const string adminAreaPolicy = "AdminAreaPolicy";

// Policies are an ASP.NET Core construct.
builder.Services.AddAuthorization(o =>
{
    // Add an admin policy with requirements.
    o.AddPolicy(adminAreaPolicy, p =>
    {
        p.RequireAuthenticatedUser();
        p.RequireRole("Admin");
    });

    // Add the fallback policy
    o.AddPolicy(DefaultAreaPolicy.PolicyName, p =>
    {
        // Requirements are required in policies, so pass-through everything.
        p.RequireAssertion(_ => true);
    });
});

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

// Configure MVC.
mvcBuilder.AddMvcOptions(options =>
{
    // See NetEnhancements.Web.Areas.Bar.FooController.
    options.UseAreaControllerNamespace();

    // Apply the policies to controllers.
    options.Conventions.Add(areaPolicyConvention);
});

// Configure Razor Pages.
mvcBuilder.AddRazorPagesOptions(options =>
{
    // Apply the policies to pages.
    options.Conventions.Add(areaPolicyConvention);
});

mvcBuilder.AddRouteDebugger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Define things that return responses.
app.MapRazorPages();

// One of our enhancements.
app.MapDefaultAreaControllerRoute();

// Register a Minimal API.
app.MapGet("/Minimal", () => "Hello World!");

// This one must be defined last.
app.MapDefaultControllerRoute();

app.Run();

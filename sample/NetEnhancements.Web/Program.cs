using NetEnhancements.AspNet;
//using NetEnhancements.AspNet.Routing;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var mvcBuilder = builder.Services.AddMvc();
mvcBuilder.AddMvcOptions(options =>
{
    options.UseAreaControllerNamespace();
});

//mvcBuilder.AddRouteDebugger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

//var d = app.Services.CreateScope().ServiceProvider.GetRequiredService<IRouteDebugger>();

//var route = d.GetRoutes().Last();

app.Run();

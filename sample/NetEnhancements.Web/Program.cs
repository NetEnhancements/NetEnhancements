using NetEnhancements.AspNet;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var mvcBuilder = builder.Services.AddMvc();
mvcBuilder.AddMvcOptions(options =>
{
    // See NetEnhancements.Web.Areas.Bar.FooController.
    options.UseAreaControllerNamespace();
});

//mvcBuilder.AddRouteDebugger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Define things that return responses.
app.MapRazorPages();

// One of our enhancements.
app.MapDefaultAreaControllerRoute();

app.MapGet("/Minimal", () => "Hello World!");

// This one must be defined last.
app.MapDefaultControllerRoute();

app.Run();

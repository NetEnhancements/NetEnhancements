using NetEnhancements.Identity.Configuration;
using NetEnhancements.Identity.Data;
using NetEnhancements.Identity.Managers;
using NetEnhancements.Shared.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetEnhancements.AspNet;

namespace NetEnhancements.Identity
{
    /// <summary>
    /// Dependency Injection extensions container.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Register ASP.NET Core Identity services.
        /// </summary>
        public static IdentityBuilder AddIdentityService<TDbContext>(this IServiceCollection services)
            where TDbContext : Data.IdentityDbContext
        {
            // Register the user's DbContext as ours.
            services.AddScoped<Data.IdentityDbContext>(p => p.GetRequiredService<TDbContext>());

            // Add ASP.NET Identity
            var builder = services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.User.RequireUniqueEmail = true;

                options.SignIn.RequireConfirmedAccount = true;

                //options.Password....
            })
                .AddUserStore<UserStore<ApplicationUser, ApplicationRole, TDbContext, Guid, IdentityUserClaim<Guid>, ApplicationUserRole, IdentityUserLogin<Guid>, IdentityUserToken<Guid>, IdentityRoleClaim<Guid>>>()
                .AddRoleStore<RoleStore<ApplicationRole, TDbContext, Guid, ApplicationUserRole, IdentityRoleClaim<Guid>>>()
                .AddUserManager<ApplicationUserManager>()
                .AddDefaultTokenProviders();

            return builder;
        }

        /// <summary>
        /// Configure the Identity cookie names by removing the ".AspNetCore" part.
        /// </summary>
        public static IdentityBuilder ConfigureAuthCookies(this IdentityBuilder builder, IConfiguration configuration)
        {
            var (section, settings) = configuration.GetSectionOrThrow<IdentitySettings>();

            if (string.IsNullOrWhiteSpace(settings.ApplicationName)) throw new ArgumentNullException("settings.ApplicationName");
            if (string.IsNullOrWhiteSpace(settings.KeyFilePath)) throw new ArgumentNullException("settings.KeyFilePath");
            Directory.CreateDirectory(settings.KeyFilePath);

            // And make sure we can still eat the cookies tomorrow.
            builder.Services.AddDataProtection()
                    .PersistKeysToFileSystem(new DirectoryInfo(settings.KeyFilePath))
                    // And after a site path change.
                    .SetApplicationName(settings.ApplicationName);

            // TODO: get all these strings from config

            // Don't leak ".AspNetCore.Antiforgery.*".
            builder.Services.AddAntiforgery(options =>
            {
                options.HeaderName = options.Cookie.Name = "XSRF-Token";
            });

            // JWT cookie and auth URLs.
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "Auth";
                options.Cookie.IsEssential = true;

                options.Validate();
            });

            // Cookie: between login and 2FA.
            builder.Services.Configure<CookieAuthenticationOptions>(Microsoft.AspNetCore.Identity.IdentityConstants.TwoFactorUserIdScheme, options =>
            {
                options.Cookie.Name = "2FAUserId";
                options.Cookie.IsEssential = true;
            });

            // Cookie: 2FA Remember me.
            builder.Services.Configure<CookieAuthenticationOptions>(Microsoft.AspNetCore.Identity.IdentityConstants.TwoFactorRememberMeScheme, options =>
            {
                options.Cookie.Name = "2FAUserId";
                options.Cookie.IsEssential = true;
            });

            return builder;
        }

        /// <summary>
        /// Remove the "/Identity" prefix from the routes introduced by using the ASP.NET Core Identity Default UI. Call after <c>AddDefaultUI()</c>.
        /// </summary>
        public static IdentityBuilder RemoveIdentityPrefix(this IdentityBuilder builder, IMvcBuilder mvcBuilder)
        {
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.AccessDeniedPath = "/account/accessdenied";
            });

            mvcBuilder.AddRazorPagesOptions(options =>
            {
                // Hide "/Identity" from identity pages.
                options.UseGeneralRoutePrefix("Identity", "", removeAreaFromUrl: true);
            });

            return builder;
        }
    }
}

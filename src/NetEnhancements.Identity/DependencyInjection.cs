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

            // JWT cookie and auth URLs.
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "Auth";
                options.Cookie.IsEssential = true;
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.AccessDeniedPath = "/account/accessdenied";

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
    }
}

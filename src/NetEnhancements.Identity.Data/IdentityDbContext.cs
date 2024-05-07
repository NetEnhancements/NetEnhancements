using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace NetEnhancements.Identity.Data
{
    /// <summary>
    /// Base class for a DbContext to be used with NetEnhancements.Identity.
    /// </summary>
    public class IdentityDbContext : Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext
    <
        ApplicationUser,
        ApplicationRole,
        Guid,
        IdentityUserClaim<Guid>,
        ApplicationUserRole,
        IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>,
        IdentityUserToken<Guid>
    >
    {
        /// <summary>
        /// Have Identity tables live in the Identity schema, e.g. [Identity].[Users].
        /// </summary>
        public const string IdentitySchema = "Identity";

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        public IdentityDbContext(DbContextOptions options) : base(options) { }

        /// <summary>
        /// Set up Identity tables.
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Call the default Identity configuration first.
            base.OnModelCreating(builder);

            // Then override the registered entities with customized table and schema names.
            // These are our inherited, custom entities.
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "Users", IdentitySchema);
            });
            builder.Entity<ApplicationRole>(entity =>
            {
                entity.ToTable(name: "Roles", IdentitySchema);
            });
            builder.Entity<ApplicationUserRole>(entity =>
            {
                entity.ToTable("UserRoles", IdentitySchema);
            });

            // These are the default, untouched entities - but we still need to configure the table and schema names.
            builder.Entity<IdentityUserClaim<Guid>>(entity =>
            {
                entity.ToTable("UserClaims", IdentitySchema);
            });
            builder.Entity<IdentityUserLogin<Guid>>(entity =>
            {
                entity.ToTable("UserLogins", IdentitySchema);
            });
            builder.Entity<IdentityRoleClaim<Guid>>(entity =>
            {
                entity.ToTable("RoleClaims", IdentitySchema);
            });
            builder.Entity<IdentityUserToken<Guid>>(entity =>
            {
                entity.ToTable("UserTokens", IdentitySchema);
            });

            // Many-to-many

            // User.Roles (via "UserRoles" table)
            builder.Entity<ApplicationRole>()
                   .HasMany(u => u.Users)
                   .WithMany(u => u.Roles)
                   .UsingEntity<ApplicationUserRole>(
                        left => left.HasOne<ApplicationUser>().WithMany().HasForeignKey(ur => ur.UserId).IsRequired(),
                        right => right.HasOne<ApplicationRole>().WithMany().HasForeignKey(ur => ur.RoleId).IsRequired()
                    );
        }
    }
}

using NetEnhancements.Shared.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace NetEnhancements.Identity.Data
{
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

#pragma warning disable CS8618 // DbSet properties are instantiated by EF.
        public IdentityDbContext(DbContextOptions options) : base(options) { }
#pragma warning restore CS8618

        /// <summary>
        /// Set up Identity tables.
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Identity.
            base.OnModelCreating(builder);

            // Customized
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

            // Default
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

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            MarkModifiedEntitiesAsModified();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            MarkModifiedEntitiesAsModified();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void MarkModifiedEntitiesAsModified()
        {
            var entries = ChangeTracker.Entries().Where(e => e.Entity is ITimestampedEntity && e.State == EntityState.Modified);

            foreach (var entityEntry in entries)
            {
                ((ITimestampedEntity)entityEntry.Entity).Modified = DateTimeOffset.Now;
            }
        }
    }
}

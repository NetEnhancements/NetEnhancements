using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetEnhancements.EntityFramework;
using NetEnhancements.EntityFramework.Query;
using NetEnhancements.Identity.Data;

namespace NetEnhancements.Identity.Managers
{
    /// <inheritdoc/>
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        private readonly IdentityDbContext _dbContext;
        private readonly RoleManager<ApplicationRole> _roleManager;

        // Hell yeah.
        /// <inheritdoc/>
        public ApplicationUserManager(
            // Our DI here.
            IdentityDbContext dbContext,
            RoleManager<ApplicationRole> roleManager,

            // For base, don't touch.
            IUserStore<ApplicationUser> store, IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<ApplicationUser> passwordHasher, IEnumerable<IUserValidator<ApplicationUser>> userValidators,
            IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<ApplicationUser>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Returns all roles known to the storage.
        /// </summary>
        public Task<List<ApplicationRole>> GetAllRolesAsync() => _roleManager.Roles.ToListAsync();

        public async Task<PagedResults<ApplicationUser>> FindAllUsersAsync(DataQuery query, bool includeRoles = false)
        {
            IQueryable<ApplicationUser> userQuery = _dbContext.Users;

            if (includeRoles)
            {
                userQuery = userQuery.Include(u => u.Roles);
            }

            var items = await userQuery
                .ApplyOrder(query, ApplicationUser.OrderClause)
                .Page(query)
                .ToListAsync();

            return new PagedResults<ApplicationUser>(items, query);
        }

        public Task<ApplicationUser?> FindByIdAsync(Guid? userId, bool includeRoles = false)
        {
            IQueryable<ApplicationUser> userQuery = _dbContext.Users;

            if (includeRoles)
            {
                userQuery = userQuery.Include(u => u.Roles);
            }

            return userQuery.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<bool> IsInRoleAsync(Guid? userId, string role)
        {
            var user = await FindByIdAsync(userId);

            return user != null && await IsInRoleAsync(user, role);
        }

        public async Task IncrementFailedAccessCountAsync(ApplicationUser user)
        {
            if (!SupportsUserLockout || !await GetLockoutEnabledAsync(user))
            {
                return;
            }

            // Identity will lock out the user as configured (default 5 min after 5 failed attempts).
            var failResult = await AccessFailedAsync(user);

            if (user.AccessFailedCount >= Options.Lockout.MaxFailedAccessAttempts
                // TODO: AccessFailedCount rolls over ever when the account is locked out.
                || user.AccessFailedCount == 0)
            {
                // TODO: mail/alert admins.
            }
        }

        public override async Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string token)
        {
            var result = await base.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                user.EmailConfirmedDate = DateTimeOffset.Now;
                result = await Store.UpdateAsync(user, CancellationToken);
            }

            return result;
        }

        /// <summary>
        /// Change the first and last name for the given user.
        /// </summary>
        public async Task<IdentityResult> UpdateNameAsync(ApplicationUser user, string firstName, string lastName)
        {
            var databaseUser = await base.FindByIdAsync(user.Id.ToString());

            if (databaseUser == null)
            {
                return IdentityResult.Failed(new IdentityError { Code = "1", Description = "User not found by id" });
            }

            databaseUser.FirstName = firstName;
            databaseUser.LastName = lastName;

            return await Store.UpdateAsync(databaseUser, CancellationToken);
        }

        /// <summary>
        /// Updates the email address (and therefore username) for the given user.
        /// </summary>
        public async Task<IdentityResult> UpdateEmailAsync(ApplicationUser user, string newEmail)
        {
            var otherUser = await FindByEmailAsync(newEmail);

            if (otherUser != null)
            {
                // TODO: log
                return IdentityResult.Failed(new IdentityError { Code = "2", Description = "E-mailadres reeds in gebruik" });
            }

            var emailResult = await SetEmailAsync(user, newEmail);
            if (!emailResult.Succeeded)
            {
                // TODO: log
                return emailResult;
            }

            await UpdateNormalizedEmailAsync(user);

            var userNameResult = await SetUserNameAsync(user, newEmail);
            if (!userNameResult.Succeeded)
            {
                // TODO: log
                return userNameResult;
            }

            await UpdateNormalizedUserNameAsync(user);

            return IdentityResult.Success;
        }
    }
}

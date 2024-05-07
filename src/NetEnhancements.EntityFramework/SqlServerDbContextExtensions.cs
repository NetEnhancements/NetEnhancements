using Microsoft.EntityFrameworkCore;

namespace NetEnhancements.EntityFramework
{
    /// <summary>
    /// SQL Server extensions.
    /// </summary>
    public static class SqlServerDbContextExtensions
    {
        /// <summary>
        /// Enables or disables <a href="https://learn.microsoft.com/en-us/sql/t-sql/statements/set-identity-insert-transact-sql">Identity Insert</a> for the given <typeparamref name="TEntity"/>.
        /// </summary>
        public static Task EnableIdentityInsertAsync<TEntity>(this DbContext context, bool enable)
        {
            var entityType = typeof(TEntity);

            var entityModelType = context.Model.FindEntityType(entityType);
            if (entityModelType == null)
            {
                var errorString = $"The entity type '{entityType.FullName}' is not known by the context '{context.GetType().FullName}'";

                throw new ArgumentException(errorString);
            }

            var onOrOff = enable ? "ON" : "OFF";
            var query = $"SET IDENTITY_INSERT [{entityModelType.GetSchema()}].[{entityModelType.GetTableName()}] {onOrOff}";

            return context.Database.ExecuteSqlRawAsync(query);
        }

        /// <summary>
        /// Enables <a href="https://learn.microsoft.com/en-us/sql/t-sql/statements/set-identity-insert-transact-sql">Identity Insert</a> for the given <typeparamref name="TEntity"/>, saves the changes and disables Identity Insert again, all wrapped in a transaction.
        /// </summary>
        public static async Task<int> SaveChangesWithIdentityInsertAsync<TEntity>(this DbContext context)
        {
            await using var transaction = await context.Database.BeginTransactionAsync();

            await context.EnableIdentityInsertAsync<TEntity>(enable: true);

            var rowsAffected = await context.SaveChangesAsync();

            await context.EnableIdentityInsertAsync<TEntity>(enable: false);

            await transaction.CommitAsync();

            return rowsAffected;
        }
    }
}

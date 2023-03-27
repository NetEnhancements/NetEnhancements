using Microsoft.EntityFrameworkCore;

namespace NetEnhancements.EntityFramework
{
    public static class DbContextExtensions
    {
        public static Task EnableIdentityInsert<TEntity>(this DbContext context) => context.SetIdentityInsert<TEntity>(on: true);

        public static Task DisableIdentityInsert<TEntity>(this DbContext context) => context.SetIdentityInsert<TEntity>(on: false);

        private static Task SetIdentityInsert<TEntity>(this DbContext context, bool on)
        {
            var entityType = typeof(TEntity);

            var entityModelType = context.Model.FindEntityType(entityType);
            if (entityModelType == null)
            {
                var errorString = $"The entity type '{entityType.FullName}' is not known by the context '{context.GetType().FullName}'";

                throw new ArgumentException(errorString);
            }

            var onOrOff = on ? "ON" : "OFF";
            var query = $"SET IDENTITY_INSERT [{entityModelType.GetSchema()}].[{entityModelType.GetTableName()}] {onOrOff}";

            return context.Database.ExecuteSqlRawAsync(query);
        }

        public static int SaveChanges<TEntity>(this DbContext context)
        {
            using var transaction = context.Database.BeginTransaction();

            context.EnableIdentityInsert<TEntity>();

            var rowsAffected = context.SaveChanges();

            context.DisableIdentityInsert<TEntity>();

            transaction.Commit();

            return rowsAffected;
        }
    }
}

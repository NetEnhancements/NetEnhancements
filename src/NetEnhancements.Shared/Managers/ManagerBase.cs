using System.Diagnostics;
using System.Runtime.CompilerServices;
using NetEnhancements.Shared.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace NetEnhancements.Shared.Managers
{
    /// <summary>
    /// Base class for Business Logic classes that talk to the database, which prevents you from having to create try-catch blocks.
    /// </summary>
    public abstract class ManagerBase<TDbContext>
        where TDbContext : DbContext
    {
        protected readonly TDbContext DbContext;

        protected readonly ILogger<ManagerBase<TDbContext>> Logger;

        protected ManagerBase(ILogger<ManagerBase<TDbContext>> logger, TDbContext dbContext)
        {
            Logger = logger;
            DbContext = dbContext;
        }

        /// <summary>
        /// Wrapper to handle logging and <see cref="EntityNotFoundException"/> handling for async methods returning a <see cref="BusinessResult{TResult}"/>.
        /// </summary>
        [DebuggerStepThrough]
        public Task<BusinessResult<TResult>> TryAsync<TResult>(Func<Task<TResult?>> methodBody, [CallerMemberName] string? callerMember = null)
            where TResult : class
        {
            return TryAsync(async () =>
            {
                var result = await methodBody();

                return Entity(result);
            }, callerMember);
        }

        /// <summary>
        /// Wrapper to handle logging and <see cref="EntityNotFoundException"/> handling for async methods returning a <see cref="BusinessResult{TResult}"/>.
        /// </summary>
        [DebuggerStepThrough]
        public async Task<BusinessResult<TResult>> TryAsync<TResult>(Func<Task<BusinessResult<TResult>>> methodBody, [CallerMemberName] string? callerMember = null)
            where TResult : class
        {
            try
            {
                return await methodBody();
            }
            catch (EntityNotFoundException ex)
            {
                Logger.LogError(ex, "Entity not found in {callerMember}", GetMethodInfo(callerMember));

                return BusinessResult<TResult>.FromNotFound(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Application error in {callerMember}", GetMethodInfo(callerMember));

                return BusinessResult<TResult>.FromError(ex.Message);
            }
        }

        /// <summary>
        /// Wrapper to handle logging and <see cref="Exception"/> handling for async methods returning a <see cref="BusinessResult"/>.
        /// </summary>
        [DebuggerStepThrough]
        public async Task<BusinessResult> TryAsync(Func<Task<BusinessResult>> methodBody, [CallerMemberName] string? callerMember = null)
        {
            try
            {
                return await methodBody();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Application error in {callerMember}", GetMethodInfo(callerMember));

                return BusinessResult.Error();
            }
        }

        /// <summary>
        /// Saves changes and returns a succesful <see cref="BusinessResult{T}"/> when any rows were affected.
        /// </summary>
        [DebuggerStepThrough]
        protected async Task<BusinessResult<T>> SaveChangesAsync<T>(T entity, bool allowZeroRowsAffected = false, [CallerMemberName] string? callerMember = null)
        {
            const string errorLog = $"Application error in {{callerMember}} (calling {nameof(SaveChangesAsync)}()): ";
            callerMember = GetMethodInfo(callerMember);

            // TODO: try, catch, read EF errors.
            int recordsAffected;

            try
            {
                recordsAffected = await DbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException updateEx)
            {
                Logger.LogError(updateEx, errorLog, callerMember);

                foreach (var e in updateEx.Entries)
                {
                    Debugger.Break();
                    // TODO: what can we log?
                }

                throw;
            }
            catch (DbUpdateException updateEx)
            {
                Logger.LogError(updateEx, errorLog, callerMember);

                Debugger.Break();
                // TODO: what can we log?

                throw;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, errorLog, callerMember);

                throw;
            }

            return allowZeroRowsAffected || recordsAffected > 0
                    ? entity
                    : BusinessResult<T>.FromError("No data returned");
        }

        [DebuggerStepThrough]
        protected BusinessResult<TEntity> Entity<TEntity>(TEntity? entity, [CallerMemberName] string? callerMember = null)
            where TEntity : class
        {
            const string notFoundLog = $"Entity {{entityType}} not found in {{callerMember}} (calling {nameof(Entity)}()): ";

            if (entity != null)
            {
                return new BusinessResult<TEntity>(entity);
            }

            Logger.LogWarning(notFoundLog, typeof(TEntity).Name, callerMember);

            return BusinessResult<TEntity>.FromNotFound();
        }

        [DebuggerStepThrough]
        private static string GetMethodInfo(string? callerMember) => callerMember != null ? callerMember + "()" : "(no method info)";
    }
}

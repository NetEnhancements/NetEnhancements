using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace NetEnhancements.EntityFramework;

/// <summary>
/// Intercepts saves to entities implementing <see cref="ITimestampedEntity"/>.
/// </summary>
public class TimestampedEntityInterceptor : ISaveChangesInterceptor
{
    /// <inheritdoc />
    public ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        MarkModifiedEntitiesAsModified(eventData.Context!.ChangeTracker);

        return ValueTask.FromResult(result);
    }

    /// <inheritdoc />
    public InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        MarkModifiedEntitiesAsModified(eventData.Context!.ChangeTracker);

        return result;
    }

    private static void MarkModifiedEntitiesAsModified(ChangeTracker changeTracker)
    {
        var entries = changeTracker.Entries().Where(e => e is { Entity: ITimestampedEntity, State: EntityState.Modified });

        foreach (var entityEntry in entries)
        {
            ((ITimestampedEntity)entityEntry.Entity).Modified = DateTimeOffset.Now;
        }
    }
}

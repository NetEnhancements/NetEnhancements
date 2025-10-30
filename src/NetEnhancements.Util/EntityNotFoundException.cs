namespace NetEnhancements.Util;

/// <summary>
/// An exception indicating an expected entity was not found.
/// </summary>
public class EntityNotFoundException : Exception
{
    /// <summary>
    /// Initialize the exception with an entity type <typeparamref name="TEntity"/> and the given primary key value(s).
    /// </summary>
    public static EntityNotFoundException From<TEntity>(object primaryKey, params object[] additionalKeys)
    {
        var primaryKeyString = string.Join("', '", new[] { primaryKey }.Concat(additionalKeys));

        var errorString = $"Entity of type {typeof(TEntity).FullName} not found by PK '{primaryKeyString}'";

        return new EntityNotFoundException(errorString);
    }

    private EntityNotFoundException(string message) : base(message) { }
}

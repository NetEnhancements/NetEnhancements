namespace NetEnhancements.Util;

/// <summary>
/// Exception indicating an expected entity was unexpectedly not found.
/// </summary>
public class EntityNotFoundException(string message) : Exception(message)
{
    /// <summary>
    /// Primary key representation of the entity that was not found.
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// Type name of the entity that was not found.
    /// </summary>
    public required string EntityTypeName { get; init; }

    /// <summary>
    /// Create an <see cref="EntityNotFoundException"/> for the given <typeparamref name="TEntity"/> and the primary key value(s).
    /// </summary>
    public static EntityNotFoundException For<TEntity>(object primaryKey, params object[] additionalKeys)
    {
        var typeName = typeof(TEntity).Name;
        var fullTypeName = typeof(TEntity).FullName ?? typeName;

        var primaryKeyString = string.Join("', '", new[] { primaryKey }.Concat(additionalKeys));

        var errorString = $"Entity of type {typeName} not found by PK '{primaryKeyString}'";

        return new EntityNotFoundException(errorString)
        {
            Id = primaryKeyString,
            EntityTypeName = fullTypeName
        };
    }
}

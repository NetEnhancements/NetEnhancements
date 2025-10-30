namespace NetEnhancements.Util;

/// <summary>
/// Container for results and pagination.
/// </summary>
public sealed record PagedResults<TEntity>(IReadOnlyCollection<TEntity> Items, long CurrentPage, long ItemsPerPage, long TotalItems)
{
    /// <summary>
    /// Total number of pages, calculated from the <see cref="TotalItems"/> and <see cref="ItemsPerPage"/>.
    /// </summary>
    public long TotalPages
    {
        get
        {
            var ceil = Math.Ceiling(TotalItems / (double)ItemsPerPage);
            return Math.Max((long)ceil, 0);
        }
    }

    /// <summary>
    /// Converts one PagedResults{TSource} to another, copying paging properties and assigning the passed <paramref name="entities"/>.
    /// </summary>
    public static PagedResults<TEntity> From<TSource>(PagedResults<TSource> source, IReadOnlyCollection<TEntity> entities)
        => new(entities, source.CurrentPage, source.ItemsPerPage, source.TotalItems);

    /// <summary>
    /// Turns a PagedResults{TEntity} into a PagedResults{TDestination} using the supplied conversion function. 
    /// </summary>
    public PagedResults<TDestination> ConvertUsing<TDestination>(Func<TEntity, TDestination> convert)
    {
        var converted = Items.Select(convert).ToArray();

        return PagedResults<TDestination>.From(this, converted);
    }

    /// <summary>
    /// Turns a PagedResults{TEntity} into a PagedResults{TDestination} using the supplied conversion function. 
    /// </summary>
    public async Task<PagedResults<TDestination>> ConvertUsingAsync<TDestination>(Func<TEntity, Task<TDestination>> convert)
    {
        var converted = new List<TDestination>(Items.Count);

        foreach (var item in Items)
        {
            converted.Add(await convert(item));
        }

        return PagedResults<TDestination>.From(this, converted);
    }
}

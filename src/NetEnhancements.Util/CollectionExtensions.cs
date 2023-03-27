using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace NetEnhancements.Util
{
    /// <summary>
    /// Provides extension methods for collections.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Returns whether <paramref name="value"/> is in <paramref name="matchingValues"/>.
        /// </summary>
        public static bool In<T>(this T? value, params T[] matchingValues) => value.In((IEnumerable<T>)matchingValues);

        /// <summary>
        /// Returns whether <paramref name="value"/> is in <paramref name="matchingValues"/>.
        /// </summary>
        public static bool In<T>(this T? value, IEnumerable<T> matchingValues) => matchingValues.Contains(value);

        /// <summary>
        /// Returns the collection when not null nor empty, or throws complaining thereabout.
        /// </summary>
        [StackTraceHidden]
        public static TCollection ThrowIfNullOrEmpty<TCollection>([NotNull] this TCollection? collection, [CallerArgumentExpression(nameof(collection))] string? collectionName = null)
            where TCollection : ICollection
        {
            if (collection is null or { Count: 0 })
            {
                throw new ArgumentException($"{collectionName ?? "The collection"} is null or empty");
            }

            return collection;
        }

        /// <summary>
        /// Takes one random element from the collection.
        /// </summary>
        public static T Random<T>(this ICollection<T> collection, Random random)
        {
            int index = random.Next(collection.Count);

            return collection.ElementAt(index);
        }
    }
}

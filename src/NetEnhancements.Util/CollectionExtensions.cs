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
        /// Takes one random element from the collection using the provided <see cref="Random"/> instance.
        /// </summary>
        public static T Random<T>(this IReadOnlyCollection<T> collection, Random random)
        {
            int index = random.Next(collection.Count);

            return collection.ElementAt(index);
        }

        /// <summary>
        /// Takes one random element from the collection.
        /// </summary>
        public static T Random<T>(this IReadOnlyCollection<T> collection)
        {
            return collection.Random(new Random());
        }

        /// <summary>
        /// Generates all permutations of a given length from the specified list.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the list.</typeparam>
        /// <param name="list">The input list to generate permutations from.</param>
        /// <param name="length">The length of the permutations to generate.</param>
        /// <returns>An enumerable of permutations, where each permutation is an enumerable of the same type as the input list.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the length is greater than the number of elements in the list.</exception>
        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(this IReadOnlyCollection<T> list, int length)
        {
            if (length > list.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }
            if (length == 1)
            {
                foreach (T t in list)
                {
                    yield return new[] { t };
                }
            }
            else
            {
                foreach (IEnumerable<T> perm in GetPermutations(list, length - 1))
                {
                    foreach (T t in list.Where(e => !perm.Contains(e)))
                    {
                        yield return perm.Concat(new[] { t });
                    }
                }
            }
        }

        /// <summary>
        /// Generates all permutations from the specified list.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the list.</typeparam>
        /// <param name="list">The input list to generate permutations from.</param>
        /// <returns>An enumerable of permutations, where each permutation is an enumerable of the same type as the input list.</returns>
        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(this IReadOnlyCollection<T> list)
        {
            var length = list.Count;

            return list.GetPermutations(length);
        }
    }
}

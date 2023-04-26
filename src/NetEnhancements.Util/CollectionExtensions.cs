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
        /// <typeparam name="T">Type of the elements in the collection</typeparam>
        /// <param name="value">The value to check if it is in the <paramref name="matchingValues"/> collection.</param>
        /// <param name="matchingValues">The values to check if <paramref name="value"/> is one of them.</param>
        /// <returns>true if <paramref name="value"/> is in <paramref name="matchingValues"/>, false otherwise.</returns>
        /// <example>
        /// <code>
        /// int value = 5;
        /// bool isFive = value.In(1, 2, 3, 4, 5);
        /// bool isTen = value.In(6, 7, 8, 9, 10);
        /// Console.WriteLine($"Is {value} in 1, 2, 3, 4, 5? {isFive}"); // Output: Is 5 in 1, 2, 3, 4, 5? True
        /// Console.WriteLine($"Is {value} in 6, 7, 8, 9, 10? {isTen}"); // Output: Is 5 in 6, 7, 8, 9, 10? False
        /// </code>
        /// </example>
        public static bool In<T>(this T? value, params T[] matchingValues) => value.In((IEnumerable<T>)matchingValues);

        /// <summary>
        /// Returns whether <paramref name="value"/> is in <paramref name="matchingValues"/>.
        /// </summary>
        /// <typeparam name="T">Type of the elements in the collection</typeparam>
        /// <param name="value">The value to check if it is in the <paramref name="matchingValues"/> collection.</param>
        /// <param name="matchingValues">The collection to check if <paramref name="value"/> is in it.</param>
        /// <returns>true if <paramref name="value"/> is in <paramref name="matchingValues"/>, false otherwise.</returns>
        /// <example>
        /// <code>
        /// string value = "cat";
        /// List&lt;string&gt; animals = new List&lt;string&gt; { "dog", "cat", "fish", "bird" };
        /// bool isInAnimals = value.In(animals);
        /// Console.WriteLine($"Is {value} in {string.Join(", ", animals)}? {isInAnimals}"); // Output: Is cat in dog, cat, fish, bird? True
        /// </code>
        /// </example>
        public static bool In<T>(this T? value, IEnumerable<T> matchingValues) => matchingValues.Contains(value);

        /// <summary>
        /// Returns the collection when not null nor empty, or throws an <see cref="ArgumentException"/> complaining thereabout.
        /// </summary>
        /// <typeparam name="TCollection">Type of the collection to validate</typeparam>
        /// <param name="collection">The collection to validate.</param>
        /// <param name="collectionName">The name of the collection being validated. If not provided, a default value will be used.</param>
        /// <returns>The collection when it is not null nor empty.</returns>
        /// <exception cref="ArgumentException">Thrown when the collection is null or empty.</exception>
        /// <example>
        /// <code>
        /// List&lt;int&gt; numbers = new List&lt;int&gt; { 1, 2, 3, 4, 5 };
        /// List&lt;int&gt; emptyList = new List&lt;int&gt;();
        /// List&lt;int&gt;? nullList = null;
        /// string name = "numbers";
        /// numbers.ThrowIfNullOrEmpty(name); // Returns numbers
        /// emptyList.ThrowIfNullOrEmpty(); // Throws ArgumentException with message "The collection is null or empty"
        /// nullList.ThrowIfNullOrEmpty("myList"); // Throws ArgumentException with message "myList is null or empty"
        /// </code>
        /// </example>
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
        /// Takes one random element from the collection using the provided <see cref="System.Random"/> instance.
        /// </summary>
        /// <typeparam name="T">Type of the elements in the collection</typeparam>
        /// <param name="collection">The collection to pick an element from.</param>
        /// <param name="random">The <see cref="System.Random"/> instance to use for picking the element.</param>
        /// <returns>A random element from the collection.</returns>
        /// <exception cref="ArgumentException">Thrown when the collection is null or empty.</exception>
        /// <example>
        /// <code>
        /// List&lt;int&gt; numbers = new List&lt;int&gt; { 1, 2, 3, 4, 5 };
        /// Random random = new Random();
        /// int randomElement = numbers.Random(random);
        /// Console.WriteLine($"A random element from {string.Join(", ", numbers)} is {randomElement}."); 
        /// </code>
        /// </example>
        public static T Random<T>(this IReadOnlyCollection<T> collection, Random random)
        {
	        ((ICollection)collection).ThrowIfNullOrEmpty();

            int index = random.Next(collection.Count);

            return collection.ElementAt(index);
        }

        /// <summary>
        /// Takes one random element from the collection.
        /// </summary>
        /// <typeparam name="T">Type of the elements in the collection</typeparam>
        /// <param name="collection">The collection to pick an element from.</param>
        /// <returns>A random element from the collection.</returns>
        /// <exception cref="ArgumentException">Thrown when the collection is null or empty.</exception>
        /// <example>
        /// <code>
        /// List&lt;int&gt; numbers = new List&lt;int&gt; { 1, 2, 3, 4, 5 };
        /// int randomElement = numbers.Random();
        /// Console.WriteLine($"A random element from {string.Join(", ", numbers)} is {randomElement}."); 
        /// </code>
        /// </example>
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
        /// <example>
        /// <code>
        /// List&lt;int&gt; numbers = new List&lt;int&gt; { 1, 2, 3, 4 };
        /// IEnumerable&lt;IEnumerable&lt;int&gt;&gt; permutations = numbers.GetPermutations(2);
        /// foreach (IEnumerable&lt;int&gt; permutation in permutations)
        /// {
        ///     Console.WriteLine(string.Join(", ", permutation));
        /// }
        /// // Output:
        /// // 1, 2
        /// // 1, 3
        /// // 1, 4
        /// // 2, 1
        /// // 2, 3
        /// // 2, 4
        /// // 3, 1
        /// // 3, 2
        /// // 3, 4
        /// // 4, 1
        /// // 4, 2
        /// // 4, 3
        /// </code>
        /// </example>
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
        /// <example>
        /// <code>
        /// List&lt;int&gt; numbers = new List&lt;int&gt; { 1, 2, 3 };
        /// IEnumerable&lt;IEnumerable&lt;int&gt;&gt; permutations = numbers.GetPermutations();
        /// foreach (IEnumerable&lt;int&gt; permutation in permutations)
        /// {
        ///     Console.WriteLine(string.Join(", ", permutation));
        /// }
        /// // Output:
        /// // 1, 2, 3
        /// // 1, 3, 2
        /// // 2, 1, 3
        /// // 2, 3, 1
        /// // 3, 1, 2
        /// // 3, 2, 1
        /// </code>
        /// </example>
        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(this IReadOnlyCollection<T> list)
        {
            var length = list.Count;

            return list.GetPermutations(length);
        }
    }
}

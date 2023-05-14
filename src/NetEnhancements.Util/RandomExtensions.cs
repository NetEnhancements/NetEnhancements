using System.Security.Cryptography;

namespace NetEnhancements.Util
{
    /// <summary>
    /// Provides extension methods for <see cref="Random"/>.
    /// </summary>
    public static class RandomExtensions
    {
        /// <summary>
        /// Generate a hexadecimal token string with the given length, which must be a multiple of 2.
        /// </summary>
        /// <param name="length">The length of the token string, which must be a multiple of 2.</param>
        /// <returns>A random hexadecimal token string of the given length.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the length parameter is not a multiple of 2.</exception>
        public static string GenerateRandomToken(int length)
        {
            if (length % 2 != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), $"Must be even, {length} is odd");
            }

            byte[] tokenData = new byte[length / 2];

            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(tokenData);
            }

            return tokenData.ToHexString();
        }
    }
}

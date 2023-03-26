using System.Security.Cryptography;

namespace NetEnhancements.Util
{
    public class RandomExtensions
    {
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

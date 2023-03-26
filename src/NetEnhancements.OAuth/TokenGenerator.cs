using NetEnhancements.Util;

namespace NetEnhancements.OAuth
{
    /// <summary>
    /// Used for API tokens.
    /// </summary>
    public static class TokenGenerator
    {
        public static (string Key, string Secret, string Hashed) GetRandomHashedKeyAndSecret()
        {
            var key = RandomExtensions.GenerateRandomToken(32);
            var secret = RandomExtensions.GenerateRandomToken(64);

            var hash = Sodium.PasswordHash.ArgonHashString($"{key}:{secret}");

            // Column's max length is 128.
            if (hash.Length > 128)
            {
                throw new InvalidOperationException("Should not happen, try again.");
            }

            return (key, secret, hash);
        }

        public static bool Match(string? key, string? secret, string hash)
        {
            var matchArgon = MatchArgon(key, secret, hash);
            // Check if hash matches Argon else check if it matches BCrypt
            return matchArgon ? matchArgon : MatchBCrypt(key, secret, hash);
        }

        private static bool MatchArgon(string? key, string? secret, string hash)
        {
            return Sodium.PasswordHash.ArgonHashStringVerify(hash, $"{key}:{secret}");
        }

        private static bool MatchBCrypt(string? key, string? secret, string hash)
        {
            return BCrypt.Net.BCrypt.Verify($"{key}:{secret}", hash);
        }
    }
}

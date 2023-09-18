using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace NetEnhancements.Util
{
    /// <summary>
    /// Provides extension methods for <see cref="string"/>s.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Returns the original string, or truncates it to <paramref name="maxLength"/> if it's longer than that.
        /// </summary>
        /// <param name="s">The string to truncate, or <c>null</c>.</param>
        /// <param name="maxLength">The maximum length of the string.</param>
        /// <returns>The original string if its length is less than or equal to <paramref name="maxLength"/>, or a truncated version of the string if its length is greater than <paramref name="maxLength"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the <paramref name="maxLength"/> is less than 0.</exception>
        [return: NotNullIfNotNull(nameof(s))]
        public static string? Truncate(this string? s, int maxLength)
        {
            if (maxLength < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxLength));
            }

            return (s?.Length).GetValueOrDefault() <= maxLength ? s : s![..maxLength];
        }

        /// <summary>
        /// Returns the hexadecimal representation of the input string in UTF-16 bytes.
        /// </summary>
        /// <param name="input">The input string to convert.</param>
        /// <returns>A hexadecimal string that represents the UTF-16 bytes of the input string.</returns>
        public static string ToHexString(this string input)
        {
            var bytes = Encoding.Unicode.GetBytes(input);

            return ToHexString(bytes);
        }

        /// <summary>
        /// Returns the hexadecimal representation of the input bytes.
        /// </summary>
        /// <param name="bytes">The input bytes to convert.</param>
        /// <returns>A hexadecimal string that represents the input bytes.</returns>
        public static string ToHexString(this byte[] bytes)
        {
            var sb = new StringBuilder();

            foreach (var b in bytes)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Given a hexadecimal string, returns the bytes that string represents.
        /// </summary>
        /// <param name="input">The hexadecimal string to convert.</param>
        /// <returns>An array of bytes that represent the input hexadecimal string.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When the length of the input string is odd.</exception>
        public static byte[] ToBytes(this string input)
        {
            if (input.Length % 2 != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(input));
            }

            var bytes = new byte[input.Length / 2];

            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(input.Substring(i * 2, 2), 16);
            }

            return bytes;
        }

        /// <summary>
        /// Given a hexadecimal string, returns the UTF-16 string the bytes therein represent.
        /// </summary>
        /// <param name="input">The hexadecimal string to convert.</param>
        /// <returns>A UTF-16 string that represents the bytes represented by the input hexadecimal string.</returns>
        public static string FromHexString(this string input)
        {
            return Encoding.Unicode.GetString(input.ToBytes());
        }

        /// <summary>
        /// Converts the given string to_snake_case.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>A new string in snake case format.</returns>
        public static string ToSnakeCase(this string str)
        {
            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x : x.ToString())).ToLower();
        }

        /// <summary>
        /// Removes the given suffix from the end of the input string, if it exists.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="suffix">The suffix to remove from the input string.</param>
        /// <param name="comparisonType">The string comparison type to use. The default is InvariantCulture.</param>
        /// <returns>The input string with the suffix removed if it exists, otherwise the original input string.</returns>
        public static string RemoveEnd(this string input, string suffix, StringComparison comparisonType = StringComparison.InvariantCulture)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (suffix == null) throw new ArgumentNullException(nameof(suffix));

            var settingsIndex = input.LastIndexOf(suffix, comparisonType);

            return settingsIndex >= 0 && settingsIndex == input.Length - suffix.Length
                ? input[..settingsIndex]
                : input;
        }

#pragma warning disable CS1574 // XML comment has cref attribute that could not be resolved - just here for documentation
        /// <summary>
        /// Returns <c>true</c> for <see cref="Microsoft.Extensions.Hosting.IHostEnvironment.EnvironmentName"/> strings like "Development", "Docker", "Development.Docker", but not for "Docker.Production".
        /// </summary>
#pragma warning restore CS1574
        public static bool IsDevelopmentOrDocker(this string environmentName) =>
            environmentName.Contains("Develop") ||
            (environmentName.Contains("Docker") && !environmentName.Contains("Production") && !environmentName.Contains("Staging"));
    }
}

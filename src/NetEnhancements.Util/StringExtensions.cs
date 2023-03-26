using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace NetEnhancements.Util
{
    public static class StringExtensions
    {
        [return: NotNullIfNotNull(nameof(s))]
        public static string? Truncate(this string? s, int maxLength)
        {
            if (maxLength < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxLength));
            }

            return (s?.Length).GetValueOrDefault() <= maxLength ? s : s![..maxLength];
        }

        public static string ToHexString(this string input)
        {
            var bytes = Encoding.Unicode.GetBytes(input);

            return ToHexString(bytes);
        }

        public static string ToHexString(this byte[] bytes)
        {
            var sb = new StringBuilder();

            foreach (var b in bytes)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }

        public static string FromHexString(this string input)
        {
            var bytes = new byte[input.Length / 2];

            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(input.Substring(i * 2, 2), 16);
            }

            return Encoding.Unicode.GetString(bytes);
        }

        /// <summary>
        /// to_snake_case
        /// </summary>
        public static string ToSnakeCase(this string str)
        {
            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
        }

        /// <summary>
        /// Removes the given <paramref name="suffix"/> if the <paramref name="input"/> ends with that string.
        /// </summary>
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

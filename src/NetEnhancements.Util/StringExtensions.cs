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

        public static string RemoveEnd(this string input, string endsWith)
        {
            var settingsIndex = input.IndexOf(endsWith);
            if (settingsIndex == input.Length - endsWith.Length)
            {
                return input.Substring(0, settingsIndex);
            }

            return input;
        }

        // ReSharper disable once InvalidXmlDocComment - it's for docs, we don't want to reference it.
        /// <summary>
        /// Returns <c>true</c> for <see cref="Microsoft.Extensions.Hosting.IHostEnvironment.EnvironmentName"/> strings like "Development", "Docker", "Development.Docker", but not for "Docker.Production".
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsDevelopmentOrDocker(this string s) =>
            s.Contains("Develop") ||
            (s.Contains("Docker") && !s.Contains("Production") && !s.Contains("Staging"));
    }
}

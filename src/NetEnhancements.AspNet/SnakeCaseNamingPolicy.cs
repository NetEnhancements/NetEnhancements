using System.Text.Json;
using NetEnhancements.Util;

namespace NetEnhancements.AspNet
{
    /// <summary>
    /// Policy to name JSON properties in snake_case.
    /// </summary>
    public class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        /// <summary>
        /// An instance of the <see cref="SnakeCaseNamingPolicy"/>.
        /// </summary>
        public static SnakeCaseNamingPolicy Instance { get; } = new();

        /// <inheritdoc/>
        public override string ConvertName(string name)
        {
            return name.ToSnakeCase();
        }
    }
}

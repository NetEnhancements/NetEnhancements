using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace NetEnhancements.Util
{
    /// <summary>
    /// Provides extension methods for enums.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Read the <see cref="DisplayAttribute.Name"/> property from an enum member.
        ///
        /// When the member can't be found by name, or when none of the attributes is found, the passed enum member's <see cref="Enum.ToString()"/> will be returned.
        /// </summary>
        /// <example>
        /// <code>
        /// enum Foo
        /// {
        ///     [Display(Name = "Foo-1")]
        ///     Foo1
        /// }
        ///
        /// // will contain "Foo-1".
        /// var foo1Name = Foo.Foo1.GetDisplayName();
        /// </code>
        /// </example>
        public static string GetDisplayName(this Enum enumMember)
        {
            var enumMemberName = enumMember.ToString();

            var member = enumMember.GetType()
                             .GetMember(enumMemberName)
                             .FirstOrDefault();

            if (member == null)
            {
                return enumMemberName;
            }

            var displayAttribute = member.GetCustomAttribute<DisplayAttribute>();

            return displayAttribute?.Name ?? enumMemberName;
        }
    }
}

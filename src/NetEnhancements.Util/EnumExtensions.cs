using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace NetEnhancements.Util
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumMember)
        {
            var enumMemberName = enumMember.ToString();

            var attribute = enumMember.GetType()
                             .GetMember(enumMemberName)
                             .FirstOrDefault()
                             ?.GetCustomAttribute<DisplayAttribute>()
                             ?.Name;

            return attribute ?? enumMemberName;
        }
    }
}

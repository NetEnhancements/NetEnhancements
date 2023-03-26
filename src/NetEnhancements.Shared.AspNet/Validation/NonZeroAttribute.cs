using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace NetEnhancements.Shared.AspNet.Validation
{
    public class NonZeroAttribute : ValidationAttribute
    {
        public NonZeroAttribute()
            : base(() => "The value cannot be equal to zero.")
        {
        }

        public override bool IsValid(object? value)
        {
            return value is IConvertible i && i.ToDecimal(CultureInfo.InvariantCulture) != 0;
        }
    }
}

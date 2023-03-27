using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace NetEnhancements.AspNet.Validation
{
    /// <summary>
    /// Indicates that a numeric property cannot have a value of zero (0).
    /// </summary>
    public class NonZeroAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initializes a new instance with the default error message.
        /// </summary>
        public NonZeroAttribute() : this(() => "The value cannot be equal to zero.") { }

        /// <summary>
        /// Initializes a new instance with the given error message.
        /// </summary>
        public NonZeroAttribute(string errorMessage) : base(errorMessage) { }

        /// <summary>
        /// Initializes a new instance with the given error message accessor.
        /// </summary>
        public NonZeroAttribute(Func<string> errorMessageAccessor) : base(errorMessageAccessor) { }

        /// <inheritdoc/>
        public override bool IsValid(object? value)
        {
            return value is IConvertible i && i.ToDecimal(CultureInfo.InvariantCulture) != 0;
        }
    }
}

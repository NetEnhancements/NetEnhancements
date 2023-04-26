using System.ComponentModel.DataAnnotations;

namespace NetEnhancements.Util.Validation
{
    /// <summary>
    /// Applied to models on which you want to apply a <see cref="VatNumberAttribute"/>.
    /// </summary>
    public interface IVatValidatable
    {
        /// <summary>
        /// The two letter country code of the country to validate the VAT number for.
        /// </summary>
        string? CountryCode { get; }
    }

    /// <summary>
    /// To validate model with a VAT property. Requires the model to implement <see cref="IVatValidatable"/>.
    /// </summary>
    public class VatNumberAttribute : ValidationAttribute
    {
        /// <summary>
        /// Whether an empty VAT number should be considered valid.
        /// </summary>
        public bool AllowEmpty { get; set; }

        /// <summary>
        /// Whether to allow spaces (" ") and dots (".") as separators within a VAT number.
        /// </summary>
        public bool AllowSeparatorChars { get; set; }

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (validationContext.ObjectInstance is not IVatValidatable vatValidatable)
            {
                throw new InvalidOperationException($"The model to be validated needs to be an {nameof(IVatValidatable)}");
            }

            var vatNumber = value as string;

            var countryCode = vatValidatable.CountryCode?.ToUpperInvariant();

            return VatNumberValidator.Validate(vatNumber?.ToUpperInvariant(), countryCode, AllowSeparatorChars, AllowEmpty);
        }
    }
}

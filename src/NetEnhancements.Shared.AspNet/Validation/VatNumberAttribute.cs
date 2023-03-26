using System.ComponentModel.DataAnnotations;
using NetEnhancements.Util;

namespace NetEnhancements.Shared.AspNet.Validation
{
    /// <summary>
    /// Applied to models on which you want to apply a <see cref="VatNumberAttribute"/>.
    /// </summary>
    public interface IVatValidatable
    {
        string? CountryCode { get; }
    }

    /// <summary>
    /// To validate model with a VAT property. Requires the model to implement <see cref="IVatValidatable"/>.
    /// </summary>
    public class VatNumberAttribute : ValidationAttribute
    {
        public bool AllowEmpty { get; set; }

        public bool AllowSeparatorChars { get; set; }

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

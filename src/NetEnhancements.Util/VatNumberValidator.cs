using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace NetEnhancements.Util
{
    public static class VatNumberValidator
    {
        private static readonly Dictionary<string, string> CountryVatRegex = new()
        {
            // EU
            { "AT", "ATU[0-9]{8}" },
            { "BE", "BE[01][0-9]{9}" },
            { "BG", "BG[0-9]{9,10}" },
            { "HR", "HR[0-9]{11}" },
            { "CY", "CY[0-9]{8}[A-Z]" },
            { "CZ", "CZ[0-9]{8,10}" },
            { "DK", "DK[0-9]{8}" },
            { "EE", "EE[0-9]{9}" },
            { "FI", "FI[0-9]{8}" },
            { "FR", "FR[A-Z0-9]{2}[0-9]{9}" },
            { "DE", "DE[0-9]{9}" },
            { "EL", "EL[0-9]{9}" },
            { "HU", "HU[0-9]{8}" },
            { "IE", "IE([0-9][A-Z][0-9]{5}[A-Z]|[0-9]{7}[A-Z]{1,2})" },
            { "IT", "IT[0-9]{11}" },
            { "LV", "LV[0-9]{11}" },
            { "LT", "LT([0-9]{9}|[0-9]{12})" },
            { "LU", "LU[0-9]{8}" },
            { "MT", "MT[0-9]{8}" },
            { "NL", "NL[0-9]{9}B[0-9]{2}" },
            { "PL", "PL[0-9]{10}" },
            { "PT", "PT[0-9]{9}" },
            { "RO", "RO[0-9]{2,10}" },
            { "SK", "SK[0-9]{10}" },
            { "SI", "SI[0-9]{8}" },
            { "ES", "ES[A-Z0-9][0-9]{7}[A-Z0-9]" },
            { "SE", "SE[0-9]{12}" },
            
            // Non-EU
            { "CA", "[A-Z0-9]{9}" },
            { "CH", "CHE[0-9]{9}(MWST|TVA|IVA)" },
            { "GB", "GB([0-9]{9}|[0-9]{12}|[A-Z]{2}[0-9]{3})" },
            { "US", "[0-9]{5,15}" },
        };

        public static ValidationResult? Validate(string? vatNumber, string? countryCode, bool allowSeparatorChars, bool allowEmpty)
        {
            if (countryCode == null || !CountryVatRegex.TryGetValue(countryCode, out var regex))
            {
                return new ValidationResult($"Unknown country '{countryCode}'");
            }

            if (string.IsNullOrEmpty(vatNumber))
            {
                return allowEmpty
                    ? ValidationResult.Success
                    : new ValidationResult("The VAT number is required");
            }

            if (allowSeparatorChars)
            {
                if (vatNumber.Contains("  ") || vatNumber.Contains("..") || vatNumber.Contains(". ") || vatNumber.Contains(" ."))
                {
                    return new ValidationResult("The VAT number can't contain repeated separators");
                }

                vatNumber = vatNumber.Replace(" ", null).Replace(".", null);

                if (string.IsNullOrWhiteSpace(vatNumber))
                {
                    return new ValidationResult("The VAT number can't exist of just separators");
                }
            }

            return Regex.IsMatch(vatNumber, $"^{regex}$")
                ? ValidationResult.Success
                : new ValidationResult($"The VAT number does not adhere to the provided country's format ({regex})");
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace NetEnhancements.Util.Tests
{
    public class VatNumberValidatorTests
    {
        [Test]
        public void Validate_Null_CountryCode_Invalid()
        {
            var valid = VatNumberValidator.Validate("VAT123", null, allowSeparatorChars: true, allowEmpty: false);

            Assert.That(valid, Is.Not.Null);
            Assert.That(valid!.ErrorMessage, Contains.Substring("country"));
        }

        [Test]
        public void Validate_Unknown_CountryCode_Throws()
        {
            var valid = VatNumberValidator.Validate("VAT123", "XXXX", allowSeparatorChars: true, allowEmpty: false);

            Assert.That(valid, Is.Not.Null);
            Assert.That(valid!.ErrorMessage, Contains.Substring("country"));
        }

        [Test]
        [TestCaseSource(nameof(EUVatData))]
        [TestCaseSource(nameof(NonEUVatData))]
        public void VatNumber_ByCountryCode_IsValid_OrNot((string CountryCode, string VatNumber, bool ShouldBeValid) testData)
        {
            var validationResult = VatNumberValidator.Validate(testData.VatNumber, testData.CountryCode, allowSeparatorChars: false, allowEmpty: false);

            if (testData.ShouldBeValid)
            {
                Assert.That(validationResult, Is.EqualTo(ValidationResult.Success));
            }
            else
            {
                Assert.That(validationResult!.ErrorMessage, Is.Not.Empty);
            }
        }

        [Test]
        [TestCaseSource(nameof(SeparatorData))]
        public void VatNumberSeparator((string? VatNumber, bool AllowSeparatorChars, bool AllowEmpty, bool ShouldBeValid) testData)
        {
            var validationResult = VatNumberValidator.Validate(testData.VatNumber, "NL", testData.AllowSeparatorChars, testData.AllowEmpty);

            if (testData.ShouldBeValid)
            {
                Assert.That(validationResult, Is.EqualTo(ValidationResult.Success));
            }
            else
            {
                Assert.That(validationResult!.ErrorMessage, Is.Not.Empty);
            }
        }

        private static List<(string? VatNumber, bool AllowSeparatorChars, bool AllowEmpty, bool ShouldBeValid)> SeparatorData()
        {
            return new List<(string?, bool, bool, bool)>
            {
                // Valid
                new ("NL123456789B01", false, false, true),
                new ("NL 123 456 789.B01", true, false, true),
                new ("", false, true, true),
                new (null, false, true, true),
                
                // Invalid
                new ("", true, false, false),
                new (" ", true, true, false),
                new ("  ", true, true, false),
                new (".", true, true, false),
                new (" .", true, true, false),
                new (null, true, false, false),
                new ("  NL 123 456 789..B01", true, false, false),
                new ("NL 123456789. B01", true, false, false),
            };
        }

        // ReSharper disable once InconsistentNaming - EU
        private static List<(string, string, bool)> EUVatData()
        {
            return new List<(string, string, bool)>
            {
                new ("AT", "geen-vat", false),
                new ("AT", "ATU12345678", true),
                new ("BE", "geen-vat", false),
                new ("BE", "BE0123456789", true),
                new ("BE", "BE1123456789", true),
                new ("BG", "geen-vat", false),
                new ("BG", "BG123456789", true),
                new ("BG", "BG1234567890", true),
                new ("HR", "geen-vat", false),
                new ("HR", "HR12345678901", true),
                new ("CY", "geen-vat", false),
                new ("CY", "CY12345678A", true),
                new ("CY", "CY12345678Z", true),
                new ("CZ", "geen-vat", false),
                new ("CZ", "CZ12345678", true),
                new ("CZ", "CZ123456789", true),
                new ("CZ", "CZ1234567890", true),
                new ("DK", "geen-vat", false),
                new ("DK", "DK12345678", true),
                new ("EE", "geen-vat", false),
                new ("EE", "EE123456789", true),
                new ("FI", "geen-vat", false),
                new ("FI", "FI12345678", true),
                new ("FR", "geen-vat", false),
                new ("FR", "FRAA123456789", true),
                new ("FR", "FR99123456789", true),
                new ("FR", "FRA9123456789", true),
                new ("FR", "FR9A123456789", true),
                new ("DE", "geen-vat", false),
                new ("DE", "DE123456789", true),
                new ("EL", "geen-vat", false),
                new ("EL", "EL123456789", true),
                new ("HU", "geen-vat", false),
                new ("HU", "HU12345678", true),
                new ("IE", "geen-vat", false),
                new ("IE", "IE1A12345A", true),
                new ("IE", "IE1234567A", true),
                new ("IE", "IE1234567AB", true),
                new ("IT", "geen-vat", false),
                new ("IT", "IT12345678901", true),
                new ("LV", "geen-vat", false),
                new ("LV", "LV12345678901", true),
                new ("LT", "geen-vat", false),
                new ("LT", "LT123456789", true),
                new ("LT", "LT123456789012", true),
                new ("LU", "geen-vat", false),
                new ("LU", "LU12345678", true),
                new ("MT", "geen-vat", false),
                new ("MT", "MT12345678", true),
                new ("NL", "geen-vat", false),
                new ("NL", "NL123456789B01", true),
                new ("PL", "geen-vat", false),
                new ("PL", "PL1234567890", true),
                new ("PT", "geen-vat", false),
                new ("PT", "PT123456789", true),
                new ("RO", "geen-vat", false),
                new ("RO", "RO12", true),
                new ("RO", "RO123456", true),
                new ("RO", "RO1234567890", true),
                new ("SK", "geen-vat", false),
                new ("SK", "SK1234567890", true),
                new ("SI", "geen-vat", false),
                new ("SI", "SI12345678", true),
                new ("ES", "geen-vat", false),
                new ("ES", "ES112345671", true),
                new ("ES", "ES11234567A", true),
                new ("ES", "ESA12345671", true),
                new ("ES", "ESA1234567A", true),
                new ("SE", "geen-vat", false),
                new ("SE", "SE123456789012", true),
            };
        }

        // ReSharper disable once InconsistentNaming - EU
        private static List<(string, string, bool)> NonEUVatData()
        {
            return new List<(string, string, bool)>
            {
                new ("CA", "geen-vat", false),
                new ("CA", "123ABC789", true),
                new ("CH", "geen-vat", false),
                new ("CH", "CHE123456789MWST", true),
                new ("CH", "CHE123456789TVA", true),
                new ("CH", "CHE123456789IVA", true),
                new ("GB", "geen-vat", false),
                new ("GB", "GB123456789", true),
                new ("GB", "GB123456789012", true),
                new ("GB", "GBAA123", true),
                new ("GB", "GBZZ987", true),
                new ("US", "geen-vat", false),
                new ("US", "12345", true),
                new ("US", "1234567890", true),
                new ("US", "123456789012345", true),
            };
        }
    }
}

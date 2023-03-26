using System.ComponentModel.DataAnnotations;

namespace NetEnhancements.Util
{
    public static class AttributeValidator
    {
        /// <summary>
        /// Validates an object according to its attributes.
        /// </summary>
        public static (bool IsValid, List<ValidationResult> ValidationResults) Validate<T>(T instance)
            where T : class
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(instance);
            var isValid = Validator.TryValidateObject(instance, context, validationResults, validateAllProperties: true);

            return (isValid, validationResults);
        }
    }
}

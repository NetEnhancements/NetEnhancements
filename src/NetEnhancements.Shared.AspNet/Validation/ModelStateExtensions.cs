using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NetEnhancements.Shared.AspNet.Validation
{
    public static class ModelStateExtensions
    {
        private const string UnknownErrorMessage = "Unknown error";

        /// <summary>
        /// Adds the error results from <see cref="NetEnhancements.Util.AttributeValidator.Validate{T}"/> to the <paramref name="modelState"/>, prefixed with the member name <paramref name="prefix"/>.
        /// 
        /// In the case of an empty error message provided by a validator, the <paramref name="unknownErrorMessage"/> will be added, which defaults to "Unknown error" when not specified otherwise.
        /// </summary>
        public static void AddValidationErrors(this ModelStateDictionary modelState, string prefix, List<ValidationResult> errors, string? unknownErrorMessage = null)
        {
            ArgumentNullException.ThrowIfNull(modelState);
            ArgumentNullException.ThrowIfNull(prefix);
            ArgumentNullException.ThrowIfNull(errors);

            foreach (var error in errors)
            {
                foreach (var memberName in error.MemberNames)
                {
                    var propertyName = prefix + "." + memberName;

                    modelState.TryAddModelError(propertyName, error.ErrorMessage ?? unknownErrorMessage ?? UnknownErrorMessage);
                }
            }
        }
    }
}

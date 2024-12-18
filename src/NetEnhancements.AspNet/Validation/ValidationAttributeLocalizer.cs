﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.Options;

namespace NetEnhancements.AspNet.Validation
{
    /// <summary>
    /// Options class to configure the validation attribute localization.
    /// </summary>
    public class ValidationAttributeLocalizerOptions
    {
        /// <summary>
        /// The resource type containing strings for validation attribute error message translations.
        /// </summary>
        public Type? ValidationErrorResourceType { get; set; }
    }

    /// <summary>
    /// Enables translation of ValidationAttributes.
    ///
    /// Kinda hacky, kinda inspired by ForEvolve/ForEvolve.AspNetCore.Localization.
    /// </summary>
    internal class ValidationAttributeLocalizer : IValidationMetadataProvider
    {
        public Type ErrorMessageResourceType { get; }

        public ValidationAttributeLocalizer(IOptions<ValidationAttributeLocalizerOptions> options)
        {
            ErrorMessageResourceType = options.Value?.ValidationErrorResourceType
                                       ?? throw new ArgumentNullException(nameof(ValidationAttributeLocalizerOptions.ValidationErrorResourceType));
        }

        public void CreateValidationMetadata(ValidationMetadataProviderContext context)
        {
            var localizableAttributes = context.Attributes
                .OfType<ValidationAttribute>()
                .Where(a => string.IsNullOrWhiteSpace(a.ErrorMessage));

            foreach (var validationAttribute in localizableAttributes)
            {
                validationAttribute.ErrorMessageResourceType = ErrorMessageResourceType;
                validationAttribute.ErrorMessageResourceName = GetErrorMessageResourceName(validationAttribute);
            }
        }

        private static string? GetErrorMessageResourceName(ValidationAttribute validationAttribute)
        {
            var resourceName = validationAttribute.GetType().Name + "_ValidationError";

            return validationAttribute is StringLengthAttribute { MinimumLength: > 0 }
                ? resourceName + "IncludingMinimum"
                : resourceName;
        }
    }
}

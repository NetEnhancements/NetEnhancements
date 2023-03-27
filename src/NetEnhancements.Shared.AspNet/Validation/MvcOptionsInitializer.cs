using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace NetEnhancements.AspNet.Validation
{
    /// <summary>
    /// Registers our localizer with MVC, to translate errors while model binding.
    /// </summary>
    internal class MvcOptionsInitializer : IConfigureOptions<MvcOptions>
    {
        private readonly ValidationAttributeLocalizer _localizer;

        public MvcOptionsInitializer(ValidationAttributeLocalizer localizer)
        {
            _localizer = localizer;
        }

        public void Configure(MvcOptions options)
        {
            options.ModelMetadataDetailsProviders.Add(_localizer);
        }
    }
}

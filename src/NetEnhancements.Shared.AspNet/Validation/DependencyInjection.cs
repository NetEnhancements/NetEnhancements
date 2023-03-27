using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace NetEnhancements.AspNet.Validation
{
    public static class DependencyInjection
    {
        public static IMvcBuilder AddValidationAttributeLocalization(this IMvcBuilder builder, Action<ValidationAttributeLocalizerOptions> setupAction)
        {
            builder.Services.Configure(setupAction);

            builder.Services.AddSingleton<ValidationAttributeLocalizer>();

            builder.Services.AddSingleton<IConfigureOptions<MvcOptions>, MvcOptionsInitializer>();

            return builder;
        }
    }
}

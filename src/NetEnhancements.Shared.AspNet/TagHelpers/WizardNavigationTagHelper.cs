using Microsoft.AspNetCore.Razor.TagHelpers;

namespace NetEnhancements.Shared.AspNet.TagHelpers
{
    public class WizardNavigationTagHelper : TagHelper
    {
        private string GetButtonClass(int step)
        {
            const string currentStepClass = "btn btn-primary active";
            const string enabledStepClass = "btn btn-outline-primary";
            const string disabledStepClass = "btn btn-outline-primary disabled";

            if (step == CurrentStep)
            {
                return currentStepClass;
            }

            if (step < CurrentStep && !string.IsNullOrWhiteSpace(Url) || EnableAllSteps)
            {
                return enabledStepClass;
            }

            return disabledStepClass;
        }

        public int Step { get; set; }

        public int CurrentStep { get; set; }

        public bool EnableAllSteps { get; set; }

        public string? Url { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";

            output.Attributes.Add("class", GetButtonClass(Step));

            if (CurrentStep > Step || EnableAllSteps)
            {
                if (string.IsNullOrWhiteSpace(Url))
                {
                    throw new ArgumentNullException(nameof(Url), $"Step {Step} needs to have a {nameof(Url)}");
                }

                output.Attributes.Add("href", Url);
            }
        }
    }
}

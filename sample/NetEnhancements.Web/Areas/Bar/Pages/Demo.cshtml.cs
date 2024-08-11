using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NetEnhancements.Web.Areas.Bar.Pages
{
    public class DemoModel : PageModel
    {
        private readonly ILogger<DemoModel> _logger;

        public DemoModel(ILogger<DemoModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}
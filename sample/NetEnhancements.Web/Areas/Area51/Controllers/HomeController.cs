using Microsoft.AspNetCore.Mvc;

namespace NetEnhancements.Web.Areas.Area51
{
    /// <summary>
    /// Controller demonstrating area namespace conventions, <see cref="NetEnhancements.AspNet.MvcOptionsExtensions.UseAreaControllerNamespace"/>.
    /// </summary>
    [Area("Area51")]
    public class HomeController : Controller
    {
        /// <summary>
        /// GET /Area51/
        /// </summary>
        [HttpGet]
        public IActionResult Index(int? id = 42)
        {
            return Content($"You have reached the Index({id}) action method in the Home controller in the Area51 area.");
        }
    }
}

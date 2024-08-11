using Microsoft.AspNetCore.Mvc;

namespace NetEnhancements.Web.Areas.Bar
{
    /// <summary>
    /// Controller demonstrating area namespace conventions, <see cref="NetEnhancements.AspNet.MvcOptionsExtensions.UseAreaControllerNamespace"/>.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// GET /Bar/
        /// </summary>
        [HttpGet]
        public IActionResult Index(int? id = 42)
        {
            return Content($"You have reached the Index({id}) action method in the Home controller in the Bar area.");
        }
    }
}

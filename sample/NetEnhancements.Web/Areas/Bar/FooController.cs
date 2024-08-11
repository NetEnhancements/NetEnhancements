using Microsoft.AspNetCore.Mvc;

namespace NetEnhancements.Web.Areas.Bar
{
    /// <summary>
    /// Controller demonstrating area namespace conventions, <see cref="NetEnhancements.AspNet.MvcOptionsExtensions.UseAreaControllerNamespace"/>.
    /// </summary>
    public class FooController : Controller
    {
        /// <summary>
        /// GET /Bar/Foo
        /// </summary>
        [HttpGet]
        public IActionResult Index(int? id = 42)
        {
            return Content($"You have reached the Index({id}) action method in the Foo controller in the Bar area.");
        }

        /// <summary>
        /// GET|POST /Bar/Foo/Baz
        /// </summary>
        [HttpGet]
        [HttpPost]
        public IActionResult Baz(int? id = 42)
        {
            return Content($"You have reached the Baz({id}) action method in the Foo controller in the Bar area.");
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace NetEnhancements.Web.Controllers
{
    public class FooController : Controller
    {
        public IActionResult Index()
        {
            return Content("/Foo");
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace NetEnhancements.Web.Areas.Bar
{
    public class FooController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}

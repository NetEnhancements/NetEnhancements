using Microsoft.AspNetCore.Mvc;
using NetEnhancements.AspNet;

namespace NetEnhancements.Web.Controllers
{
    public class FooController : Controller
    {
        public IActionResult Index()
        {
            var clientIp = Request.HttpContext.GetRequestIPAddress();

            return Content(clientIp);
        }
    }
}

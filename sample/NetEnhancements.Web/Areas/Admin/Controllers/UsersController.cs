using Microsoft.AspNetCore.Mvc;

namespace NetEnhancements.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller demonstrating area namespace conventions and policies, <see cref="NetEnhancements.AspNet.MvcOptionsExtensions.UseAreaControllerNamespace"/>.
    /// </summary>
    public class UsersController : Controller
    {
        /// <summary>
        /// GET /Admin/Users
        /// </summary>
        [HttpGet]
        public IActionResult Index(int? id = 42)
        {
            return Content($"You have reached the Index({id}) action method in the User controller in the Admin area.");
        }
    }
}

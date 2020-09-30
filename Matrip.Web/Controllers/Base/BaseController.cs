using Microsoft.AspNetCore.Mvc;

namespace Matrip.Web.Controllers.Base
{
    public class BaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
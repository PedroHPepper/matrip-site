using Microsoft.AspNetCore.Mvc;

namespace Matrip.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Matrip.Web.Controllers
{
    public class FeedbackController : Controller
    {
        public IActionResult Index(int PurchaseID)
        {

            return View();
        }
    }
}
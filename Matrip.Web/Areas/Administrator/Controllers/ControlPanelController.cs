using Matrip.Web.Libraries.Filter;
using Matrip.Web.Libraries.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;

namespace Matrip.Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [AdministratorAuthentication]
    public class ControlPanelController : Controller
    {
        private IConfiguration _configuration;
        private UserLogin _userLogin;

        private HttpClient client;
        public ControlPanelController(IConfiguration configuration, UserLogin UserLogin)
        {
            _configuration = configuration;
            _userLogin = UserLogin;
            if (client == null)
            {
                client = new HttpClient();
                client.BaseAddress = new Uri(_configuration.GetValue<string>("Matrip.API:ApiLink"));
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            }
        }
        public IActionResult Index()//int? page)
        {
            var ma05trip = 1;
            return View();


            //return View(ma06tripcategory);
        }

    }
}
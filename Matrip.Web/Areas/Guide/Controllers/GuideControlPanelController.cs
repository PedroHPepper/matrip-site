using Matrip.Domain.Models.AccountModels;
using Matrip.Domain.Models.Entities;
using Matrip.Web.Libraries.Filter;
using Matrip.Web.Libraries.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Matrip.Web.Areas.Administrator.Controllers
{
    [Area("Guide")]
    [GuideAuthentication]
    public class GuideControlPanelController : Controller
    {
        private UserLogin _userLogin;
        private IConfiguration _configuration;
        private HttpClient client;

        public GuideControlPanelController(UserLogin UserLogin, IConfiguration configuration)
        {
            _userLogin = UserLogin;
            _configuration = configuration;
            if (client == null)
            {
                client = new HttpClient();
                client.BaseAddress = new Uri(_configuration.GetValue<string>("Matrip.API:ApiLink"));
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            }
        }
        public IActionResult Index(int? page)
        {
            //var ma05trip = 1;// = _tripRepository.GetList(page);
            return View();

        }
        




    }
}
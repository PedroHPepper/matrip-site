using Matrip.Domain.Models.AccountModels;
using Matrip.Domain.Models.Entities;
using Matrip.Domain.Models.TripModel;
using Matrip.Web.Domain.Models;
using Matrip.Web.Libraries.Archive;
using Matrip.Web.Libraries.Filter;
using Matrip.Web.Libraries.Login;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Matrip.Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [AdministratorAuthentication]
    public class AdminTripController : Controller
    {
        private IHostingEnvironment _env;
        private IConfiguration _configuration;
        private UserLogin _userLogin;

        private HttpClient client;
        public AdminTripController(IHostingEnvironment env, IConfiguration configuration, UserLogin UserLogin)
        {
            _env = env;
            _configuration = configuration;
            _userLogin = UserLogin;
            if (client == null)
            {
                client = new HttpClient();
                client.BaseAddress = new Uri(_configuration.GetValue<string>("Matrip.API:ApiLink"));
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            }
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AddTrip()
        {
            return View("AddTripView");
        }
        [HttpPost]
        [ValidateHttpReferer]
        public async Task<IActionResult> AddTrip(TripModel tripModel)
        {
            if (ModelState.IsValid)
            {
                TokenModel JWToken = _userLogin.GetToken();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);

                string json = JsonConvert.SerializeObject(tripModel);
                HttpResponseMessage response = await client.PostAsync("Trip/RegisterTrip", new StringContent(json, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    ViewData["MSG_S"] = "Passeio adicionado com sucesso";
                }
                else if(response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _userLogin.Logout();
                    return RedirectToAction("Index", "Home", new { Area = "" });
                }
                else
                {
                    ViewData["MSG_E"] = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }
            }
            return View("AddTripView");
        }

        

        [HttpGet]
        public IActionResult SearchTrip()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> UpdateTrip(string TripName)
        {
            TokenModel JWToken = _userLogin.GetToken();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);
            HttpResponseMessage response = await client.GetAsync("Trip/GetTripModel?TripName=" + HttpUtility.UrlEncode(TripName));
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                TripModel tripModel = JsonConvert.DeserializeObject<TripModel>(result); 
                return View(tripModel);
            }
            else
            {
                TempData["MSG_E"] = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return RedirectToAction("SearchTrip");
            }
        }
        [HttpPost]
        [ValidateHttpReferer]
        public async Task<IActionResult> UpdateTrip(TripModel tripModel)
        {
            TokenModel JWToken = _userLogin.GetToken();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);

            string json = JsonConvert.SerializeObject(tripModel);
            HttpResponseMessage response = await client.PostAsync("Trip/UpdateTrip", new StringContent(json, Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ViewTrip", "Trip", new { TripID = tripModel.ma05trip.ma05idtrip, Area = "" });
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _userLogin.Logout();
                return RedirectToAction("Index", "Home", new { Area = "" });
            }
            else
            {
                TempData["MSG_E"] = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return RedirectToAction("SearchTrip");
            }
        }

        

        

        public IActionResult GetTrips(int? page)
        {
            var trips = 1;// = _tripRepository.GetList(page);
            return View(trips);
        }
    }
}
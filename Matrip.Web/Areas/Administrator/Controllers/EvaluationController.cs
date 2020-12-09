using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Matrip.Domain.Models.AccountModels;
using Matrip.Domain.Models.Entities;
using Matrip.Web.Libraries.Filter;
using Matrip.Web.Libraries.Login;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Matrip.Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [AdminGuideAuthentication]
    public class EvaluationController : Controller
    {
        private IHostingEnvironment _env;
        private IConfiguration _configuration;
        private UserLogin _userLogin;

        private HttpClient client;
        public EvaluationController(IHostingEnvironment env, IConfiguration configuration, UserLogin UserLogin)
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


        [HttpGet]
        public IActionResult SearchTrip()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ViewTripEvaluations(string TripName)
        {
            TokenModel JWToken = _userLogin.GetToken();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);
            HttpResponseMessage response = await client.GetAsync("Feedback/GetAllEvaluations?TripName=" + HttpUtility.UrlEncode(TripName));
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                ma05trip EvaluatedTrip = JsonConvert.DeserializeObject<ma05trip>(result);
                return View(EvaluatedTrip);
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

        [HttpGet]
        public async Task<IActionResult> ApproveEvaluation(string TripName, int EvaluationID)
        {
            TokenModel JWToken = _userLogin.GetToken();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);
            HttpResponseMessage response = await client.GetAsync("Feedback/ApproveEvaluation/"+EvaluationID);
            if (response.IsSuccessStatusCode)
            {
                TempData["MSG_S"] = "Avaliação aprovada com sucesso!";
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _userLogin.Logout();
                return RedirectToAction("Index", "Home", new { Area = "" });
            }
            else
            {
                TempData["MSG_E"] = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }
            return RedirectToAction("ViewTripEvaluations", new { Area = "Administrator", TripName });
        }

        [HttpGet]
        public async Task<IActionResult> DisapproveEvaluation(string TripName, int EvaluationID)
        {
            TokenModel JWToken = _userLogin.GetToken();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);
            HttpResponseMessage response = await client.GetAsync("Feedback/DisapproveEvaluation/" + EvaluationID);
            if (response.IsSuccessStatusCode)
            {
                TempData["MSG_S"] = "Avaliação aprovada com sucesso!";
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _userLogin.Logout();
                return RedirectToAction("Index", "Home", new { Area = "" });
            }
            else
            {
                TempData["MSG_E"] = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }
            return RedirectToAction("ViewTripEvaluations", new { Area = "Administrator", TripName });
        }


    }
}
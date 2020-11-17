using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Matrip.Domain.Libraries.Text;
using Matrip.Domain.Models.AccountModels;
using Matrip.Domain.Models.Entities;
using Matrip.Web.Libraries.Archive;
using Matrip.Web.Libraries.Filter;
using Matrip.Web.Libraries.Login;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Matrip.Web.Controllers
{
    [UserAuthentication]
    public class FeedbackController : Controller
    {
        private IConfiguration _configuration;
        private UserLogin _userLogin;

        private HttpClient client;
        public FeedbackController(IConfiguration configuration, UserLogin userLogin)
        {
            _configuration = configuration;
            _userLogin = userLogin;

            if (client == null)
            {
                client = new HttpClient();
                client.BaseAddress = new Uri(_configuration.GetValue<string>("Matrip.API:ApiLink"));
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            }
        }

        [HttpGet]
        [ValidateHttpReferer]
        public async Task<IActionResult> Index(int SaleID)
        {
            TokenModel JWToken = _userLogin.GetToken();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);
            HttpResponseMessage response = await client.GetAsync("Feedback/GetConcludedSales/"+SaleID);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                ma32sale sale = JsonConvert.DeserializeObject<ma32sale>(result);

                return View(sale);
            }
            TempData["MSG_E"] = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return RedirectToAction("GetSaleList", "Purchase", new { page = 1 });
        }

        [HttpGet]
        [ValidateHttpReferer]
        public async Task<IActionResult> EvaluateTrip(int SaleTripID)
        {
            TokenModel JWToken = _userLogin.GetToken();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);
            HttpResponseMessage response = await client.GetAsync("Feedback/GetConcludedSaleTrip/" + SaleTripID);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                ma21saleTrip saleTrip = JsonConvert.DeserializeObject<ma21saleTrip>(result);
                if(saleTrip.ma21date < DateConvert.HrBrasilia() &&
                                    saleTrip.ma39tripEvaluation.Count() == 0)
                {
                    ViewBag.SaleTrip = saleTrip;
                    return View();
                }
                TempData["MSG_E"] = "Ocorreu um erro!";
                return RedirectToAction("Index", "Feedback", new { SaleID = saleTrip.FK2132idSale });
            }
            TempData["MSG_E"] = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return RedirectToAction("GetSaleList", "Purchase", new { page = 1 });
        }

        [HttpPost]
        [ValidateHttpReferer]
        public async Task<IActionResult> EvaluateTrip([FromForm] ma39tripEvaluation tripEvaluation, [FromForm] IFormFile file)
        {
            TokenModel JWToken = _userLogin.GetToken();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);

            string json = JsonConvert.SerializeObject(tripEvaluation);
            HttpResponseMessage response = await client.PostAsync("Feedback/PostFeedback", new StringContent(json, Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                if (file != null)
                {
                    tripEvaluation.ma39photoQuantity = 1;
                    await PhotoManager.AddFeedbackImage(file, tripEvaluation);
                }
                TempData["MSG_S"] = "Obrigado por sua avaliação.";
            }
            else
            {
                TempData["MSG_E"] = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }
            return RedirectToAction("GetSaleList", "Purchase", new { page = 1 });
        }
    }
}
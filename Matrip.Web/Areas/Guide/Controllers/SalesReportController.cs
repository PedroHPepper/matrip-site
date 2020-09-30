using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Matrip.Domain.Models.AccountModels;
using Matrip.Domain.Models.Entities;
using Matrip.Domain.Models.GuideModels;
using Matrip.Web.Libraries.Filter;
using Matrip.Web.Libraries.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Matrip.Web.Areas.Guide.Controllers
{
    [Area("Guide")]
    [GuideAuthentication]
    public class SalesReportController : Controller
    {
        private UserLogin _userLogin;
        private IConfiguration _configuration;
        private HttpClient client;

        public SalesReportController(UserLogin UserLogin, IConfiguration configuration)
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
        [HttpGet]
        public async Task<IActionResult> SearchPartner()
        {
            TokenModel JWToken = _userLogin.GetToken();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);
            HttpResponseMessage response = await client.GetAsync("Guide/GetPartners");
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                ma04guide guide = JsonConvert.DeserializeObject<ma04guide>(result);

                return View(guide);
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Index", "Home", new { Area = "" });
            }
            else
            {
                return RedirectToAction("Index", "Home", new { Area = "" });
            }
        }
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery]int partnerID, [FromQuery] DateTime initialDate, [FromQuery]DateTime finalDate,
            [FromQuery] int DateType)
        {
            TokenModel JWToken = _userLogin.GetToken();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);

            HttpResponseMessage response = await client.GetAsync("Guide/PartnerReport?partnerID=" + partnerID + "&initialDate="+ initialDate.ToString("yyyy-MM-dd") + 
                                                                    "&finalDate="+ finalDate.ToString("yyyy-MM-dd") + "&DateType=" + DateType);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                SaleReport saleReport = JsonConvert.DeserializeObject<SaleReport>(result);
                ViewBag.partnerID = partnerID;
                return View(saleReport);
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Index", "Home", new { Area = "" });
            }
            else
            {
                return RedirectToAction("SearchPartner", "SalesReport", new { Area = "Guide" });
            }
        }
    }
}
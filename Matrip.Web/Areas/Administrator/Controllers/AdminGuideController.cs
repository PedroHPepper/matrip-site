using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Matrip.Domain.Models.AccountModels;
using Matrip.Domain.Models.GuideModels;
using Matrip.Web.Libraries.Filter;
using Matrip.Web.Libraries.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Matrip.Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [AdministratorAuthentication]
    public class AdminGuideController : Controller
    {
        private UserLogin _userLogin;
        private IConfiguration _configuration;
        private HttpClient client;
        public AdminGuideController(UserLogin UserLogin, IConfiguration configuration)
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
        public IActionResult AddGuide()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddGuide(GuideModel guide)
        {
            TokenModel JWToken = _userLogin.GetToken();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);

            string json = JsonConvert.SerializeObject(guide);
            HttpResponseMessage response = await client.PostAsync("Partner/CreateGuide", new StringContent(json, Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                TempData["MSG_S"] = "Guia adicionado com sucesso!";
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
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> RemoveGuide(string GuideEmail)
        {
            TokenModel JWToken = _userLogin.GetToken();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);

            string json = JsonConvert.SerializeObject(GuideEmail);
            HttpResponseMessage response = await client.PostAsync("Partner/RemoveGuide", new StringContent(json, Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                TempData["MSG_S"] = "Guia excluído com sucesso!";
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
            return RedirectToAction("AddGuide", "AdminGuide", new { Area = "Administrator" });
        }
    }
}
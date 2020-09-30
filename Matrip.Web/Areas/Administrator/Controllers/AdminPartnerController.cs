using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Matrip.Domain.Models.AccountModels;
using Matrip.Domain.Models.Entities;
using Matrip.Domain.Models.GuideModels;
using Matrip.Domain.Models.PartnerModels;
using Matrip.Web.Libraries.Filter;
using Matrip.Web.Libraries.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Matrip.Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [AdministratorAuthentication]
    public class AdminPartnerController : Controller
    {
        private UserLogin _userLogin;
        private IConfiguration _configuration;
        private HttpClient client;

        public AdminPartnerController(UserLogin UserLogin, IConfiguration configuration)
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
        public async Task<IActionResult> GetPartners()
        {
            TokenModel JWToken = _userLogin.GetToken();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);

            HttpResponseMessage response = await client.GetAsync("Partner/GetPartnerList");
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                List<ma25partner> partnerList = JsonConvert.DeserializeObject<List<ma25partner>>(result);
                ViewBag.Partners = partnerList;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _userLogin.Logout();
                return RedirectToAction("Index", "Home", new { Area = "" });
            }
            else
            {
                ViewData["MSG_E"] = "Não foi possível encontrar os parceiros!";
            }

            return View();
        }

        [HttpGet]
        public IActionResult AddPartner()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddPartner(PartnerModel partner)
        {
            TokenModel JWToken = _userLogin.GetToken();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);

            string json = JsonConvert.SerializeObject(partner);
            HttpResponseMessage response = await client.PostAsync("Partner/AddPartner", new StringContent(json, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                TempData["MSG_S"] = "Parceiro Cadastrado com Sucesso!";
            }
            else
            {
                ViewData["MSG_E"] = "Ocorreu um Erro!";
                return View();
            }
            return RedirectToAction("GetPartners","AdminPartner", new { Area = "Administrator"});
        }

        [HttpGet]
        public async Task<IActionResult> UpdatePartner(int PartnerID)
        {
            TokenModel JWToken = _userLogin.GetToken();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);

            HttpResponseMessage response = await client.GetAsync("Partner/GetPartnerWithGuides/"+PartnerID);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                ma25partner partner = JsonConvert.DeserializeObject<ma25partner>(result);
                ViewBag.Partner = partner;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _userLogin.Logout();
                return RedirectToAction("Index", "Home", new { Area = "" });
            }
            else
            {
                TempData["MSG_E"] = "Não foi possível encontrar o parceiro!";
                return RedirectToAction("GetPartners", "AdminPartner", new { Area = "Administrator" });
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePartner(PartnerModel partner)
        {
            TokenModel JWToken = _userLogin.GetToken();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);

            string json = JsonConvert.SerializeObject(partner);
            HttpResponseMessage response = await client.PostAsync("Partner/UpdatePartner", new StringContent(json, Encoding.UTF8, "application/json"));
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _userLogin.Logout();
                return RedirectToAction("Index", "Home", new { Area = "" });
            }
            else if (!response.IsSuccessStatusCode)
            {
                TempData["MSG_E"] = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }
            return RedirectToAction("UpdatePartner", "AdminPartner", new { Area = "Administrator", 
                PartnerID = partner.ma25partner.ma25idpartner });
        }

        [HttpPost]
        public async Task<IActionResult> AddGuide(PartnerGuideModel guide)
        {
            TokenModel JWToken = _userLogin.GetToken();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);

            string json = JsonConvert.SerializeObject(guide);
            HttpResponseMessage response = await client.PostAsync("Partner/CreatePartnerGuide", new StringContent(json, Encoding.UTF8, "application/json"));

            if(response.IsSuccessStatusCode)
            {
                TempData["MSG_S"] = "Guia adicionado com sucesso!";
            }
            else if(response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _userLogin.Logout();
                return RedirectToAction("Index", "Home", new { Area = "" });
            }
            else
            {
                TempData["MSG_E"] = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }
            return RedirectToAction("UpdatePartner", "AdminPartner", new { Area = "Administrator", guide.PartnerID });
        }

        [HttpGet]
        public async Task<IActionResult> RemovePartnerGuide(int PartnerGuideID, int PartnerID)
        {
            TokenModel JWToken = _userLogin.GetToken();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);

            HttpResponseMessage response = await client.GetAsync("Partner/RemovePartnerGuide/" + PartnerGuideID);
            if (response.IsSuccessStatusCode)
            {
                TempData["MSG_S"] = "Guia removido do parceiro com sucesso!";
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

            return RedirectToAction("UpdatePartner", "AdminPartner", new { Area = "Administrator", PartnerID });
        }
    }
}
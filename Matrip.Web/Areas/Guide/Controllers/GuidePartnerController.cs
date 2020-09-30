using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Matrip.Domain.Models.AccountModels;
using Matrip.Domain.Models.PartnerModels;
using Matrip.Web.Libraries.Filter;
using Matrip.Web.Libraries.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Matrip.Web.Areas.Guide.Controllers
{
    [Area("Guide")]
    [GuideAuthentication]
    public class GuidePartnerController : Controller
    {
        private UserLogin _userLogin;
        private IConfiguration _configuration;
        private HttpClient client;

        public GuidePartnerController(UserLogin UserLogin, IConfiguration configuration)
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
        /*
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
                ViewData["MSG_S"] = "Parceiro Cadastrado com Sucesso!";
            }
            else
            {
                ViewData["MSG_E"] = "Ocorreu um Erro!";
            }
            return View();
        }
        */
    }
}
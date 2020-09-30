using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Matrip.Domain.Models.AccountModels;
using Matrip.Domain.Models.CityModel;
using Matrip.Domain.Models.Entities;
using Matrip.Web.Libraries.Archive;
using Matrip.Web.Libraries.Filter;
using Matrip.Web.Libraries.Login;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Matrip.Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [AdministratorAuthentication]
    public class AdminCityController : Controller
    {
        private IHostingEnvironment _env;
        private IConfiguration _configuration;
        private UserLogin _userLogin;

        private HttpClient client;
        public AdminCityController(IHostingEnvironment env, IConfiguration configuration, UserLogin UserLogin)
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
        public IActionResult SearchCity()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Index(string UF, string CityName)
        {
            TokenModel JWToken = _userLogin.GetToken();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);

            HttpResponseMessage response = await client.GetAsync("City/GetCityByName?UF=" + UF+"&CityName="+CityName);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                ma09city city = JsonConvert.DeserializeObject<ma09city>(result);
                ViewBag.City = city;
                return View();
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _userLogin.Logout();
                return RedirectToAction("Index", "Home", new { Area = "" });
            }
            TempData["MSG_E"] = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return RedirectToAction("SearchCity","AdminCity");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCity(CityModel City)
        {
            TokenModel JWToken = _userLogin.GetToken();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);

            string json = JsonConvert.SerializeObject(City);
            HttpResponseMessage response = await client.PostAsync("City/UpdateCity", new StringContent(json, Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                TempData["MSG_S"] = "Cidade atualizada com sucesso!";
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _userLogin.Logout();
                return RedirectToAction("Index", "Home", new { Area = "" });
            }
            else
            {
                TempData["MSG_E"] = "Erro ao atualizar a descrição de cidade!";
            }
            return RedirectToAction("Index", "AdminCity", new { City.UF, City.CityName });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCityPhoto([FromForm] int cityID, int? photoID, [FromForm] string UF, [FromForm] string CityName,
            [FromForm] IFormFile file)
        {
            TokenModel JWToken = _userLogin.GetToken();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);
            HttpResponseMessage response = null;
            if (photoID == null)
            {
                response = await client.GetAsync("Photo/AddCityPhoto/"+ cityID);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    ma35cityphoto cityphoto = JsonConvert.DeserializeObject<ma35cityphoto>(result);
                    photoID = cityphoto.ma35idcityphoto;
                }
            }
            else
            {
                response = await client.GetAsync("Photo/UpdateCityPhoto/" + cityID);
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _userLogin.Logout();
                return RedirectToAction("Index", "Home", new { Area = "" });
            }
            else if(!response.IsSuccessStatusCode)
            {
                TempData["MSG_E"] = "Erro ao adicionar a foto do header!";
                return RedirectToAction("Index", "AdminCity", new { UF, CityName });
            }

            await PhotoManager.UploadHeaderCityPhoto(file, photoID.Value, cityID);
            TempData["MSG_S"] = "Foto do header adicionada com sucesso!";
            return RedirectToAction("Index", "AdminCity", new { UF, CityName });
        }

    }
}
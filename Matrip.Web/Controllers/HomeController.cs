using Matrip.Domain.Libraries.Email;
using Matrip.Domain.Models;
using Matrip.Domain.Models.AccountModels;
using Matrip.Domain.Models.Entities;
using Matrip.Domain.Models.HomeModels;
using Matrip.Web.Domain.Models;
using Matrip.Web.Libraries.Filter;
using Matrip.Web.Libraries.Login;
using Matrip.Web.Libraries.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace matrip.Controllers
{
    public class HomeController : Controller
    {
        private IConfiguration _configuration;
        private readonly EmailManagement _emailManagement;
        private readonly UserLogin _userLogin;

        private readonly HttpClient client;
        public HomeController(IConfiguration configuration, EmailManagement EmailManagement, UserLogin UserLogin)
        {
            _configuration = configuration;
            _emailManagement = EmailManagement;
            _userLogin = UserLogin;

            if (client == null)
            {
                client = new HttpClient()
                {
                    BaseAddress = new Uri(_configuration.GetValue<string>("Matrip.API:ApiLink"))
                };
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            }
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await client.GetAsync("Trip/GetFeaturedTrips");//"City/GetCities");

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                HomeIndexModel HomeIndexModel = JsonConvert.DeserializeObject<HomeIndexModel>(result);

                return View(HomeIndexModel);
            }
            else
            {
                return new NotFoundResult();
            }
        }

        [HttpGet]
        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateHttpReferer]
        public IActionResult ContactAction([FromForm] Contact contact)
        {
            try
            {
                var messageList = new List<ValidationResult>();
                var context = new ValidationContext(contact);
                bool isValid = Validator.TryValidateObject(contact, context, messageList, true);

                if (isValid)
                {
                    _emailManagement.SendContactEmail(contact);
                    ViewData["MSG_S"] = "Mensagem enviada com sucesso";
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var texto in messageList)
                    {
                        sb.Append(texto.ErrorMessage + "<br />");
                    }
                    ViewData["MSG_E"] = sb.ToString();
                }
            }
            catch (Exception)
            {
                ViewData["MSG_E"] = "Ops! Ocorreu um erro...";
            }
            return View("Contact");
        }

        [HttpGet]
        [UserAuthentication]
        public async Task<IActionResult> UserConfiguration()
        {
            TokenModel JWToken = _userLogin.GetToken();

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);
            HttpResponseMessage response = await client.GetAsync("User/GetUser");
            if (!response.IsSuccessStatusCode)
            {
                _userLogin.Logout();
                return RedirectToAction("Login", "Account");
            }
            string result = await response.Content.ReadAsStringAsync();
            ma01user ma01user = JsonConvert.DeserializeObject<ma01user>(result);
            UserConfigurationModel UserConfigurationModel = new UserConfigurationModel
            {
                ma01FullName = ma01user.ma01FullName,
                ma01birth = ma01user.ma01birth,
                ma01Email = ma01user.Email,
                ma01PhoneNumber = ma01user.PhoneNumber
            };
            if (TempData.ContainsKey("MSG_E"))
            {
                ViewData["MSG_E"] = TempData["MSG_E"];
            }
            response = await client.GetAsync("User/GetUserAddress");
            result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                ma33UserAddress userAddress = JsonConvert.DeserializeObject<ma33UserAddress>(result);
                if (userAddress == null)
                {
                    ViewBag.Zipcode = "";
                    ViewBag.Country = "";
                    ViewBag.State = "";
                    ViewBag.City = "";
                    ViewBag.Street = "";
                    ViewBag.Neighborhood = "";
                    ViewBag.StreetNumber = "";
                    ViewBag.Complement = "";
                    ViewBag.CPF = "";
                    ViewBag.DocumentNumber = "";
                    ViewBag.DocumentIssuingBody = "";
                    ViewBag.DocumentUF = "";
                }
                else
                {
                    ViewBag.Zipcode = userAddress.ma33Zipcode;
                    ViewBag.Country = userAddress.ma33Country;
                    ViewBag.State = userAddress.ma33State;
                    ViewBag.City = userAddress.ma33City;
                    ViewBag.Street = userAddress.ma33Street;
                    ViewBag.Neighborhood = userAddress.ma33Neighborhood;
                    ViewBag.StreetNumber = userAddress.ma33StreetNumber;
                    ViewBag.Complement = userAddress.ma33Complement;
                    ViewBag.CPF = userAddress.ma33CPF;
                    ViewBag.DocumentNumber = userAddress.ma33documentNumber;
                    ViewBag.DocumentIssuingBody = userAddress.ma33DocumentIssuingBody;
                    ViewBag.DocumentUF = userAddress.ma33DocumentUF;
                }
            }
            return View(UserConfigurationModel);
        }

        [HttpPost]
        [UserAuthentication]
        [ValidateHttpReferer]
        public async Task<IActionResult> UserConfigurationAction(UserConfigurationModel UserConfigurationModel)
        {
            TokenModel JWToken = _userLogin.GetToken();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);

            string json = JsonConvert.SerializeObject(UserConfigurationModel);
            HttpResponseMessage response = await client.PostAsync("User/ChangeUserConfigurations", new StringContent(json, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                TempData["MSG_E"] = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }
            else
            {
                TokenModel tokenModel =_userLogin.GetToken();
                tokenModel.userName = UserConfigurationModel.ma01FullName;
                _userLogin.Logout();
                _userLogin.AddToken(tokenModel);
                TempData["UserConfigurationMessage"] = "Dados Atualizados com Sucesso!";
            }
            return RedirectToAction("UserConfiguration", "Home");
        }
        
        [HttpPost]
        [UserAuthentication]
        [ValidateHttpReferer]
        public async Task<IActionResult> UserConfigurationPostAddress(ma33UserAddress userAddress)
        {
            if (ModelState.IsValid)
            {
                if (CPFAttribute.IsCpf(userAddress.ma33CPF))
                {
                    string json = JsonConvert.SerializeObject(userAddress);

                    TokenModel JWToken = _userLogin.GetToken();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);
                    HttpResponseMessage response = await client.PostAsync("User/AddOrUpdateAddress", new StringContent(json, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["MensagemDoEndereço"] = "Dados Atualizados com Sucesso!";
                    }
                }
                else
                {
                    TempData["MensagemDoEndereço"] = "CPF Inválido";
                }
            }
            return RedirectToAction("UserConfiguration", "Home");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

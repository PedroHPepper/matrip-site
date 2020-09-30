using Matrip.Domain.Libraries.Operations;
using Matrip.Domain.Libraries.Text;
using Matrip.Domain.Models.AccountModels;
using Matrip.Domain.Models.Entities;
using Matrip.Domain.Models.TripPurchase;
using Matrip.Web.Domain.Models;
using Matrip.Web.Libraries.Filter;
using Matrip.Web.Libraries.Login;
using Matrip.Web.Libraries.ShoppingCartCookie;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Matrip.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private IConfiguration _configuration;
        private readonly UserLogin _userLogin;
        private readonly HttpClient client;
        public ShoppingCartController(IConfiguration configuration, UserLogin userLogin)
        {
            _configuration = configuration;
            _userLogin = userLogin;
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
        [UserAuthentication]
        public async Task<IActionResult> Index()
        {
            TokenModel JWToken = _userLogin.GetToken();

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);
            HttpResponseMessage response = await client.GetAsync("ShoppingCart/GetShoppingCart");
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                List<ma18tripitemshoppingcart> ma18TripitemshoppingcartList = JsonConvert.DeserializeObject<List<ma18tripitemshoppingcart>>(result);
                if (ma18TripitemshoppingcartList != null)
                {
                    TempData["ma18TripitemshoppingcartList"] = JsonConvert.SerializeObject(ma18TripitemshoppingcartList);
                }
                return View(ma18TripitemshoppingcartList);
            }
            return NotFound();
        }

        //remoção de item no carrinho de compras
        [HttpGet]
        [UserAuthentication]
        [ValidateHttpReferer]
        public async Task<IActionResult> Remove(int id)
        {
            TokenModel JWToken = _userLogin.GetToken();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);

            HttpResponseMessage response = await client.DeleteAsync("ShoppingCart/RemoveShoppingCartItem/" + id);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "ShoppingCart");
            }
            return RedirectToAction("Index", "Home");
        }
        //controller que leva à tela de escolha de datas
        [HttpGet]
        [UserAuthentication]
        public IActionResult ChooseSubTripsDateTime()
        {
            TokenModel JWToken = _userLogin.GetToken();
            if (JWToken == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else if (!TempData.ContainsKey("ma18TripitemshoppingcartList"))
            {
                return RedirectToAction("Index", "ShoppingCart");
            }
            List<ma18tripitemshoppingcart> ma18TripitemshoppingcartList = JsonConvert.DeserializeObject
                <List<ma18tripitemshoppingcart>>(TempData["ma18TripitemshoppingcartList"].ToString());

            if (ma18TripitemshoppingcartList == null)
            {
                return RedirectToAction("Index", "ShoppingCart");
            }
            ViewBag.TripItemShoppingCartList = ma18TripitemshoppingcartList;

            TempData["ma18TripitemshoppingcartList"] = JsonConvert.SerializeObject(ma18TripitemshoppingcartList);

            return View("ChooseDateTime");
        }

        //controller que submete as datas e direciona à pagamento
        [HttpPost]
        [UserAuthentication]
        [ValidateHttpReferer]
        public IActionResult SubmitDate(List<ChoosedSubtripSaleDate> ChoosedSubtripSaleDateList)
        {
            TempData["ChoosedSubtripSaleDateList"] = JsonConvert.SerializeObject(ChoosedSubtripSaleDateList);

            return RedirectToAction("PaymentForm", "ShoppingCart");
            //return RedirectToAction("Index", "Purchase");
        }

        [HttpGet]
        [UserAuthentication]
        public async Task<IActionResult> PaymentForm(string discountCode = "")
        {
            if (TempData.ContainsKey("ChoosedSubtripSaleDateList"))
            {
                TokenModel JWToken = _userLogin.GetToken();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);
                if (discountCode != "")
                {
                    HttpResponseMessage responseDiscount = await client.GetAsync("Sale/GetDiscount/" + discountCode);
                    if (responseDiscount.IsSuccessStatusCode)
                    {
                        string resultDiscount = await responseDiscount.Content.ReadAsStringAsync();
                        ma38InfluencerDiscount discount = JsonConvert.DeserializeObject<ma38InfluencerDiscount>(resultDiscount);
                        if (discount == null)
                        {
                            ViewData["E_MSG"] = "Código inválido!";
                        }
                        ViewBag.InfluencerDiscount = discount;
                    }
                    else
                    {
                        ViewData["E_MSG"] = "Ocorreu um erro. Tente novamente.";
                    }
                }

                HttpResponseMessage response = await client.GetAsync("ShoppingCart/GetShoppingCart");
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    List<ma18tripitemshoppingcart> ma18TripitemshoppingcartList = JsonConvert.DeserializeObject<List<ma18tripitemshoppingcart>>(result);
                    double totalValue = CalculateValues.GetCalculatedTotalValue(ma18TripitemshoppingcartList);

                    ViewBag.tripItemShoppingCart = ma18TripitemshoppingcartList;
                    ViewBag.totalValue = totalValue;
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            return View("PaymentFormView");
        }


        [HttpGet]
        public async Task<IActionResult> GetUnavailableDates(int scheduleID, int subtripItemShoppingCartID, string Date)
        {
            TokenModel JWToken = _userLogin.GetToken();

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);
            HttpResponseMessage response = await client.GetAsync("Sale/GetUnavailableDates?scheduleID=" + scheduleID + "&subtripItemShoppingCartID=" + subtripItemShoppingCartID + "&Date=" + Date);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                bool dateIsAvailable = JsonConvert.DeserializeObject<bool>(result);
                return Ok(dateIsAvailable);
            }
            else
            {
                return BadRequest("Falha ao obter datas!");
            }
        }











        
        
    }
}

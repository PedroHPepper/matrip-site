using Matrip.Domain.Models.AccountModels;
using Matrip.Domain.Models.Entities;
using Matrip.Domain.Models.TripPurchase;
using Matrip.Web.Domain.Models;
using Matrip.Web.Libraries.ChoosedTripCookie;
using Matrip.Web.Libraries.Filter;
using Matrip.Web.Libraries.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using X.PagedList;

namespace Matrip.Web.Controllers
{
    public class TripController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient client;
        private readonly UserLogin _userLogin;
        private readonly ChoosedTripCookie _choosedTripCookie;
        public TripController(IConfiguration configuration, UserLogin userLogin, ChoosedTripCookie choosedTripCookie)
        {
            _configuration = configuration;
            _userLogin = userLogin;
            _choosedTripCookie = choosedTripCookie;

            if (client == null)
            {
                client = new HttpClient()
                {
                    BaseAddress = new Uri(_configuration.GetValue<string>("Matrip.API:ApiLink"))
                };
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            }
        }

        [HttpGet("TripbyCategory/{CategoryName}/{UF}/{CityName}/{Page}")]
        public async Task<IActionResult> GetTripListByCategory(string CategoryName, string UF, string CityName, int? Page)
        {
            HttpResponseMessage response = await client.GetAsync("Trip/byCategory?CategoryName=" + CategoryName + "&UF=" + UF + "&CityName=" + HttpUtility.UrlEncode(CityName) + "&Page=" + Page);



            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                //PaginationEntity<ma05trip> trips = JsonConvert.DeserializeObject<PaginationEntity<ma05trip>>(result);
                TripListView tripListView = JsonConvert.DeserializeObject<TripListView>(result);
                if (tripListView.ma06tripcategory == null || tripListView.ma08uf == null || tripListView.ma09city == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                StaticPagedList<ma05trip> lst = new StaticPagedList<ma05trip>(
                    tripListView.lst.Items,
                    tripListView.lst.MetaData.PageNumber,
                    tripListView.lst.MetaData.PageSize,
                    tripListView.lst.MetaData.TotalItemCount);

                ViewBag.CategoryName = tripListView.ma06tripcategory.ma06name;
                ViewBag.UF = tripListView.ma08uf.ma08UFInitials;
                ViewBag.City = tripListView.ma09city;


                return View("ViewTripList", lst);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("Trip/{UF}/{CityName}/{Page}")]
        public async Task<IActionResult> GetTripList(string UF, string CityName, int? Page)
        {
            HttpResponseMessage response = await client.GetAsync("Trip/byCity?UF=" + UF + "&CityName=" + HttpUtility.UrlEncode(CityName) + "&Page=" + Page);


            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                TripListView tripListView = JsonConvert.DeserializeObject<TripListView>(result);
                if (tripListView.ma08uf == null || tripListView.ma09city == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                StaticPagedList<ma05trip> lst = new StaticPagedList<ma05trip>(
                    tripListView.lst.Items,
                    tripListView.lst.MetaData.PageNumber,
                    tripListView.lst.MetaData.PageSize,
                    tripListView.lst.MetaData.TotalItemCount);

                ViewBag.UF = tripListView.ma08uf.ma08UFInitials;
                ViewBag.City = tripListView.ma09city;

                return View("ViewTripList", lst);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("Trip/ViewTrip/{TripID}")]
        public async Task<IActionResult> ViewTrip(int TripID)
        {
            HttpResponseMessage response = await client.GetAsync("Trip/GetTrip/" + TripID);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                ma05trip Trip = JsonConvert.DeserializeObject<ma05trip>(result);
                ViewBag.Trip = Trip;

                return View("ViewTrip");
            }

            return NotFound();
        }
        [HttpPost]
        [UserAuthentication]
        [ValidateHttpReferer]
        public IActionResult ViewTrip(TripItem tripItem)
        {
            if(_userLogin.GetToken() == null)
            {
                return RedirectToAction("Login", "Account");
            }
            string SubtripString = HttpContext.Request.Form["subtrip"];
            string[] statusSelectionString = HttpContext.Request.Form["statusSelection"];

            tripItem.SubTripID = int.Parse(SubtripString.Remove(0, 8));
            for (int j = 0; j < statusSelectionString.Count(); j++)
            {
                if (int.Parse(statusSelectionString[j].Split("/")[1]) == tripItem.SubTripID)
                {
                    tripItem.SubtripStatus = statusSelectionString[j];
                    break;
                }
            }
            if (tripItem.Services != null)
            {
                List<ServiceQuantity> ServicesToRemove = new List<ServiceQuantity>();
                for (int i = 0; i < tripItem.Services.Count(); i++)
                {
                    if (tripItem.Services[i].Quantity == 0)
                    {
                        ServicesToRemove.Add(tripItem.Services[i]);
                    }
                }
                if(ServicesToRemove.Count() != 0)
                {
                    for(int i=0; i< ServicesToRemove.Count(); i++)
                    {
                        tripItem.Services.Remove(ServicesToRemove[i]);
                    } 
                }
            }


            TempData["tripItem"] = JsonConvert.SerializeObject(tripItem);
            return RedirectToAction("TouristsForm", "Trip");
        }

        [HttpGet]
        [UserAuthentication]
        public async Task<IActionResult> TouristsForm()
        {
            if (TempData.ContainsKey("tripItem"))
            {
                TripItem tripItem = JsonConvert.DeserializeObject<TripItem>(TempData["tripItem"].ToString());
                TempData["tripItem"] = JsonConvert.SerializeObject(tripItem);

                HttpResponseMessage response = await client.GetAsync("Trip/GetTrip/" + tripItem.TripID);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    ma05trip Trip = JsonConvert.DeserializeObject<ma05trip>(result);

                    ViewBag.Trip = Trip;
                    return View(tripItem);
                }
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [UserAuthentication]
        [ValidateHttpReferer]
        public async Task<IActionResult> SubmitTourists()
        {
            if (TempData.ContainsKey("tripItem"))
            {
                TripItem tripItem = JsonConvert.DeserializeObject<TripItem>(TempData["tripItem"].ToString());

                string[] AgeDiscountIDStringList = HttpContext.Request.Form["AgeDiscountID"];
                string[] TouristNameStringList = HttpContext.Request.Form["TouristName"];
                string[] RGOrPassportStringList = HttpContext.Request.Form["RGOrPassport"];
                string[] AgeList = HttpContext.Request.Form["Age"];

                int ContinueOrCart = int.Parse(HttpContext.Request.Form["ContinueOrCart"]);

                List<TouristModel> touristList = new List<TouristModel>();
                for (int i = 0; i < AgeDiscountIDStringList.Count(); i++)
                {
                    TouristModel tourist = new TouristModel()
                    {
                        AgeDiscountID = int.Parse(AgeDiscountIDStringList[i]),
                        ma28Name = TouristNameStringList[i],
                        ma28PassportOrRG = RGOrPassportStringList[i],
                        Age = int.Parse(AgeList[i])
                    };
                    touristList.Add(tourist);
                }
                if (ContinueOrCart == 1)
                {
                    ChoosedTripPackage choosedTripPackage = new ChoosedTripPackage()
                    {
                        TouristList = touristList,
                        TripItem = tripItem
                    };

                    TokenModel JWToken = _userLogin.GetToken();
                    string json = JsonConvert.SerializeObject(choosedTripPackage);
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);
                    HttpResponseMessage responsePost = await client.PostAsync("ShoppingCart/AddShoppingCartItem", new StringContent(json, Encoding.UTF8, "application/json"));
                    if (responsePost.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "ShoppingCart");
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else if (ContinueOrCart == 2)
                {
                    HttpResponseMessage response = await client.GetAsync("Trip/GetTrip/" + tripItem.TripID);
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        ma05trip Trip = JsonConvert.DeserializeObject<ma05trip>(result);

                        ChoosedTripPackage choosedTripPackage = new ChoosedTripPackage()
                        {
                            TouristList = touristList,
                            trip = removeUnchoosedItems(Trip, tripItem)
                            
                        };
                        choosedTripPackage.TripItem = removeUnchoosedServices(Trip, tripItem);
                        TempData["ChoosedTripPackage"] = JsonConvert.SerializeObject(choosedTripPackage);
                        return RedirectToAction("ChooseSubTripsDateTime", "ItemPurchase");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        //Para fazer a escolha de item, é preciso remover os subpasseios que não foram escolhidos
        private ma05trip removeUnchoosedItems(ma05trip trip, TripItem tripItem)
        {
            foreach (ma14subtrip subtrip in trip.ma14subtrip.ToList())
            {
                //remove os subpasseios que não estão na escolha de item, restando apenas 1
                if (tripItem.SubTripID != subtrip.ma14idsubtrip)
                {
                    trip.ma14subtrip.Remove(subtrip);
                }
                else
                {
                    if(subtrip.ma11service != null)
                    {
                        foreach (ma11service service in subtrip.ma11service.ToList())
                        {
                            if ((!tripItem.Services.Where(e => e.ServiceID == service.ma11idservice).Any()) || service.ma11status == "0")
                            {
                                subtrip.ma11service.Remove(service);
                            }
                        }
                    }
                }
            }
            return trip;
        }

        //Aqui são removidos os serviços dos outros subpasseios não escolhidos na venda do item, restando apenas os referentes ao subpasseio
        //escolhido
        private TripItem removeUnchoosedServices(ma05trip trip, TripItem tripItem)
        {
            if (tripItem.Services != null)
            {
                foreach (ServiceQuantity serviceQuantity in tripItem.Services.ToList())
                {
                    foreach (ma14subtrip subtrip in trip.ma14subtrip.ToList())
                    {
                        if (subtrip.ma11service.ToList().Where(e => e.ma11idservice == serviceQuantity.ServiceID).Any())
                        {
                            break;
                        }
                        else
                        {
                            tripItem.Services.Remove(serviceQuantity);
                        }
                    }
                }
            }
            
            return tripItem;
        }
    }
}
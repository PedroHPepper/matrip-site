using Matrip.Domain.Libraries.Email;
using Matrip.Domain.Libraries.Operations;
using Matrip.Domain.Libraries.Text;
using Matrip.Domain.Libraries.Validation;
using Matrip.Domain.Models.AccountModels;
using Matrip.Domain.Models.Entities;
using Matrip.Domain.Models.Payment;
using Matrip.Domain.Models.SaleModels;
using Matrip.Domain.Models.TripPurchase;
using Matrip.Web.Domain.Models;
using Matrip.Web.Libraries.Filter;
using Matrip.Web.Libraries.Login;
using Matrip.Web.Libraries.Management.Payment.PagarMe;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PagarMe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Matrip.Web.Controllers
{
    public class ItemPurchaseController : Controller
    {
        private readonly ManagePagarMe _payment;
        private readonly IConfiguration _configuration;
        private readonly HttpClient client;
        private readonly UserLogin _userLogin;
        private EmailManagement _emailManagement;
        public ItemPurchaseController(IConfiguration configuration, ManagePagarMe payment, UserLogin userLogin, EmailManagement emailManagement)
        {
            _configuration = configuration;
            _payment = payment;
            _userLogin = userLogin;
            _emailManagement = emailManagement;
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
        public IActionResult ChooseSubTripsDateTime()
        {
            if (!TempData.ContainsKey("ChoosedTripPackage"))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ChoosedTripPackage choosedTripPackage = JsonConvert.DeserializeObject<ChoosedTripPackage>(TempData["ChoosedTripPackage"].ToString());
                TempData["ChoosedTripPackage"] = JsonConvert.SerializeObject(choosedTripPackage);

                if (choosedTripPackage == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.TripItem = choosedTripPackage.TripItem;
                ViewBag.Trip = choosedTripPackage.trip;
                ViewBag.PeopleQuantity = choosedTripPackage.TouristList.Count();

                return View("ChooseDateTime");
            }
        }
        [HttpPost]
        [UserAuthentication]
        public IActionResult SubmitDate(List<ChoosedSubtripSaleDate> ChoosedSubtripSaleDateList)
        {
            TempData["ChoosedSubtripSaleDateList"] = JsonConvert.SerializeObject(ChoosedSubtripSaleDateList);

            return RedirectToAction("PaymentForm", "ItemPurchase");
        }
        [HttpGet]
        [UserAuthentication]
        public async Task<IActionResult> PaymentForm(string discountCode = "")
        {
            if (TempData.ContainsKey("ChoosedSubtripSaleDateList") && TempData.ContainsKey("ChoosedTripPackage"))
            {
                ChoosedTripPackage choosedTripPackage = JsonConvert.DeserializeObject<ChoosedTripPackage>(TempData["ChoosedTripPackage"].ToString());
                TempData["ChoosedTripPackage"] = JsonConvert.SerializeObject(choosedTripPackage);


                if (!string.IsNullOrEmpty(discountCode))
                {
                    TokenModel JWToken = _userLogin.GetToken();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);
                    HttpResponseMessage responseDiscount = await client.GetAsync("Sale/GetDiscount/" + discountCode);
                    if (responseDiscount.IsSuccessStatusCode)
                    {
                        string resultDiscount = await responseDiscount.Content.ReadAsStringAsync();
                        ma38InfluencerDiscount discount = JsonConvert.DeserializeObject<ma38InfluencerDiscount>(resultDiscount);
                        double totalValue = CalculateValues.GetCalculatedTotalValue(choosedTripPackage, null);
                        if (discount == null)
                        {
                            ViewData["E_MSG"] = "Código inválido!";
                        }
                        else
                        {
                            totalValue = CalculateValues.GetCalculatedTotalValue(choosedTripPackage, discount.ma38DiscountPercent);
                            ViewBag.InfluencerDiscount = discount;
                            ViewBag.DiscountMessage = "(" + discount.ma38DiscountPercent.ToString() + "% de Desconto no Passeio)";
                        }

                        ViewBag.Trip = choosedTripPackage.trip;
                        ViewBag.totalValue = totalValue;
                    }
                    else
                    {
                        ViewData["E_MSG"] = "Ocorreu um erro. Tente novamente.";
                        double totalValue = CalculateValues.GetCalculatedTotalValue(choosedTripPackage, null);
                        ViewBag.Trip = choosedTripPackage.trip;
                        ViewBag.totalValue = totalValue;
                    }
                }
                else
                {
                    double totalValue = CalculateValues.GetCalculatedTotalValue(choosedTripPackage, null);
                    ViewBag.Trip = choosedTripPackage.trip;
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
        [UserAuthentication]
        public async Task<IActionResult> Index(double creditCardPayment, double transferencePayment, string discountCode = "")
        {
            if (TempData.ContainsKey("ChoosedSubtripSaleDateList") && TempData.ContainsKey("ChoosedTripPackage"))
            {
                TokenModel JWToken = _userLogin.GetToken();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);
                HttpResponseMessage response = await client.GetAsync("User/GetUserAddress");
                string result = await response.Content.ReadAsStringAsync();
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
                ChoosedTripPackage choosedTripPackage = JsonConvert.DeserializeObject<ChoosedTripPackage>
                                                                (TempData["ChoosedTripPackage"].ToString());
                TempData["ChoosedTripPackage"] = JsonConvert.SerializeObject(choosedTripPackage);
                List<ChoosedSubtripSaleDate> choosedSubtripSaleDateList = JsonConvert.DeserializeObject<List<ChoosedSubtripSaleDate>>
                                                                            (TempData["ChoosedSubtripSaleDateList"].ToString());
                TempData["ChoosedSubtripSaleDateList"] = JsonConvert.SerializeObject(choosedSubtripSaleDateList);

                double totalValue = 0;
                if (!string.IsNullOrEmpty(discountCode))
                {
                    HttpResponseMessage responseDiscount = await client.GetAsync("Sale/GetDiscount/" + discountCode);
                    if (responseDiscount.IsSuccessStatusCode)
                    {
                        string resultDiscount = await responseDiscount.Content.ReadAsStringAsync();
                        ma38InfluencerDiscount discount = JsonConvert.DeserializeObject<ma38InfluencerDiscount>(resultDiscount);
                        if (discount == null)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        ViewBag.InfluencerDiscount = discount;
                        ViewBag.DiscountMessage = "(" + discount.ma38DiscountPercent + "% de Desconto no Passeio)";

                        totalValue += CalculateValues.GetCalculatedTotalValue(choosedTripPackage, discount.ma38DiscountPercent);
                        ViewBag.Trip = choosedTripPackage.trip;
                        ViewBag.totalValue = totalValue;
                    }
                }
                else
                {
                    totalValue += CalculateValues.GetCalculatedTotalValue(choosedTripPackage, null);
                    ViewBag.Trip = choosedTripPackage.trip;
                    ViewBag.totalValue = totalValue;
                }
                if (totalValue != (creditCardPayment + transferencePayment))
                {
                    return RedirectToAction("Index", "Home");
                }


                ViewBag.RecentSaleDate = choosedSubtripSaleDateList.OrderBy(e => e.ChoosedDate).FirstOrDefault();
                if (creditCardPayment > 0 && transferencePayment > 0)
                {
                    ViewBag.creditCardPayment = creditCardPayment;
                    ViewBag.transferencePayment = transferencePayment;
                }
                else if (transferencePayment > 0)
                {
                    ViewBag.transferencePayment = transferencePayment;
                }
                else
                {
                    ViewBag.creditCardPayment = creditCardPayment;
                }
                ViewBag.SubmitAction = "ItemPurchase";
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        [UserAuthentication]
        [ValidateHttpReferer]
        public async Task<IActionResult> SubmitPayment([FromForm]PaymentViewModel Payment)
        {
            if (ModelState.IsValid)
            {
                if (CPFAttribute.IsCpf(Payment.ma33UserAddress.ma33CPF))
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

                    try
                    {
                        if (TempData.ContainsKey("ChoosedSubtripSaleDateList") && TempData.ContainsKey("ChoosedTripPackage"))
                        {
                            ChoosedTripPackage choosedTripPackage = JsonConvert.DeserializeObject<ChoosedTripPackage>
                                                                      (TempData["ChoosedTripPackage"].ToString());

                            List<ChoosedSubtripSaleDate> choosedSubtripSaleDateList = JsonConvert.DeserializeObject<List<ChoosedSubtripSaleDate>>
                                                                                        (TempData["ChoosedSubtripSaleDateList"].ToString());

                            double totalValue = CalculateValues.GetCalculatedTotalValue(choosedTripPackage, null);
                            if (!string.IsNullOrEmpty(Payment.influencerDiscountCode))
                            {
                                HttpResponseMessage responseDiscount = await client.GetAsync("Sale/GetDiscount/" + Payment.influencerDiscountCode);
                                if (responseDiscount.IsSuccessStatusCode)
                                {
                                    string resultDiscount = await responseDiscount.Content.ReadAsStringAsync();
                                    ma38InfluencerDiscount discount = JsonConvert.DeserializeObject<ma38InfluencerDiscount>(resultDiscount);
                                    if (discount == null)
                                    {
                                        return RedirectToAction("Index", "Home");
                                    }
                                    totalValue = CalculateValues.GetCalculatedTotalValue(choosedTripPackage, discount.ma38DiscountPercent);
                                }
                            }
                            response = await submitPayment(ma01user, choosedSubtripSaleDateList, choosedTripPackage, Payment, totalValue);
                            if (response.IsSuccessStatusCode)
                            {
                                response = await client.GetAsync("Sale/GetLastSale");
                                if (response.IsSuccessStatusCode)
                                {
                                    result = await response.Content.ReadAsStringAsync();
                                    SaleViewModel saleView = JsonConvert.DeserializeObject<SaleViewModel>(result);
                                    try
                                    {
                                        _emailManagement.SendPartnerEmail(saleView, _userLogin.GetToken().userName);
                                    }
                                    catch(Exception e)
                                    {
                                        return RedirectToAction("GetSaleList", "Purchase", new { page = 1 });
                                    }
                                }
                                return RedirectToAction("GetSaleList", "Purchase", new { page = 1 });
                            }
                            else
                            {
                                TempData["MSG_E"] = "Ocorreu um erro inesperado...: " + response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                            }
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    catch (Exception e)
                    {
                        TempData["MSG_E"] = e.Message;
                    }
                }
                else
                {
                    TempData["MSG_E"] = "O valor de CPF é inválido.";
                }
            }
            double creditCardPayment = 0;
            if (Payment.CreditCard != null)
            {
                creditCardPayment = Payment.CreditCard.Value;
            }
            if (!string.IsNullOrEmpty(Payment.influencerDiscountCode))
            {
                return RedirectToAction("Index", new { creditCardPayment = creditCardPayment, transferencePayment = Payment.Transference.TransferenceValue, discountCode = Payment.influencerDiscountCode });
            }
            else
            {
                return RedirectToAction("Index", new { creditCardPayment = creditCardPayment, transferencePayment = Payment.Transference.TransferenceValue });
            }
        }


        public async Task<HttpResponseMessage> submitPayment(ma01user ma01user, List<ChoosedSubtripSaleDate> choosedSubtripSaleDateList, ChoosedTripPackage choosedTripPackage, PaymentViewModel Payment, double totalValue)
        {
            List<ma24payment> paymentList = new List<ma24payment>();
            foreach (int paymentMethod in Payment.paymentMethods)
            {
                if (paymentMethod == 1)
                {
                    List<Installments> lista = _payment.CalculateInstallmentsPayment(Convert.ToDecimal(Payment.CreditCard.Value));
                    Installments installments = lista.Where(e => e.InstallmentsNumber == Payment.CreditCard.InstallmentsNumber).FirstOrDefault();

                    Transaction transaction = _payment.GenerateCreditCardPaymentWithTripItem(ma01user, choosedTripPackage, Payment.ma33UserAddress.ma33CPF,
                                              Payment.CreditCard, Payment.ma33UserAddress, installments, totalValue);
                    ma24payment ma24payment = new ma24payment()
                    {
                        ma24CardFlag = transaction.CardBrand.ToString(),
                        ma24Installments = Payment.CreditCard.InstallmentsNumber,
                        ma24CreditCard = true,
                        ma24paymentStatus = 1,
                        ma24TransactionID = transaction.Id,
                        ma24Transference = false,
                        ma24paymentValue = Convert.ToDouble(installments.Value)
                    };
                    paymentList.Add(ma24payment);
                }
                else
                {
                    ma24payment ma24payment = new ma24payment()
                    {
                        ma24Installments = Payment.Transference.InstallmentsNumber,
                        ma24CreditCard = false,
                        ma24paymentStatus = 3,
                        ma24Transference = true,
                        ma24paymentValue = Payment.Transference.TransferenceValue
                    };
                    paymentList.Add(ma24payment);
                }
            }
            PaymentTripItemModel paymentModel = new PaymentTripItemModel()
            {
                choosedSubtripSaleDateList = choosedSubtripSaleDateList,
                Payment = paymentList,
                TouristList = choosedTripPackage.TouristList,
                trip = choosedTripPackage.trip,
                TripItem = choosedTripPackage.TripItem,
                userAddress = Payment.ma33UserAddress,
                influencerDiscountCode = Payment.influencerDiscountCode
            };
            string json = JsonConvert.SerializeObject(paymentModel);
            HttpResponseMessage response = await client.PostAsync("Sale/SubmitTripItemSale", new StringContent(json, Encoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode)
            {
                if(Payment.paymentMethods.Count() > 1)
                {
                    ma24payment payment = paymentList.Where(e => e.ma24TransactionID != "").FirstOrDefault();
                    if (payment != null)
                    {
                        _payment.Chargeback(payment.ma24TransactionID);
                    }
                } 
            }
            //string result = await response.Content.ReadAsStringAsync();
            //int SaleID = JsonConvert.DeserializeObject<int>(result);

            //_emailManagement.SendContractEmail(Url.Action("ConsultContract", "Purchase", new { id = SaleID }), ma01user.Email);
            return response;
        }






        [HttpGet]
        private async Task<IActionResult> GetUnavailableDates(int scheduleID, string Date, int peopleQuantity, int ValueID)
        {
            TokenModel JWToken = _userLogin.GetToken();

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);
            HttpResponseMessage response = await client.GetAsync("Sale/GetUnavailableDatesWithSubtripID?scheduleID=" + scheduleID +
                                                                    "&Date=" + Date + "&peopleQuantity=" + peopleQuantity + "&ValueID=" + ValueID);
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
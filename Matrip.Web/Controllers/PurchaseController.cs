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
using Matrip.Web.Libraries.Pagination;
using Matrip.Web.Libraries.ShoppingCartCookie;
using Matrip.Web.Libraries.Text;
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
using X.PagedList;

namespace Matrip.Web.Controllers
{
    public class PurchaseController : Controller
    {
        private IConfiguration _configuration;
        private UserLogin _userLogin;
        private HttpClient client;
        private ManagePagarMe _payment;
        private ShoppingCartCookie _shoppingCartCookie;
        private EmailManagement _emailManagement;

        public PurchaseController(IConfiguration configuration, UserLogin userLogin, ManagePagarMe payment, ShoppingCartCookie shoppingCartCookie, EmailManagement emailManagement)
        {
            _configuration = configuration;
            _userLogin = userLogin;
            _payment = payment;
            _shoppingCartCookie = shoppingCartCookie;
            _emailManagement = emailManagement;
            if (client == null)
            {
                client = new HttpClient();
                client.BaseAddress = new Uri(_configuration.GetValue<string>("Matrip.API:ApiLink"));
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            }
        }
        [HttpGet]
        [UserAuthentication]
        public async Task<IActionResult> Index(double creditCardPayment, double transferencePayment, string discountCode = "")
        {
            if (TempData.ContainsKey("ChoosedSubtripSaleDateList"))
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
                response = await client.GetAsync("ShoppingCart/GetShoppingCart");
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                    List<ma18tripitemshoppingcart> ma18TripitemshoppingcartList = JsonConvert.DeserializeObject<List<ma18tripitemshoppingcart>>(result);
                    double totalValue = CalculateValues.GetCalculatedTotalValue(ma18TripitemshoppingcartList, null);
                    if (!string.IsNullOrEmpty(discountCode))
                    {
                        HttpResponseMessage responseDiscount = await client.GetAsync("Sale/GetDiscount/" + discountCode);
                        if (responseDiscount.IsSuccessStatusCode)
                        {
                            string resultDiscount = await responseDiscount.Content.ReadAsStringAsync();
                            ma38InfluencerDiscount discount = JsonConvert.DeserializeObject<ma38InfluencerDiscount>(resultDiscount);

                            ViewBag.InfluencerDiscount = discount;
                            
                            totalValue = CalculateValues.GetCalculatedTotalValue(ma18TripitemshoppingcartList, discount.ma38DiscountPercent);
                            ViewBag.tripItemShoppingCart = ma18TripitemshoppingcartList;
                            ViewBag.totalValue = totalValue;
                        }
                    }
                    else
                    {
                        ViewBag.tripItemShoppingCart = ma18TripitemshoppingcartList;
                        ViewBag.totalValue = totalValue;
                    }
                    if(totalValue != (creditCardPayment + transferencePayment))
                    {
                        return RedirectToAction("Index", "ShoppingCart");
                    }
                }
                List<ChoosedSubtripSaleDate> choosedSubtripSaleDateList = JsonConvert.DeserializeObject<List<ChoosedSubtripSaleDate>>
                                                                                                (TempData["ChoosedSubtripSaleDateList"].ToString());
                TempData["ChoosedSubtripSaleDateList"] = JsonConvert.SerializeObject(choosedSubtripSaleDateList);

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
                ViewBag.SubmitAction = "Purchase";
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
                //verifica o CPF do usuário, se for inválido dispara mensagem de erro na view
                if (CPFAttribute.IsCpf(Payment.ma33UserAddress.ma33CPF))
                {
                    //faz uma requisição de busca para pegar informação de usuário e carrinho de compras completo
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
                    response = await client.GetAsync("ShoppingCart/GetShoppingCart");
                    try
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            //resgata o carrinho de compras da requisição
                            result = await response.Content.ReadAsStringAsync();
                            List<ma18tripitemshoppingcart> ma18TripitemshoppingcartList = JsonConvert.DeserializeObject<List<ma18tripitemshoppingcart>>(result);

                            //calcuta valor total do carrinho de compras
                            double totalValue = CalculateValues.GetCalculatedTotalValue(ma18TripitemshoppingcartList, null);

                            //verifica se tem código de desconto válido para o para o carrinho. Se tiver, calcula cada passeio que permite desconto
                            //e influencia no valor final. Isso, calculando novamente o valor tota, só que com desconto
                            if (Payment.influencerDiscountCode != "")
                            {
                                HttpResponseMessage responseDiscount = await client.GetAsync("Sale/GetDiscount/" + Payment.influencerDiscountCode);
                                if (responseDiscount.IsSuccessStatusCode)
                                {
                                    string resultDiscount = await responseDiscount.Content.ReadAsStringAsync();
                                    ma38InfluencerDiscount discount = JsonConvert.DeserializeObject<ma38InfluencerDiscount>(resultDiscount);
                                    //aqui calcula novamente o valor total novamente, agora com desconto
                                    totalValue = CalculateValues.GetCalculatedTotalValue(ma18TripitemshoppingcartList, discount.ma38DiscountPercent);
                                }
                            }
                            List<ChoosedSubtripSaleDate> choosedSubtripSaleDateList = JsonConvert.DeserializeObject<List<ChoosedSubtripSaleDate>>
                                                                                                (TempData["ChoosedSubtripSaleDateList"].ToString());
                            //submete o pagamento
                            response = await submitPayment(ma01user, choosedSubtripSaleDateList, ma18TripitemshoppingcartList, Payment);
                            if (response.IsSuccessStatusCode)
                            {
                                //apos completar a compra, resgata a ultima compra na api de venda pra enviar o email para os parceiros
                                //comunicando sobre as vendas
                                response = await client.GetAsync("Sale/GetLastSale");
                                if (response.IsSuccessStatusCode)
                                {
                                    result = await response.Content.ReadAsStringAsync();
                                    SaleViewModel saleView = JsonConvert.DeserializeObject<SaleViewModel>(result);
                                    try
                                    {
                                        _emailManagement.SendPartnerEmail(saleView, _userLogin.GetToken().userName);
                                    }
                                    catch (Exception e)
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
                            TempData["MSG_E"] = "Ocorreu um erro inesperado!";
                        }
                    }
                    catch (Exception e)
                    {
                        TempData["MSG_E"] = e.Message;
                        return RedirectToAction("Index", new { creditCardPayment = Payment.CreditCard.Value, transferencePayment = Payment.Transference.TransferenceValue });
                    }
                }
                else
                {
                    TempData["MSG_E"] = "O valor de CPF é inválido.";
                }
            }
            //caso der erro, retorna à pagina da compra com os devidos valores
            double creditCardPayment = 0;
            if (Payment.CreditCard != null)
            {
                creditCardPayment = Payment.CreditCard.Value;
            }
            if (Payment.influencerDiscountCode != "")
            {
                return RedirectToAction("Index", new { creditCardPayment = creditCardPayment, transferencePayment = Payment.Transference.TransferenceValue, discountCode = Payment.influencerDiscountCode });
            }
            else
            {
                return RedirectToAction("Index", new { creditCardPayment = creditCardPayment, transferencePayment = Payment.Transference.TransferenceValue });
            }
        }

        

        [HttpGet("Purchase/GetSaleList/{page}")]
        [UserAuthentication]
        public async Task<IActionResult> GetSaleList(int page)
        {
            TokenModel JWToken = _userLogin.GetToken();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);
            HttpResponseMessage response = await client.GetAsync("Sale/GetSaleList?page=" + page);
            if (response.IsSuccessStatusCode)
            {
                //resgata lista de compras com paginação
                string result = await response.Content.ReadAsStringAsync();
                PaginationEntity<ma32sale> saleList = JsonConvert.DeserializeObject<PaginationEntity<ma32sale>>(result);
                StaticPagedList<ma32sale> lst = new StaticPagedList<ma32sale>(
                    saleList.Items,
                    saleList.MetaData.PageNumber,
                    saleList.MetaData.PageSize,
                    saleList.MetaData.TotalItemCount);
                return View("SaleListView", lst);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        [UserAuthentication]
        public async Task<IActionResult> GetSale(int id)
        {
            SaleViewModel SaleView = new SaleViewModel();
            SaleTransactionViewModel saleTransactionViewModel = new SaleTransactionViewModel();

            TokenModel JWToken = _userLogin.GetToken();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);
            HttpResponseMessage response = await client.GetAsync("Sale/GetSale/" + id);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                SaleView = JsonConvert.DeserializeObject<SaleViewModel>(result);
                Transaction transaction = null;
                if (SaleView.sale.ma24payment.Where(e => e.ma24TransactionID != null).Any())
                {
                    transaction = _payment.GetTransaction(SaleView.sale.ma24payment.Where(e => e.ma24TransactionID != null).FirstOrDefault().ma24TransactionID);
                }
                saleTransactionViewModel = new SaleTransactionViewModel()
                {
                    ma32sale = SaleView.sale,
                    transaction = transaction
                };
                ViewBag.UserAddress = SaleView.userAddress;
            }
            else
            {
                ViewData["MSG_E"] = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return RedirectToAction("GetSaleList", "Purchase", new { page = 1 });
            }
            return View("SaleView", saleTransactionViewModel);
        }

        [HttpGet]
        [UserAuthentication]
        public async Task<IActionResult> ConsultContract(int id)
        {
            TokenModel JWToken = _userLogin.GetToken();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);
            HttpResponseMessage response = await client.GetAsync("Sale/GetContract/" + id);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                ContractViewModel contract = JsonConvert.DeserializeObject<ContractViewModel>(result);

                double TotalSaleValue = CalculateValues.CalculateTotalSaleValue(contract.SaleModel.sale);

                ViewBag.TotalSaleValue = TotalSaleValue.ToString("N2");
                ViewBag.TotalSaleValueInFull = NumberInFull.InFull(Convert.ToDecimal(TotalSaleValue));

                List<ma25partner> partners = new List<ma25partner>();
                foreach(ma21saleTrip ma21saleTrip in contract.SaleModel.sale.ma21saleTrip)
                {
                    foreach(ma22subtripsale ma22subtripsale in ma21saleTrip.ma22subtripsale)
                    {
                        if (!partners.Contains(ma22subtripsale.ma14subtrip.ma25partner))
                        {
                            partners.Add(ma22subtripsale.ma14subtrip.ma25partner);
                        }
                    }
                }
                ViewBag.ma25partnerList = partners;
                return View("ContractView", contract);
            }

            return RedirectToAction("Index", "Home");
        }














        public async Task<HttpResponseMessage> submitPayment(ma01user ma01user, List<ChoosedSubtripSaleDate> choosedSubtripSaleDateList, List<ma18tripitemshoppingcart> ma18TripitemshoppingcartList, PaymentViewModel Payment)
        {
            List<ma24payment> paymentList = new List<ma24payment>();
            foreach (int paymentMethod in Payment.paymentMethods)
            {
                if (paymentMethod == 1)
                {
                    List<Installments> lista = _payment.CalculateInstallmentsPayment(Convert.ToDecimal(Payment.CreditCard.Value));
                    Installments installments = lista.Where(e => e.InstallmentsNumber == Payment.CreditCard.InstallmentsNumber).FirstOrDefault();

                    Transaction transaction = _payment.GenerateCreditCardPayment(ma01user, ma18TripitemshoppingcartList, Payment.ma33UserAddress.ma33CPF,
                                              Payment.CreditCard, Payment.ma33UserAddress, installments);
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
            PaymentModel paymentModel = new PaymentModel()
            {
                choosedSubtripSaleDateList = choosedSubtripSaleDateList,
                Payment = paymentList,
                influencerDiscountCode = Payment.influencerDiscountCode,
                userAddress = Payment.ma33UserAddress
            };
            string json = JsonConvert.SerializeObject(paymentModel);
            HttpResponseMessage response = await client.PostAsync("Sale/SubmitSale", new StringContent(json, Encoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode)
            {
                if (Payment.paymentMethods.Count() > 1)
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

        
        public List<ma24payment> RefundAllTransactions(List<Transaction> TransactionList)
        {
            List<ma24payment> paymentList = new List<ma24payment>();
            foreach (Transaction Transaction in TransactionList)
            {
                if (TransactionStatus.Refused == Transaction.Status)
                {
                    ma24payment payment = new ma24payment()
                    {
                        ma24CardFlag = Transaction.CardBrand.ToString(),
                        ma24CreditCard = true,
                        ma24TransactionID = Transaction.Id,
                        ma24Transference = false,
                        ma24Installments = Transaction.Installments,
                        ma24paymentStatus = 0
                    };
                    paymentList.Add(payment);
                }
                else
                {
                    _payment.Chargeback(Transaction.Id);
                    ma24payment payment = new ma24payment()
                    {
                        ma24CardFlag = Transaction.CardBrand.ToString(),
                        ma24CreditCard = true,
                        ma24TransactionID = Transaction.Id,
                        ma24Transference = false,
                        ma24Installments = Transaction.Installments,
                        ma24paymentStatus = 3
                    };
                    paymentList.Add(payment);
                }
            }
            return paymentList;
        }
        public List<ma24payment> GetPaidTransactions(List<Transaction> TransactionList)
        {
            List<ma24payment> paymentList = new List<ma24payment>();
            foreach (Transaction transaction in TransactionList)
            {
                ma24payment payment = new ma24payment()
                {
                    ma24CardFlag = transaction.CardBrand.ToString(),
                    ma24CreditCard = true,
                    ma24TransactionID = transaction.Id,
                    ma24Transference = false,
                    ma24Installments = transaction.Installments,
                    ma24paymentStatus = 1
                };
                paymentList.Add(payment);
            }
            return paymentList;
        }


        

        
    }
}
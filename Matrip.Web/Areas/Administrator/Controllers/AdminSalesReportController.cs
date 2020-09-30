using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

using Matrip.Domain.Models.AccountModels;
using Matrip.Domain.Models.GuideModels;
using Matrip.Web.Libraries.Filter;
using Matrip.Web.Libraries.Login;
using Matrip.Domain.Models.Entities;
using Matrip.Web.Libraries.Management.Payment.PagarMe;

namespace Matrip.Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [AdministratorAuthentication]
    public class AdminSalesReportController : Controller
    {
        private UserLogin _userLogin;
        private IConfiguration _configuration;
        private HttpClient client;
        private ManagePagarMe _managePagarMe;

        public AdminSalesReportController(UserLogin UserLogin, IConfiguration configuration, ManagePagarMe managePagarMe)
        {
            _userLogin = UserLogin;
            _configuration = configuration;
            _managePagarMe = managePagarMe;
            if (client == null)
            {
                client = new HttpClient();
                client.BaseAddress = new Uri(_configuration.GetValue<string>("Matrip.API:ApiLink"));
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            }
        }
        //Action para inserir a pesquisa de relatório por data
        [HttpGet]
        public IActionResult AdminSearchPartner()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] DateTime initialDate, [FromQuery]DateTime finalDate,
              [FromQuery] int DateType, [FromQuery]string PartnerName = "")
        {
            TokenModel JWToken = _userLogin.GetToken();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);
            
            /*se PartnerName for diferente de nulo ou for vazio, filtra a pesquisa para apenas 1 parceiro.
             * Se for nulo ou vazio, faz a busca completa das vendas.
             */
            HttpResponseMessage response = null;
            if(!string.IsNullOrEmpty(PartnerName))
            {
                string encodedPartnerName = HttpUtility.UrlEncode(PartnerName);

                response = await client.GetAsync("Administrator/PartnerReport?PartnerName=" + encodedPartnerName + "&initialDate=" + initialDate.ToString("yyyy-MM-dd") +
                                                                        "&finalDate=" + finalDate.ToString("yyyy-MM-dd") + "&DateType=" + DateType);
            }
            else
            {
                response = await client.GetAsync("Administrator/GetCompleteReport?initialDate=" + initialDate.ToString("yyyy-MM-dd") +
                                                                        "&finalDate=" + finalDate.ToString("yyyy-MM-dd") + "&DateType=" + DateType, 
                                                                        HttpCompletionOption.ResponseHeadersRead);
            }
            //se a busca der certo, transfere o relatório de venda da view
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                SaleReport saleReport = JsonConvert.DeserializeObject<SaleReport>(result);
                //parametros necessários para dar refresh na pagina quando algum link for ativado na view
                ViewBag.PartnerName = PartnerName;
                ViewBag.initialDate = initialDate;
                ViewBag.finalDate = finalDate;
                ViewBag.DateType = DateType;
                return View(saleReport);
            }
            //vai para a home se o usuário não for autorizado
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _userLogin.Logout();
                return RedirectToAction("Index", "Home", new { Area = "" });
            }
            //se não for encontrado o nome da empresa (se foi colocado uma na pesquisa), retorna a a action anterior
            else
            {
                TempData["MSG_E"] = "Não foi possível encontrar a empresa ou vendas de passeio neste período.";
                return RedirectToAction("AdminSearchPartner", "AdminSalesReport", new { Area = "Administrator" });
            }
        }

        [HttpGet]
        [ValidateHttpReferer]
        public async Task<IActionResult> TerminateSale([FromQuery] DateTime initialDate, [FromQuery]DateTime finalDate,
            [FromQuery] int SaleID, [FromQuery] int DateType, [FromQuery]string PartnerName = "")
        {
            TokenModel JWToken = _userLogin.GetToken();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);
            HttpResponseMessage responseSale = await client.GetAsync("Administrator/TerminateSale?SaleID=" + SaleID);
            if (responseSale.IsSuccessStatusCode)
            {
                TempData["MSG_S"] = "Ação realizada com sucesso.";
            }//vai para a home se o usuário não for autorizado
            else if (responseSale.StatusCode == HttpStatusCode.Unauthorized)
            {
                _userLogin.Logout();
                return RedirectToAction("Index", "Home", new { Area = "" });
            }
            else
            {
                TempData["MSG_E"] = "Ocorreu um erro! Tente novamente.";
            }
            return RedirectToAction("Index", new { initialDate, finalDate, DateType, PartnerName });
        }

        [HttpGet]
        [ValidateHttpReferer]
        public async Task<IActionResult> UnterminateSale([FromQuery] DateTime initialDate, [FromQuery]DateTime finalDate,
             [FromQuery] int CancelSaleID, [FromQuery] int DateType, [FromQuery]string PartnerName = "")
        {
            TokenModel JWToken = _userLogin.GetToken();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);
            HttpResponseMessage responseSale = await client.GetAsync("Administrator/UnterminateSale?SaleID=" + CancelSaleID);
            if (responseSale.IsSuccessStatusCode)
            {
                TempData["MSG_S"] = "Ação realizada com sucesso.";
            }//vai para a home se o usuário não for autorizado
            else if (responseSale.StatusCode == HttpStatusCode.Unauthorized)
            {
                _userLogin.Logout();
                return RedirectToAction("Index", "Home", new { Area = "" });
            }
            else
            {
                TempData["MSG_E"] = "Ocorreu um erro! Tente novamente.";
            }
            return RedirectToAction("Index", new { initialDate, finalDate, DateType, PartnerName });
        }

        [HttpGet]
        [ValidateHttpReferer]
        public async Task<IActionResult> TerminateTransferenceStatus([FromQuery] DateTime initialDate, [FromQuery]DateTime finalDate,
            [FromQuery] int SaleID, [FromQuery] int DateType, [FromQuery]string PartnerName = "")
        {
            TokenModel JWToken = _userLogin.GetToken();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);
            HttpResponseMessage response = await client.GetAsync("Administrator/TerminateTransferenceStatus?SaleID=" + SaleID);

            if (response.IsSuccessStatusCode)
            {
                TempData["MSG_S"] = "Ação realizada com sucesso.";
            }//vai para a home se o usuário não for autorizado
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _userLogin.Logout();
                return RedirectToAction("Index", "Home", new { Area = "" });
            }
            else
            {
                TempData["MSG_E"] = "Ocorreu um erro! Tente novamente.";
            }
            return RedirectToAction("Index", new { initialDate, finalDate, DateType, PartnerName });
        }

        [HttpGet]
        [ValidateHttpReferer]
        public async Task<IActionResult> DisapproveTransferenceStatus([FromQuery] DateTime initialDate, [FromQuery]DateTime finalDate,
            [FromQuery] int? SaleID, [FromQuery] int DateType, [FromQuery]string PartnerName = "")
        {
            TokenModel JWToken = _userLogin.GetToken();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);
            HttpResponseMessage response = await client.GetAsync("Administrator/DisapproveTransferenceStatus?SaleID=" + SaleID.Value);

            if (response.IsSuccessStatusCode)
            {
                TempData["MSG_S"] = "Ação realizada com sucesso.";
            }//vai para a home se o usuário não for autorizado
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _userLogin.Logout();
                return RedirectToAction("Index", "Home", new { Area = "" });
            }
            else
            {
                TempData["MSG_E"] = "Ocorreu um erro! Tente novamente.";
            }
            return RedirectToAction("Index", new { initialDate, finalDate, DateType, PartnerName });
        }

        [HttpGet]
        [ValidateHttpReferer]
        public async Task<IActionResult> CancelSale([FromQuery] DateTime initialDate, [FromQuery]DateTime finalDate,
            [FromQuery] int SaleID, [FromQuery] int DateType, [FromQuery]string PartnerName = "")
        {
            TokenModel JWToken = _userLogin.GetToken();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);
            HttpResponseMessage response = await client.GetAsync("Administrator/CancelSale?SaleID=" + SaleID);
            if (response.IsSuccessStatusCode)
            {
                string resultSale = await response.Content.ReadAsStringAsync();
                ma32sale sale = JsonConvert.DeserializeObject<ma32sale>(resultSale);
                try
                {
                    foreach (ma24payment payment in sale.ma24payment.ToList())
                    {
                        if (payment.ma24CreditCard)
                        {
                            _managePagarMe.Chargeback(payment.ma24TransactionID);
                        }
                    }
                }
                catch
                {
                    TempData["MSG_E"] = "Não foi possível estornar o cartão. Favor, realizar o estorno no site do PagarMe.";
                }
            }//vai para a home se o usuário não for autorizado
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _userLogin.Logout();
                return RedirectToAction("Index", "Home", new { Area = "" });
            }
            else
            {
                TempData["MSG_E"] = "Ocorreu um erro inesperado na API da Matrip. Favor, contate os desenvolvedores.";
            }
            return RedirectToAction("Index", new { initialDate, finalDate, DateType, PartnerName });
        }


    }
}
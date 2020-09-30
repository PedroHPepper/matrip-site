using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Matrip.Domain.Models.Entities;
using Matrip.Domain.Models.GuideModels;
using Matrip.Web.Database;
using Matrip.Web.Repositories.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Matrip.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdministratorController : ControllerBase
    {
        private ApplicationDbContext _dbContext;
        private Ima25PartnerRepository _ma25partnerRepository;
        private UserManager<ma01user> _userManager;
        private Ima05TripRepository _ma05TripRepository;
        private Ima04GuideRepository _ma04guideRepository;
        private Ima32saleRepository _ma32saleRepository;
        private Ima22SubTripSaleRepository _ma22SubTripSaleRepository;
        private Ima24PaymentRepository _ma24PaymentRepository;
        private Ima13TripPhotoRepository _ma13TripPhotoRepository;
        public AdministratorController(ApplicationDbContext dbContext, Ima25PartnerRepository partnerRepository,
            UserManager<ma01user> userManager, Ima05TripRepository tripRepository, Ima04GuideRepository guideRepository,
            Ima22SubTripSaleRepository subTripSaleRepository, Ima32saleRepository saleRepository,
            Ima24PaymentRepository paymentRepository, Ima13TripPhotoRepository tripPhotoRepository)
        {
            _dbContext = dbContext;
            _ma25partnerRepository = partnerRepository;
            _userManager = userManager;
            _ma05TripRepository = tripRepository;
            _ma04guideRepository = guideRepository;
            _ma22SubTripSaleRepository = subTripSaleRepository;
            _ma32saleRepository = saleRepository;
            _ma24PaymentRepository = paymentRepository;
            _ma13TripPhotoRepository = tripPhotoRepository;
        }

        

        [HttpGet("PartnerReport")]
        public async Task<IActionResult> PartnerReport(string PartnerName, DateTime initialDate, DateTime finalDate, int DateType)
        {
            ma01user user = await _userManager.GetUserAsync(HttpContext.User);
            if (user.ma01type != "admin")
            {
                return Unauthorized();
            }
            ma25partner partner = _ma25partnerRepository.GetPartnerWithSubtrips(PartnerName, null);
            if (partner == null)
            {
                return BadRequest();
            }
            List<ma22subtripsale> subtripsales = new List<ma22subtripsale>();
            foreach (ma14subtrip subtrip in partner.ma14subtrip)
            {
                List<ma22subtripsale> saleList = _ma22SubTripSaleRepository.GetSubtripReport(subtrip, initialDate, finalDate, DateType);
                if (saleList != null)
                {
                    subtripsales.AddRange(saleList);
                }
            }
            List<ma25partner> partnerList = new List<ma25partner>();
            partnerList.Add(partner);
            SaleReport saleReport = new SaleReport
            {
                ma22subtripsaleList = subtripsales,
                ma25partnerList = partnerList
            };
            return Ok(saleReport);
        }

        [HttpGet("GetCompleteReport")]
        public async Task<IActionResult> GetCompleteReport(DateTime initialDate, DateTime finalDate, int DateType)
        {
            ma01user user = await _userManager.GetUserAsync(HttpContext.User);
            if (user.ma01type != "admin")
            {
                return Unauthorized();
            }
            
            List<ma22subtripsale> subtripsales = _ma22SubTripSaleRepository.GetSubtripReport(initialDate, finalDate, DateType);
            List<ma25partner> partnerList = new List<ma25partner>();
            foreach(ma22subtripsale subtripsale in subtripsales)
            {
                if (!partnerList.Contains(subtripsale.ma14subtrip.ma25partner))
                {
                    if(subtripsale.ma14subtrip.ma25partner != null)
                    {
                        partnerList.Add(subtripsale.ma14subtrip.ma25partner);
                    }
                    subtripsale.ma14subtrip.ma25partner = null;
                    subtripsale.ma14subtrip.ma22subtripsale = null;
                    subtripsale.ma14subtrip.ma11service = null;

                }
            }

            SaleReport saleReport = new SaleReport
            {
                ma22subtripsaleList = subtripsales,
                ma25partnerList = partnerList
            };
            return Ok(saleReport);
        }

        [HttpGet("TerminateSale")]
        public async Task<IActionResult> TerminateSale(int SaleID)
        {
            ma01user user = await _userManager.GetUserAsync(HttpContext.User);
            if (user.ma01type != "admin")
            {
                return Unauthorized();
            }
            ma32sale sale = _ma32saleRepository.GetById(SaleID);
            if(sale != null)
            {
                sale.ma32terminate = "1";
                _ma32saleRepository.Update(sale);

                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet("UnterminateSale")]
        public async Task<IActionResult> UnterminateSale(int SaleID)
        {
            ma01user user = await _userManager.GetUserAsync(HttpContext.User);
            if (user.ma01type != "admin")
            {
                return Unauthorized();
            }
            ma32sale sale = _ma32saleRepository.GetById(SaleID);
            if (sale != null)
            {
                sale.ma32terminate = "0";
                _ma32saleRepository.Update(sale);

                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("TerminateTransferenceStatus")]
        public async Task<IActionResult> TerminateTransferenceStatus(int SaleID)
        {
            try
            {
                ma01user user = await _userManager.GetUserAsync(HttpContext.User);
                if (user.ma01type != "admin")
                {
                    return Unauthorized();
                }
                ma32sale sale = _ma32saleRepository.GetSale(SaleID);
                if (sale != null)
                {
                    foreach (ma24payment payment in sale.ma24payment.ToList())
                    {
                        if (payment.ma24Transference)
                        {
                            sale.ma32situation = 1;
                            _ma32saleRepository.Update(sale);
                            payment.ma24paymentStatus = 1;
                            _ma24PaymentRepository.Update(payment);
                        }
                    }
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            } 
        }

        [HttpGet("DisapproveTransferenceStatus")]
        public async Task<IActionResult> DisapproveTransferenceStatus(int SaleID)
        {
            try
            {
                ma01user user = await _userManager.GetUserAsync(HttpContext.User);
                if (user.ma01type != "admin")
                {
                    return Unauthorized();
                }
                ma32sale sale = _ma32saleRepository.GetSale(SaleID);
                if (sale != null)
                {
                    foreach (ma24payment payment in sale.ma24payment.ToList())
                    {
                        if (payment.ma24Transference)
                        {
                            sale.ma32situation = 3;
                            _ma32saleRepository.Update(sale);
                            payment.ma24paymentStatus = 3;
                            _ma24PaymentRepository.Update(payment);
                        }
                    }
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("CancelSale")]
        public async Task<IActionResult> CancelSale(int SaleID)
        {
            try
            {
                ma01user user = await _userManager.GetUserAsync(HttpContext.User);
                if (user.ma01type != "admin")
                {
                    return Unauthorized();
                }
                ma32sale sale = _ma32saleRepository.GetSale(SaleID);
                if (sale != null)
                {
                    foreach (ma24payment payment in sale.ma24payment.ToList())
                    {
                        payment.ma24paymentStatus = 2;
                        _ma24PaymentRepository.Update(payment);
                    }
                    sale.ma32situation = 2;
                    _ma32saleRepository.Update(sale);
                    return Ok(sale);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
    }
}
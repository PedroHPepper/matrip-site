using Matrip.Domain.Models.Entities;
using Matrip.Domain.Models.GuideModels;
using Matrip.Domain.Models.TripModel;
using Matrip.Web.Database;
using Matrip.Web.Repositories.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Matrip.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GuideController : ControllerBase
    {
        private ApplicationDbContext _dbContext;
        private Ima25PartnerRepository _ma25partnerRepository;
        private UserManager<ma01user> _userManager;
        private Ima05TripRepository _ma05TripRepository;
        private Ima04GuideRepository _ma04guideRepository;
        private Ima22SubTripSaleRepository _ma22SubTripSaleRepository;
        public GuideController(ApplicationDbContext dbContext, Ima25PartnerRepository partnerRepository,
            UserManager<ma01user> userManager, Ima05TripRepository tripRepository, Ima04GuideRepository guideRepository,
            Ima22SubTripSaleRepository subTripSaleRepository)
        {
            _dbContext = dbContext;
            _ma25partnerRepository = partnerRepository;
            _userManager = userManager;
            _ma05TripRepository = tripRepository;
            _ma04guideRepository = guideRepository;
            _ma22SubTripSaleRepository = subTripSaleRepository;
        }

        [HttpGet("GetTripList")]
        public async Task<IActionResult> GetTripList()
        {
            ma01user ma01user = await _userManager.GetUserAsync(HttpContext.User);
            if (ma01user.ma01type != "guide")
            {
                return Unauthorized();
            }
            List<ma05trip> tripList = _ma05TripRepository.GetGuideTripList(ma01user.Id);
            return Ok(tripList);
        }
        public async Task<IActionResult> GetTripAsync(int TripID)
        {
            try
            {
                ma01user user = await _userManager.GetUserAsync(HttpContext.User);
                // ma05trip trip

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("GetPartners")]
        public async Task<IActionResult> GetPartners()
        {
            ma01user user = await _userManager.GetUserAsync(HttpContext.User);
            if (user.ma01type != "guide")
            {
                return Unauthorized();
            }
            ma04guide guide = _ma04guideRepository.GetGuidePartnerList(user.Id);
            foreach(ma26PartnerGuide partnerGuide in guide.ma26PartnerGuide)
            {
                if(partnerGuide.FK2604idGuide == guide.ma04idguide && partnerGuide.ma26status == "0")
                {
                    guide.ma26PartnerGuide.Remove(partnerGuide);
                }
            }

            return Ok(guide);
        }

        [HttpGet("PartnerReport")]
        public async Task<IActionResult> PartnerReport(int partnerID, DateTime initialDate, DateTime finalDate, int DateType)
        {
            ma01user user = await _userManager.GetUserAsync(HttpContext.User);
            if (user.ma01type != "guide")
            {
                return Unauthorized();
            }
            ma04guide guide = _ma04guideRepository.GetGuideByUserId(user.Id);
            ma25partner partner = _ma25partnerRepository.GetPartnerWithSubtrips(partnerID, guide.ma04idguide);
            if(partner == null)
            {
                return BadRequest();
            }
            
            List<ma22subtripsale> subtripsales = new List<ma22subtripsale>();
            foreach (ma14subtrip subtrip in partner.ma14subtrip)
            {
                List<ma22subtripsale> saleList = _ma22SubTripSaleRepository.GetSubtripReport(subtrip, initialDate, finalDate, DateType);
                if(saleList != null)
                {
                    subtripsales.AddRange(saleList);
                }
            }
            List<ma25partner> partnerList = new List<ma25partner>();
            partnerList.Add(partner);
            SaleReport saleReport = new SaleReport
            {
                ma04guide = guide,
                ma22subtripsaleList = subtripsales,
                ma25partnerList = partnerList
            };
            return Ok(saleReport);
            
        }
        [Authorize]
        [HttpPost("RegisterTrip")]
        public async Task<IActionResult> RegisterTrip([FromBody] TripModel tripModel)
        {
            ma01user ma01user = await _userManager.GetUserAsync(HttpContext.User);
            if (ma01user.ma01type != "guide" || ma01user.ma01type != "admin")
            {
                return Unauthorized();
            }
            /*Controller de registro para passeios. Aqui é preciso ter as chaves estrangeiras para serem atribuídas ao passeio
             *Chaves estrangeiras de categoria, cidade e o guia.
             */
            if (ModelState.IsValid)
            {
                _dbContext.ma05trip.Add(tripModel.ma05trip);
                tripModel.ma27AgeDiscountList.ForEach(e => e.FK2705idTrip = tripModel.ma05trip.ma05idtrip);
                _dbContext.ma27AgeDiscount.AddRange(tripModel.ma27AgeDiscountList);

                foreach (SubtripModel subtripModel in tripModel.SubtripModelList)
                {
                    subtripModel.ma14subtrip.FK1405idtrip = tripModel.ma05trip.ma05idtrip;
                    _dbContext.ma14subtrip.Add(subtripModel.ma14subtrip);

                    if (subtripModel.ma11serviceList != null)
                    {
                        subtripModel.ma11serviceList.ForEach(e => e.FK1114idsubtrip = subtripModel.ma14subtrip.ma14idsubtrip);
                        _dbContext.ma11service.AddRange(subtripModel.ma11serviceList);
                    }
                    subtripModel.ma16subtripscheduleList.ForEach(e => e.FK1614idsubtrip = subtripModel.ma14subtrip.ma14idsubtrip);
                    _dbContext.ma16subtripschedule.AddRange(subtripModel.ma16subtripscheduleList);

                    subtripModel.ma17SubtripValueList.ForEach(e => e.FK1714idsubtrip = subtripModel.ma14subtrip.ma14idsubtrip);
                    _dbContext.ma17SubtripValue.AddRange(subtripModel.ma17SubtripValueList);
                }
            }
            else
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
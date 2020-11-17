using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Matrip.Domain.Libraries.Email;
using Matrip.Domain.Libraries.Text;
using Matrip.Domain.Models.Entities;
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
    public class FeedbackController : ControllerBase
    {
        private UserManager<ma01user> _userManager;
        private ApplicationDbContext _dbContext;
        private Ima32saleRepository _ma32saleRepository;
        private Ima21SaleTripRepository _ma21SaleTripRepository;
        private EmailManagement _emailManagement;
        private Ima39tripEvaluationRepository _ma39tripEvaluationRepository;
        private DateTime dateTimeNow = DateConvert.HrBrasilia();

        public FeedbackController(ApplicationDbContext context,
             Ima32saleRepository saleRepository, UserManager<ma01user> userManager, Ima22SubTripSaleRepository subTripSaleRepository,
            Ima28SaleTouristRepository saleTouristRepository, Ima23ServiceSaleRepository serviceSaleRepository,
            Ima16SubTripScheduleRepository subTripScheduleRepository, Ima21SaleTripRepository saleTripRepository,
            Ima39tripEvaluationRepository ma39tripEvaluationRepository, EmailManagement emailManagement)
        {
            _userManager = userManager;
            _dbContext = context;
            _ma32saleRepository = saleRepository;
            _ma21SaleTripRepository = saleTripRepository;
            _ma39tripEvaluationRepository = ma39tripEvaluationRepository;
            _emailManagement = emailManagement;
        }


        [Authorize]
        [HttpGet("GetConcludedSales/{SaleID}")]
        public IActionResult GetConcludedSales(int SaleID)
        {
            try
            {
                ma32sale sale = _ma32saleRepository.GetSaleToFeedback(SaleID);
                return Ok(sale);
            }
            catch (Exception)
            {
                return BadRequest("Não foi possível resgatar a compra.");
            }
        }


        [Authorize]
        [HttpGet("GetConcludedSaleTrip/{SaleTripID}")]
        public async Task<IActionResult> GetConcludedSaleTripAsync(int SaleTripID)
        {
            try
            {
                ma01user user = await _userManager.GetUserAsync(HttpContext.User);

                ma21saleTrip saleTrip = _ma21SaleTripRepository.GetSaleTripToFeedback(SaleTripID);
                return Ok(saleTrip);
            }
            catch (Exception)
            {
                return BadRequest("Não foi possível resgatar a compra.");
            }
        }

        [Authorize]
        [HttpPost("PostFeedback")]
        public async Task<IActionResult> PostFeedback([FromBody] ma39tripEvaluation tripEvaluation)
        {
            try
            {
                ma01user user = await _userManager.GetUserAsync(HttpContext.User);
                tripEvaluation.FK3901idUser = user.Id;
                _ma39tripEvaluationRepository.Add(tripEvaluation);

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("Não foi possível adicionar a avaliação.");
            }
        }
    }
}
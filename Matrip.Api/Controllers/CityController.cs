using Matrip.Domain.Models.CityModel;
using Matrip.Domain.Models.Entities;
using Matrip.Domain.Models.HomeModels;
using Matrip.Web.Domain.Models;
using Matrip.Web.Repositories.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Matrip.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private UserManager<ma01user> _userManager;
        private Ima08UFRepository _ufRepository;
        private Ima09CityRepository _cityRepository;
        private Ima05TripRepository _ma05tripRepository;
        public CityController(Ima08UFRepository uFRepository, Ima09CityRepository cityRepository, Ima05TripRepository tripRepository,
            UserManager<ma01user> userManager)
        {
            _userManager = userManager;
            _ufRepository = uFRepository;
            _cityRepository = cityRepository;
            _ma05tripRepository = tripRepository;
        }

        [HttpGet("GetCity")]
        public IActionResult GetCity(int CityID)
        {
            ma09city city = _cityRepository.GetCityByID(CityID);
            return Ok(city);
        }

        [Authorize]
        [HttpGet("GetCityByName")]
        public IActionResult GetCityByName(string UF, string CityName)
        {
            try
            {
                ma08uf uf = _ufRepository.GetByInitials(UF);
                ma09city city = _cityRepository.GetByName(uf.ma08iduf, CityName);
                if(city == null)
                {
                    throw new Exception();
                }
                return Ok(city);
            }
            catch
            {
                return BadRequest("Não foi possível encontrar cidade!");
            }
        }

        [Authorize]
        [HttpPost("UpdateCity")]
        public async Task<IActionResult> UpdateCity([FromBody] CityModel City)
        {
            try
            {
                ma01user user = await _userManager.GetUserAsync(HttpContext.User);
                if (user.ma01type != "admin")
                {
                    return Unauthorized();
                }
                ma08uf uf = _ufRepository.GetByInitials(City.UF);
                ma09city city = _cityRepository.GetByName(uf.ma08iduf, City.CityName);
                if (city == null)
                {
                    throw new Exception();
                }
                city.ma09Description = City.CityDescription;
                _cityRepository.Update(city);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GetHomeCityList")]
        public IActionResult GetHomeCityList(string cityText)
        {
            try
            {
                List<ma09city> ma09cityList = _cityRepository.GetSearch(cityText, "");
                return Ok(ma09cityList);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("SearchCities")]
        public IActionResult SearchCities(string cityText, string UF)
        {
            try
            {
                List<ma09city> cities = _cityRepository.GetSearch(cityText, UF);
                return Ok(cities);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        
    }
}
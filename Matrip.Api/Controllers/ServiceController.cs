using Matrip.Domain.Models.Entities;
using Matrip.Web.Repositories.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Matrip.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {

        private UserManager<ma01user> _userManager;
        private Ima05TripRepository _tripRepository;
        private Ima06TripCategoryRepository _tripCategoryRepository;
        private Ima08UFRepository _ufRepository;
        private Ima09CityRepository _cityRepository;
        private Ima11ServiceRepository _serviceRepository;
        private Ima04GuideRepository _guideRepository;
        public ServiceController(UserManager<ma01user> userManager, Ima05TripRepository tripRepository, Ima06TripCategoryRepository tripCategoryRepository,
            Ima08UFRepository uFRepository, Ima09CityRepository cityRepository, Ima11ServiceRepository serviceRepository, Ima04GuideRepository guideRepository)
        {
            _userManager = userManager;
            _tripRepository = tripRepository;
            _tripCategoryRepository = tripCategoryRepository;
            _ufRepository = uFRepository;
            _cityRepository = cityRepository;
            _serviceRepository = serviceRepository;
            _guideRepository = guideRepository;
        }

        [HttpGet("GetServicesList/{SubTripID}")]
        public IActionResult GetServicesList(int SubTripID)
        {

            var services = _serviceRepository.GetSubTripServiceList(SubTripID);
            if (services == null)
            {
                return NotFound();
            }

            return Ok(services);
        }

        [HttpGet("GetService/{serviceID}")]
        public IActionResult GetService(int serviceID)
        {
            ma11service ma11service = _serviceRepository.GetById(serviceID);
            if (ma11service == null)
            {
                return NotFound();
            }
            return Ok(ma11service);
        }

        [Authorize]
        [HttpPost("RegisterService")]
        public async Task<IActionResult> RegisterService([FromBody] ma11service ma11service)
        {
            ma01user ma01user = await _userManager.GetUserAsync(HttpContext.User);
            /*
            ma05trip ma05trip = _tripRepository.GetById(ma11service.FK1114idsubtrip);
            ma04guide ma04guide = _guideRepository.GetById(ma05trip.FK0504idguide);

            if (ModelState.IsValid && ma01user.Id == ma04guide.ma04idguide)
            {
                _serviceRepository.Add(ma11service);
            }
            else
            {
                return BadRequest();
            }
            */
            return Ok();
        }

        [Authorize]
        [HttpPost("UpdateService")]
        public async Task<IActionResult> UpdateService([FromBody] ma11service ma11service)
        {
            ma01user ma01user = await _userManager.GetUserAsync(HttpContext.User);
            /*
            if (ModelState.IsValid && ma01user.ma01type == "guide")
            {
                ma05trip ma05trip = _tripRepository.GetById(ma11service.FK1114idsubtrip);
                ma04guide ma04guide = _guideRepository.GetById(ma05trip.FK0504idguide);
                if(ma04guide.FK0401iduser == ma01user.Id)
                {
                    _serviceRepository.Update(ma11service);
                }
            }
            else
            {
                return BadRequest();
            }
            */
            return Ok();
        }

        [Authorize]
        [HttpDelete("DeleteService/{TripId}/{ServiceId}")]
        public async Task<IActionResult> DeleteService(int TripId, int ServiceId)
        {
            ma01user ma01user = await _userManager.GetUserAsync(HttpContext.User);
            /*
            if (ma01user.ma01type == "guide")
            {
                ma05trip ma05trip = _tripRepository.GetById(TripId);
                ma11service ma11service = _serviceRepository.GetById(ServiceId);

                if (ma01user.Id != ma05trip.FK0504idguide || ma11service.FK1105idtrip != ma05trip.ma05idtrip)
                {
                    return BadRequest();
                }

                _serviceRepository.Remove(ma11service);

                return Ok();
            }
            return BadRequest();
            */
            return Ok();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Matrip.Domain.Libraries.Text;
using Matrip.Domain.Models.Entities;
using Matrip.Web.Repositories.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Matrip.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private UserManager<ma01user> _userManager;
        private Ima13TripPhotoRepository _ma13TripPhotoRepository;
        private Ima35cityphotoRepository _ma35photoRepository;
        public PhotoController(UserManager<ma01user> userManager, Ima13TripPhotoRepository tripPhotoRepository, 
            Ima35cityphotoRepository cityphotoRepository)
        {
            _userManager = userManager;
            _ma13TripPhotoRepository = tripPhotoRepository;
            _ma35photoRepository = cityphotoRepository;
        }

        [HttpGet("AddTripPhoto/{tripID}")]
        public async Task<IActionResult> AddTripPhoto(int tripID)
        {
            ma01user user = await _userManager.GetUserAsync(HttpContext.User);
            if (user.ma01type != "admin" && user.ma01type != "guide")
            {
                return Unauthorized();
            }
            ma13tripphoto TripPhoto = _ma13TripPhotoRepository.GetByTripID(tripID);
            if (TripPhoto == null)
            {
                TripPhoto = new ma13tripphoto()
                {
                    FK1305idtrip = tripID,
                    ma13photoquantity = 1,
                    ma13versionDate = DateConvert.HrBrasilia()
                };
                _ma13TripPhotoRepository.Add(TripPhoto);
            }
            else
            {
                TripPhoto.ma13photoquantity += 1;
                TripPhoto.ma13versionDate = DateConvert.HrBrasilia();
                _ma13TripPhotoRepository.Update(TripPhoto);
            }
            return Ok(TripPhoto);
        }

        [HttpGet("DeleteTripPhoto/{tripID}")]
        public async Task<IActionResult> DeleteTripPhoto(int tripID)
        {
            ma01user user = await _userManager.GetUserAsync(HttpContext.User);
            if (user.ma01type != "admin" && user.ma01type != "guide")
            {
                return Unauthorized();
            }
            ma13tripphoto TripPhoto = _ma13TripPhotoRepository.GetByTripID(tripID);
            if (TripPhoto == null)
            {
                return BadRequest();
            }
            else
            {
                if (TripPhoto.ma13photoquantity > 0)
                {
                    TripPhoto.ma13photoquantity -= 1;
                    TripPhoto.ma13versionDate = DateConvert.HrBrasilia();
                    _ma13TripPhotoRepository.Update(TripPhoto);
                }
            }
            return Ok(TripPhoto);
        }


        [Authorize]
        [HttpGet("AddCityPhoto/{CityID}")]
        public async Task<IActionResult> AddCityPhoto(int CityID)
        {
            ma01user user = await _userManager.GetUserAsync(HttpContext.User);
            if (user.ma01type != "admin" && user.ma01type != "guide")
            {
                return Unauthorized();
            }
            ma35cityphoto cityphoto = new ma35cityphoto()
            {
                FK3509idcity = CityID,
                ma35versionDate = DateConvert.HrBrasilia()
            };
            _ma35photoRepository.Add(cityphoto);
            _ma35photoRepository.Save();
            return Ok(cityphoto);
        }

        [Authorize]
        [HttpGet("UpdateCityPhoto/{CityID}")]
        public async Task<IActionResult> UpdateCityPhoto(int CityID)
        {
            ma01user user = await _userManager.GetUserAsync(HttpContext.User);
            if (user.ma01type != "admin" && user.ma01type != "guide")
            {
                return Unauthorized();
            }
            ma35cityphoto cityphoto = _ma35photoRepository.GetCityPhoto(CityID);
            cityphoto.ma35versionDate = DateConvert.HrBrasilia();
            _ma35photoRepository.Update(cityphoto);
            return Ok();
        }
    }
}
using Matrip.Domain.Models.Entities;
using Matrip.Web.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Matrip.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubTripController : ControllerBase
    {
        private Ima14SubTripRepository _subTripRepository;
        public SubTripController(Ima14SubTripRepository subTripRepository)
        {
            _subTripRepository = subTripRepository;
        }


        public IActionResult GetSubTripList(int TripID)
        {
            List<ma14subtrip> ma14subtrip = _subTripRepository.GetSubTripList(TripID);

            return Ok(ma14subtrip);
        }

    }
}
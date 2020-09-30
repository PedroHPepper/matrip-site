using Matrip.Domain.Models.Entities;
using Matrip.Web.Libraries.Filter;
using Matrip.Web.Libraries.Login;
using Microsoft.AspNetCore.Mvc;

namespace Matrip.Web.Areas.Guide.Controllers
{
    [Area("Guide")]
    [GuideAuthentication]
    public class GuideTripController : Controller
    {
        private UserLogin _userLogin;

        public GuideTripController(UserLogin UserLogin)
        {
            _userLogin = UserLogin;
        }
        public IActionResult Index(int? page)
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetTripList(int? page)
        {

            //int userID = _userLogin.GetUser().ma01IDuser;
            //ma04guide ma04guide = _guideRepository.GetGuideByUserId(userID);

            var trips = 1;// = _tripRepository.GetGuideTripList(page, 1);//ma04guide.ma04idguide);
            return View("ViewTripList", trips);
        }
        [HttpGet]
        public IActionResult GetTrip(int tripID)
        {
            ma05trip ma05trip = null;// = _tripRepository.GetById(tripID);

            return View("ViewTrip", ma05trip);
        }
        [HttpGet]
        public IActionResult RegisterTrip()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RegisterTrip([FromForm] ma05trip ma05trip, int tripCategoryID, int cityID)
        {
            /*Controller de registro para passeios. Aqui é preciso ter as chaves estrangeiras para serem atribuídas ao passeio
             *Chaves estrangeiras de categoria, cidade e o guia.
             */
            ma05trip.FK0506idtripcategory = tripCategoryID;
            //ma05trip.FK0504idguide = _userLogin.GetUser().ma01IDuser;
            ma05trip.FK0509idcity = cityID;

            if (ModelState.IsValid)
            {
                //_tripRepository.Add(ma05trip);
            }
            else
            {
                ViewData["MSG_E"] = "Dados preenchidos incorretamente!";
            }

            return View();
        }
        [HttpGet]
        public IActionResult UpdateTrip(int tripID)
        {
            ma05trip ma05trip = null;// = _tripRepository.GetById(tripID);
            return View(ma05trip);
        }
        [HttpPost]
        public IActionResult UpdateTrip([FromForm] ma05trip ma05trip)
        {
            if (ModelState.IsValid)
            {
                //_tripRepository.Update(ma05trip);
            }
            else
            {
                ViewData["MSG_E"] = "Dados preenchidos incorretamente!";
            }

            return View();
        }
        public IActionResult DeleteTrip(int tripID)
        {
            //TODO - Escrever a exclusão dos passeios
            // ma05trip ma05trip = null;// = _tripRepository.GetById(tripID);
            // _tripRepository.Remove(ma05trip);
            return View();

        }
        [HttpGet]
        public IActionResult GetServicesList(int? page, int tripID)
        {
            var services = 1;//= _serviceRepository.GetTripServiceList(page, tripID);

            return View("ViewService", services);
        }
        [HttpGet]
        public IActionResult GetService(int serviceID)
        {
            ma11service ma11service = null;// = _serviceRepository.GetById(serviceID);

            return View(ma11service);
        }
        [HttpPost]
        public IActionResult RegisterService([FromForm] ma11service ma11service, int SubTripID)
        {
            ma11service.FK1114idsubtrip = SubTripID;

            if (ModelState.IsValid)
            {
                //_serviceRepository.Add(ma11service);
            }
            else
            {
                ViewData["MSG_E"] = "Dados preenchidos incorretamente!";
            }

            return View();
        }
        [HttpPost]
        public IActionResult UpdateService([FromForm] ma11service ma11service)
        {
            if (ModelState.IsValid)
            {
                //_serviceRepository.Update(ma11service);
            }
            else
            {
                ViewData["MSG_E"] = "Dados preenchidos incorretamente!";
            }

            return View();
        }
        public IActionResult DeleteService(int serviceID)
        {
            //TODO - Escrever a exclusão dos passeios
            //ma11service ma11service = null;// = _serviceRepository.GetById(serviceID);
            //_serviceRepository.Remove(ma11service);
            return View();

        }
    }
}
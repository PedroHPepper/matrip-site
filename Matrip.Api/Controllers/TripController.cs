using Matrip.Domain.Models.Entities;
using Matrip.Domain.Models.HomeModels;
using Matrip.Domain.Models.TripModel;
using Matrip.Web.Database;
using Matrip.Web.Domain.Models;
using Matrip.Web.Libraries.Pagination;
using Matrip.Web.Repositories.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace Matrip.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripController : ControllerBase
    {
        private UserManager<ma01user> _userManager;
        private Ima25PartnerRepository _ma25partnerRepository;
        private Ima05TripRepository _tripRepository;
        private Ima06TripCategoryRepository _tripCategoryRepository;
        private Ima27AgeDiscountRepository _ma27AgeDiscountRepository;
        private Ima16SubTripScheduleRepository _ma16SubTripScheduleRepository;
        private Ima08UFRepository _ufRepository;
        private Ima09CityRepository _cityRepository;
        private Ima35cityphotoRepository _cityphotoRepository;
        private Ima11ServiceRepository _serviceRepository;
        private Ima04GuideRepository _guideRepository;
        private Ima14SubTripRepository _subTripRepository;
        private Ima13TripPhotoRepository _tripPhotoRepository;
        private Ima17SubTripValueRepository _subTripValueRepository;
        private Ima12SubtripGuideRepository _subtripGuideRepository;
        public TripController(UserManager<ma01user> userManager, Ima05TripRepository tripRepository, Ima06TripCategoryRepository tripCategoryRepository,
            Ima08UFRepository uFRepository, Ima09CityRepository cityRepository, Ima35cityphotoRepository cityphotoRepository,
            Ima11ServiceRepository serviceRepository, Ima04GuideRepository guideRepository, Ima14SubTripRepository subTripRepository,
            Ima13TripPhotoRepository tripPhotoRepository, Ima17SubTripValueRepository subTripValueRepository,
            Ima12SubtripGuideRepository subtripGuideRepository, Ima25PartnerRepository partnerRepository, 
            Ima27AgeDiscountRepository ageDiscountRepository, Ima16SubTripScheduleRepository subTripScheduleRepository)
        {
            _userManager = userManager;
            _tripRepository = tripRepository;
            _tripCategoryRepository = tripCategoryRepository;
            _ufRepository = uFRepository;
            _cityRepository = cityRepository;
            _cityphotoRepository = cityphotoRepository;
            _serviceRepository = serviceRepository;
            _guideRepository = guideRepository;
            _subTripRepository = subTripRepository;
            _tripPhotoRepository = tripPhotoRepository;
            _subTripValueRepository = subTripValueRepository;
            _subtripGuideRepository = subtripGuideRepository;
            _ma25partnerRepository = partnerRepository;
            _ma27AgeDiscountRepository = ageDiscountRepository;
            _ma16SubTripScheduleRepository = subTripScheduleRepository;
        }

        [HttpGet("GetFeaturedTrips")]
        public IActionResult GetFeaturedTrips()
        {
            try
            {
                List<ma05trip> tripList = _tripRepository.GetFeaturedTrips();

                HomeIndexModel indexModel = new HomeIndexModel
                {
                    ma05tripList = tripList
                };
                return Ok(indexModel);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [AllowAnonymous]
        [HttpGet("byCategory")]
        public IActionResult GetTripList([FromQuery] string CategoryName, [FromQuery] string UF, [FromQuery] string CityName, [FromQuery] int? page)
        {
            try
            {

                ma06tripcategory ma06tripcategory = _tripCategoryRepository.GetByName(CategoryName);
                ma08uf ma08uf = _ufRepository.GetByInitials(UF);
                ma09city ma09city = _cityRepository.GetByName(ma08uf.ma08iduf, CityName);

                IPagedList<ma05trip> trips = _tripRepository.GetList(ma06tripcategory.ma06idtripcategory, ma09city.ma09idcity, page);
                PaginationEntity<ma05trip> result = new PaginationEntity<ma05trip>()
                {
                    Items = trips,
                    MetaData = new PaginationMetaData()
                    {
                        FirstItemOnPage = trips.GetMetaData().FirstItemOnPage,
                        HasNextPage = trips.GetMetaData().HasNextPage,
                        HasPreviousPage = trips.GetMetaData().HasPreviousPage,
                        IsFirstPage = trips.GetMetaData().IsFirstPage,
                        TotalItemCount = trips.GetMetaData().TotalItemCount,
                        IsLastPage = trips.GetMetaData().IsLastPage,
                        LastItemOnPage = trips.GetMetaData().LastItemOnPage,
                        PageCount = trips.GetMetaData().PageCount,
                        PageNumber = trips.GetMetaData().PageNumber,
                        PageSize = trips.GetMetaData().PageSize
                    }
                };
                TripListView tripListView = new TripListView() { lst = result, ma09city = ma09city, ma06tripcategory = ma06tripcategory, ma08uf = ma08uf };

                return Ok(tripListView);
            }
            catch (Exception e)
            {
                return BadRequest("Não foi possível consultar a lista de passeios:" + e.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("byCity")]
        public IActionResult GetTripList([FromQuery] string UF, [FromQuery] string CityName, [FromQuery] int? page)
        {
            try
            {
                ma08uf ma08uf = _ufRepository.GetByInitials(UF);
                ma09city ma09city = _cityRepository.GetByName(ma08uf.ma08iduf, CityName);

                if (ma09city.FK0908iduf != ma08uf.ma08iduf)
                {
                    return BadRequest("Não foi possível consultar a lista de passeios.");
                }

                IPagedList<ma05trip> trips = _tripRepository.GetList(0, ma09city.ma09idcity, page);
                PaginationEntity<ma05trip> result = new PaginationEntity<ma05trip>()
                {
                    Items = trips,
                    MetaData = new PaginationMetaData()
                    {
                        FirstItemOnPage = trips.GetMetaData().FirstItemOnPage,
                        HasNextPage = trips.GetMetaData().HasNextPage,
                        HasPreviousPage = trips.GetMetaData().HasPreviousPage,
                        IsFirstPage = trips.GetMetaData().IsFirstPage,
                        TotalItemCount = trips.GetMetaData().TotalItemCount,
                        IsLastPage = trips.GetMetaData().IsLastPage,
                        LastItemOnPage = trips.GetMetaData().LastItemOnPage,
                        PageCount = trips.GetMetaData().PageCount,
                        PageNumber = trips.GetMetaData().PageNumber,
                        PageSize = trips.GetMetaData().PageSize
                    }
                };
                TripListView tripListView = new TripListView() { lst = result, ma09city = ma09city, ma08uf = ma08uf };

                return Ok(tripListView);

            }
            catch (Exception e)
            {
                return BadRequest("Não foi possível consultar a lista de passeios: "+ e.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("GetTrip/{TripId}")]
        public IActionResult GetTrip(int TripId)
        {
            try
            {
                ma05trip ma05trip = _tripRepository.GetTrip(TripId);
                if (ma05trip == null)
                {
                    return BadRequest("Objeto Nulo!!");
                }else if (ma05trip.ma05status == "0")
                {
                    return BadRequest("Objeto não existe!!");
                }
                return Ok(ma05trip);
            }
            catch (Exception)
            {
                return BadRequest("Não foi possível consultar o passeio.");
            }
        }
        [Authorize]
        [HttpPost("RegisterTrip")]
        public async Task<IActionResult> RegisterTrip([FromBody] TripModel tripModel)
        {
            ma01user ma01user = await _userManager.GetUserAsync(HttpContext.User);
            if (ma01user.ma01type != "guide" && ma01user.ma01type != "admin")
            {
                return Unauthorized();
            }
            /*Controller de registro para passeios. Aqui é preciso ter as chaves estrangeiras para serem atribuídas ao passeio
             *Chaves estrangeiras de categoria, cidade e o guia.
             */
            if (ModelState.IsValid)
            {
                try
                {
                    ma08uf uf = _ufRepository.GetByInitials(tripModel.tripUF);
                    if (uf == null)
                    {
                        throw new Exception("Não foi possível encontrar a UF para o passeio...");
                    }
                    ma09city city = _cityRepository.GetByName(uf.ma08iduf, tripModel.tripCityName);
                    if (city == null)
                    {
                        throw new Exception("Não foi possível encontrar a cidade para o passeio...");
                    }

                    tripModel.ma05trip.FK0509idcity = city.ma09idcity;
                    _tripRepository.Add(tripModel.ma05trip);

                    foreach (ma27AgeDiscount ageDiscount in tripModel.ma27AgeDiscountList)
                    {
                        ageDiscount.FK2705idTrip = tripModel.ma05trip.ma05idtrip;
                    }
                    _ma27AgeDiscountRepository.AddList(tripModel.ma27AgeDiscountList);

                    foreach (SubtripModel subtripModel in tripModel.SubtripModelList)
                    {
                        subtripModel.ma14subtrip.FK1405idtrip = tripModel.ma05trip.ma05idtrip;
                        ma25partner partner = _ma25partnerRepository.GetByName(subtripModel.PartnerName);
                        if (partner == null)
                        {
                            throw new Exception("Não foi possível encontrar um dos parceiros...");
                        }

                        subtripModel.ma14subtrip.FK1425idpartner = partner.ma25idpartner;
                        _subTripRepository.Add(subtripModel.ma14subtrip);

                        if (subtripModel.ma11serviceList != null)
                        {
                            subtripModel.ma11serviceList.ForEach(e => e.FK1114idsubtrip = subtripModel.ma14subtrip.ma14idsubtrip);
                            _serviceRepository.AddList(subtripModel.ma11serviceList);
                        }
                        subtripModel.ma16subtripscheduleList.ForEach(e => e.FK1614idsubtrip = subtripModel.ma14subtrip.ma14idsubtrip);
                        _ma16SubTripScheduleRepository.AddList(subtripModel.ma16subtripscheduleList);

                        subtripModel.ma17SubtripValueList.ForEach(e => e.FK1714idsubtrip = subtripModel.ma14subtrip.ma14idsubtrip);
                        _subTripValueRepository.AddList(subtripModel.ma17SubtripValueList);
                    }
                }
                catch (Exception e)
                {
                    _tripRepository.Dispose();
                    return BadRequest(e.Message);
                }
                
            }
            else
            {
                return BadRequest("Dados inseridos incorretamente.");
            }
            return Ok();
        }

        [Authorize]
        [HttpPost("UpdateTrip")]
        public async Task<IActionResult> UpdateTrip([FromBody] TripModel tripModel)
        {
            ma01user ma01user = await _userManager.GetUserAsync(HttpContext.User);
            //TODO Editar esta controller
            if (ModelState.IsValid && (ma01user.ma01type == "guide" || ma01user.ma01type == "admin"))
            {
                try
                {
                    //Procura a cidade. Se não for encontrada, lança uma excessão
                    ma08uf uf = _ufRepository.GetByInitials(tripModel.tripUF);
                    ma09city city = _cityRepository.GetByName(uf.ma08iduf, tripModel.tripCityName);
                    if(city == null)
                    {
                        throw new Exception("Não foi possível encontrar a cidade...");
                    }
                    tripModel.ma05trip.FK0509idcity = city.ma09idcity;

                    _tripRepository.Update(tripModel.ma05trip);
                    if(tripModel.ma27AgeDiscountList != null)
                    {
                        tripModel.ma27AgeDiscountList.ForEach(e =>
                        {
                            if (e.ma27idAgeDiscount != 0)
                            {
                                _ma27AgeDiscountRepository.Update(e);
                            }
                            else
                            {
                                e.FK2705idTrip = tripModel.ma05trip.ma05idtrip;
                                _ma27AgeDiscountRepository.Add(e);
                            }
                        });
                    }
                    if(tripModel.SubtripModelList != null)
                    {
                        tripModel.SubtripModelList.ForEach(e =>
                        {
                            ma25partner partner = _ma25partnerRepository.GetByName(e.PartnerName);
                            if (partner == null)
                            {
                                throw new Exception("Não foi possível encontrar uma das empresas...");
                            }
                            e.ma14subtrip.FK1425idpartner = partner.ma25idpartner;
                            if (e.ma14subtrip.ma14idsubtrip != 0)
                            {
                                _subTripRepository.Update(e.ma14subtrip);
                            }
                            else
                            {
                                e.ma14subtrip.FK1405idtrip = tripModel.ma05trip.ma05idtrip;
                                _subTripRepository.Add(e.ma14subtrip);
                            }
                            if (e.ma11serviceList != null)
                            {
                                e.ma11serviceList.ForEach(w =>
                                {
                                    if (w.ma11idservice != 0)
                                    {
                                        _serviceRepository.Update(w);
                                    }
                                    else
                                    {
                                        w.FK1114idsubtrip = e.ma14subtrip.ma14idsubtrip;
                                        _serviceRepository.Add(w);
                                    }
                                });
                            }
                            if (e.ma16subtripscheduleList != null)
                            {
                                e.ma16subtripscheduleList.ForEach(w =>
                                {
                                    if (w.ma16idsubtripschedule != 0)
                                    {
                                        _ma16SubTripScheduleRepository.Update(w);
                                    }
                                    else
                                    {
                                        w.FK1614idsubtrip = e.ma14subtrip.ma14idsubtrip;
                                        _ma16SubTripScheduleRepository.Add(w);
                                    }
                                });
                            }
                            if (e.ma17SubtripValueList != null)
                            {
                                e.ma17SubtripValueList.ForEach(w =>
                                {
                                    if (w.ma17idSubtripValue != 0)
                                    {
                                        _subTripValueRepository.Update(w);
                                    }
                                    else
                                    {
                                        w.FK1714idsubtrip = e.ma14subtrip.ma14idsubtrip;
                                        _subTripValueRepository.Add(w);
                                    }
                                });
                            }

                        });
                    }
                    
                }
                catch (Exception e)
                {
                    _tripRepository.Dispose();
                    return BadRequest(e.Message);
                }
            }
            else
            {
                return Unauthorized();
            }
            return Ok();
        }

        [Authorize]
        [HttpGet("GetTripModel")]
        public async Task<IActionResult> GetTripModel(string TripName)
        {
            ma01user ma01user = await _userManager.GetUserAsync(HttpContext.User);
            if (ma01user.ma01type != "admin")
            {
                return Unauthorized();
            }
            ma05trip ma05Trip = _tripRepository.GetTrip(TripName);
            if(ma05Trip == null)
            {
                return BadRequest("Não foi possível encontrar este passeio!");
            }

            TripModel tripModel = new TripModel() {
                ma05trip = ma05Trip,
                tripCityName = ma05Trip.ma09city.ma09name,
                tripUF = ma05Trip.ma09city.ma08uf.ma08UFInitials,
                ma27AgeDiscountList = ma05Trip.ma27AgeDiscount.ToList(),
                SubtripModelList = new List<SubtripModel>()
            };
            foreach (ma14subtrip subtrip in ma05Trip.ma14subtrip.ToList())
            {
                SubtripModel SubtripModel = new SubtripModel() { 
                    ma14subtrip = subtrip,
                    ma16subtripscheduleList = subtrip.ma16subtripschedule.ToList(),
                    ma17SubtripValueList = subtrip.ma17SubtripValue.ToList(),
                    PartnerName = subtrip.ma25partner.ma25name
                };
                if(subtrip.ma11service != null)
                {
                    SubtripModel.ma11serviceList = subtrip.ma11service.ToList();
                }
                tripModel.SubtripModelList.Add(SubtripModel);
            }
            return Ok(tripModel);
        }


        [HttpGet("SearchTrips")]
        public IActionResult SearchTrips(string TripNameText)
        {
            List<ma05trip> triplist = _tripRepository.GetSearchTrip(TripNameText);
            List<string> triplistName = null;
            if(triplist != null)
            {
                triplistName = new List<string>();
                foreach (ma05trip trip in triplist)
                {
                    triplistName.Add(trip.ma05name);
                }
            }
            return Ok(triplistName);
        }
    }
}
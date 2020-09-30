using Matrip.Domain.Models.Entities;
using Matrip.Domain.Models.Payment;
using Matrip.Web.Database;
using Matrip.Domain.Models.TripPurchase;
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
using Matrip.Domain.Models.SaleModels;
using Matrip.Web.Libraries.Pagination;
using Matrip.Domain.Libraries.Text;
using Matrip.Domain.Libraries.Email;

namespace Matrip.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private UserManager<ma01user> _userManager;
        private ApplicationDbContext _dbContext;
        private Ima16SubTripScheduleRepository _ma16subTripScheduleRepository;
        private Ima18TripItemShoppingCartRepository _ma18tripItemShoppingCartRepository;
        private Ima29TouristShoppingCartRepository _ma29touristShoppingCartRepository;
        private Ima19SubTripItemShoppingCartRepository _ma19subTripItemShoppingCartRepository;
        private Ima20ServiceItemShoppingCartRepository _ma20serviceItemShoppingCartRepository;
        private Ima21SaleTripRepository _ma21saleTripRepository;
        private Ima22SubTripSaleRepository _ma22subTripSaleRepository;
        private Ima23ServiceSaleRepository _ma23serviceSaleRepository;
        private Ima28SaleTouristRepository _ma32saleTouristRepository;
        private Ima32saleRepository _ma32saleRepository;
        private Ima24PaymentRepository _ma24paymentRepository;
        private Ima36SubtripGroupRepository _ma36SubtripGroupRepository;
        private Ima34TransferencePendenciesRepository _transferencePendenciesRepository;
        private Ima33UserAddressRepository _ma33UserAddressRepository;
        private EmailManagement _emailManagement;
        private DateTime dateTimeNow = DateConvert.HrBrasilia();

        public SaleController(ApplicationDbContext context, Ima33UserAddressRepository userAddressRepository,
            Ima36SubtripGroupRepository subtripGroupRepository, Ima32saleRepository saleRepository, UserManager<ma01user> userManager, Ima22SubTripSaleRepository subTripSaleRepository,
            Ima28SaleTouristRepository saleTouristRepository, Ima23ServiceSaleRepository serviceSaleRepository, Ima24PaymentRepository paymentRepository,
            Ima18TripItemShoppingCartRepository tripItemShoppingCartRepository, Ima19SubTripItemShoppingCartRepository subTripItemShoppingCartRepository,
            Ima20ServiceItemShoppingCartRepository serviceItemShoppingCartRepository, Ima29TouristShoppingCartRepository touristShoppingCartRepository,
            Ima16SubTripScheduleRepository subTripScheduleRepository, Ima21SaleTripRepository saleTripRepository,
            Ima34TransferencePendenciesRepository transferencePendenciesRepository, EmailManagement emailManagement)
        {
            _userManager = userManager;
            _dbContext = context;
            _ma33UserAddressRepository = userAddressRepository;
            _ma36SubtripGroupRepository = subtripGroupRepository;
            _ma16subTripScheduleRepository = subTripScheduleRepository;
            _ma18tripItemShoppingCartRepository = tripItemShoppingCartRepository;
            _ma29touristShoppingCartRepository = touristShoppingCartRepository;
            _ma19subTripItemShoppingCartRepository = subTripItemShoppingCartRepository;
            _ma20serviceItemShoppingCartRepository = serviceItemShoppingCartRepository;
            _ma21saleTripRepository = saleTripRepository;
            _ma32saleRepository = saleRepository;
            _ma22subTripSaleRepository = subTripSaleRepository;
            _ma23serviceSaleRepository = serviceSaleRepository;
            _ma24paymentRepository = paymentRepository;

            _ma32saleTouristRepository = saleTouristRepository;
            _transferencePendenciesRepository = transferencePendenciesRepository;

            _emailManagement = emailManagement;
        }
        [Authorize]
        [HttpPost("SubmitSale")]
        public async Task<IActionResult> SubmitSale([FromBody]PaymentModel Payment)
        {
            try
            {
                ma38InfluencerDiscount influencerDiscount = null;
                if (Payment.influencerDiscountCode != "")
                {
                    influencerDiscount = _dbContext.ma38InfluencerDiscount
                    .Where(e => e.ma38DiscountCode == Payment.influencerDiscountCode && e.ma38InitialDate >= dateTimeNow && e.ma38FinalDate <= dateTimeNow)
                    .FirstOrDefault();
                }

                ma01user User = await _userManager.GetUserAsync(HttpContext.User);
                _ma33UserAddressRepository.AddOrUpdate(User.Id, Payment.userAddress);
                List<ma18tripitemshoppingcart> tripitemshoppingcartList = _ma18tripItemShoppingCartRepository.GetTripItemShoppingCartList(User.Id);
                List<ma24payment> paymentList = new List<ma24payment>();
                ma32sale sale = new ma32sale()
                {
                    FK3201iduser = User.Id,
                    ma32SaleDate = dateTimeNow
                };
                int paymentStatus = 1;
                foreach (ma24payment payment in Payment.Payment)
                {
                    if (payment.ma24paymentStatus == 3 && payment.ma24paymentValue > 0)
                    {
                        paymentStatus = payment.ma24paymentStatus;
                        ma34TransferencePendencies transferencePendencies = new ma34TransferencePendencies()
                        {
                            ma34idUser = User.Id,
                            ma34TransferenceValue = payment.ma24paymentValue
                        };
                        paymentList.Add(payment);
                        _transferencePendenciesRepository.Add(transferencePendencies);
                    }
                    else if (payment.ma24paymentStatus == 1)
                    {
                        paymentList.Add(payment);
                    }
                }
                sale.ma32situation = paymentStatus;
                _ma32saleRepository.Add(sale);
                foreach (ma24payment payment in paymentList)
                {
                    payment.FK2432idSale = sale.ma32idSale;
                }

                foreach (ma18tripitemshoppingcart tripitemshoppingcart in tripitemshoppingcartList)
                {

                    ma21saleTrip saleTrip = new ma21saleTrip()
                    {
                        FK2132idSale = sale.ma32idSale,
                        FK2105idtrip = tripitemshoppingcart.KF1805idtrip,
                        ma21date = dateTimeNow,
                        ma21hour = dateTimeNow.TimeOfDay
                    };
                    _ma21saleTripRepository.Add(saleTrip);
                    foreach (ma19SubTripItemShoppingCart subTripItemShoppingCart in tripitemshoppingcart.ma19SubTripItemShoppingCart)
                    {
                        double value = subTripItemShoppingCart.ma17SubtripValue.ma17value;
                        float subtripPartnerDiscount = 0;
                        float subtripInfluencerDiscount = 0;
                        float totalDiscount = 0;
                        if (influencerDiscount != null && subTripItemShoppingCart.ma14subtrip.ma14InfluencerDiscount)
                        {
                            subtripInfluencerDiscount += influencerDiscount.ma38DiscountPercent;
                            totalDiscount += subtripInfluencerDiscount;
                        }
                        if (subTripItemShoppingCart.ma14subtrip.ma14PartnerDiscountPercent > 0
                            && dateTimeNow >= subTripItemShoppingCart.ma14subtrip.ma14InitialDiscountDate
                            && dateTimeNow <= subTripItemShoppingCart.ma14subtrip.ma14FinalDiscountDate)
                        {
                            subtripPartnerDiscount += subTripItemShoppingCart.ma14subtrip.ma14PartnerDiscountPercent;
                            totalDiscount += subtripPartnerDiscount;
                        }
                        value = CalculateValueWithDiscount(value, totalDiscount);
                        double subtripTotal = 0;
                        double originalSubtripTotal = 0;
                        if (subTripItemShoppingCart.ma17SubtripValue.ma17type == "0")
                        {
                            foreach (ma29TouristShoppingCart touristShoppingCart in tripitemshoppingcart.ma29TouristShoppingCart)
                            {
                                subtripTotal += CalculateTouristValue(value, touristShoppingCart.ma27AgeDiscount.ma27DiscountPercent);
                                originalSubtripTotal += CalculateTouristValue(subTripItemShoppingCart.ma17SubtripValue.ma17value,
                                    touristShoppingCart.ma27AgeDiscount.ma27DiscountPercent);
                            }
                        }
                        else
                        {
                            subtripTotal += value;
                            originalSubtripTotal += subTripItemShoppingCart.ma17SubtripValue.ma17value;
                        }
                        ma22subtripsale subtripsale = new ma22subtripsale()
                        {
                            FK2214idSubTrip = subTripItemShoppingCart.FK1914idsubtrip,
                            FK2221idSaleTrip = saleTrip.ma21idSaleTrip,
                            ma22SaleDate = dateTimeNow,
                            ma22Date = Payment.choosedSubtripSaleDateList.
                                Where(e => e.SubtripItemShoppingCartID == subTripItemShoppingCart.ma19idSubTripItemShoppingCart).FirstOrDefault().ChoosedDate,
                            ma22Entry = Payment.choosedSubtripSaleDateList.
                                Where(e => e.SubtripItemShoppingCartID == subTripItemShoppingCart.ma19idSubTripItemShoppingCart).FirstOrDefault().Entry,
                            //Necessário para registrar e bloquear compras que excedam o número limite!!
                            ma22PeopleQuantity = subTripItemShoppingCart.ma18tripitemshoppingcart.ma29TouristShoppingCart.Count(),
                            ma22saleValue = subtripTotal,
                            ma22subtripPartnerlDiscount = subtripPartnerDiscount,
                            ma22subtripInfluencerDiscount = subtripInfluencerDiscount,
                            ma22originalSubtripValue = originalSubtripTotal,
                            FK2217idSubTripValue = subTripItemShoppingCart.FK1917idSubtripValue,
                        };

                        //subtripsale.FK2236idSubtripGroup = GroupAllocate(subTripItemShoppingCart.ma14subtrip.ma16subtripschedule.Where(e => e.ma16entry == subtripsale.ma22Entry).FirstOrDefault().ma16idsubtripschedule,
                           // subtripsale.ma22Date, subtripsale.ma22PeopleQuantity, subtripsale.FK2217idSubTripValue, subTripItemShoppingCart.ma17SubtripValue.ma17type);
                        _ma22subTripSaleRepository.Add(subtripsale);
                        foreach (ma20ServiceItemShoppingCart serviceItemShoppingCart in subTripItemShoppingCart.ma20ServiceItemShoppingCart)
                        {
                            ma23servicesale servicesale = new ma23servicesale()
                            {
                                FK2311idService = serviceItemShoppingCart.FK2011idService,
                                FK2322idSubTripSale = subtripsale.ma22idSubTripSale,
                                ma23UnitValue = serviceItemShoppingCart.ma11service.ma11Value,
                                ma23TotalValue = serviceItemShoppingCart.ma11service.ma11Value * serviceItemShoppingCart.ma20ServiceQuantity,
                                ma23ServiceQuantity = serviceItemShoppingCart.ma20ServiceQuantity
                            };
                            _ma23serviceSaleRepository.Add(servicesale);
                        }
                    }
                    foreach (ma29TouristShoppingCart touristShoppingCart in tripitemshoppingcart.ma29TouristShoppingCart)
                    {
                        ma28SaleTourist saleTourist = new ma28SaleTourist()
                        {
                            FK2821idSaleTrip = saleTrip.ma21idSaleTrip,
                            FK2827idAgeDiscount = touristShoppingCart.FK2927idAgeDiscount,
                            ma28Name = touristShoppingCart.ma29Name,
                            ma28PassportOrRG = touristShoppingCart.ma29PassportOrRG,
                            ma28Age = touristShoppingCart.ma29Age
                        };
                        _ma32saleTouristRepository.Add(saleTourist);
                    }
                }
                RemoveShoppingCartList(tripitemshoppingcartList);
                _ma24paymentRepository.AddList(paymentList);

                if (sale.ma32situation == 3)
                {
                    _emailManagement.SendTransferenceInformationEmail(User);
                }
                //await _dbContext.SaveChangesAsync();
                //return Ok(sale.ma32idSale);]
                return Ok();
            }
            catch (Exception e)
            {
                _dbContext.Dispose();
                return BadRequest(e.Message);
            }
        }
        [Authorize]
        [HttpPost("SubmitTripItemSale")]
        public async Task<IActionResult> SubmitTripItemSale([FromBody]PaymentTripItemModel Payment)
        {
            try
            {
                ma38InfluencerDiscount influencerDiscount = null;
                if (string.IsNullOrEmpty(Payment.influencerDiscountCode))
                {
                    var query = _dbContext.ma38InfluencerDiscount
                    .Where(e => e.ma38DiscountCode == Payment.influencerDiscountCode && e.ma38InitialDate <= dateTimeNow && e.ma38FinalDate >= dateTimeNow);
                    if (query.Any())
                    {
                        influencerDiscount = query.FirstOrDefault();
                    }
                }
                ma01user User = await _userManager.GetUserAsync(HttpContext.User);
                _ma33UserAddressRepository.AddOrUpdate(User.Id, Payment.userAddress);
                List<ma24payment> paymentList = new List<ma24payment>();
                ma32sale sale = new ma32sale()
                {
                    FK3201iduser = User.Id,
                    ma32SaleDate = dateTimeNow
                };
                int paymentStatus = 1;
                foreach (ma24payment payment in Payment.Payment)
                {
                    if (payment.ma24paymentStatus == 3 && payment.ma24paymentValue > 0)
                    {
                        paymentStatus = payment.ma24paymentStatus;
                        ma34TransferencePendencies transferencePendencies = new ma34TransferencePendencies()
                        {
                            ma34idUser = User.Id,
                            ma34TransferenceValue = payment.ma24paymentValue
                        };
                        paymentList.Add(payment);
                        _transferencePendenciesRepository.Add(transferencePendencies);
                    }
                    else if (payment.ma24paymentStatus == 1)
                    {
                        paymentList.Add(payment);
                    }
                }
                sale.ma32situation = paymentStatus;
                _ma32saleRepository.Add(sale);
                foreach (ma24payment payment in paymentList)
                {
                    payment.FK2432idSale = sale.ma32idSale;
                }

                ma21saleTrip saleTrip = new ma21saleTrip()
                {
                    FK2132idSale = sale.ma32idSale,
                    FK2105idtrip = Payment.trip.ma05idtrip,
                    ma21date = dateTimeNow,
                    ma21hour = dateTimeNow.TimeOfDay
                };
                _ma21saleTripRepository.Add(saleTrip);

                foreach (ma14subtrip ma14subtrip in Payment.trip.ma14subtrip)
                {
                    ma22subtripsale subtripsale = new ma22subtripsale()
                    {
                        FK2214idSubTrip = ma14subtrip.ma14idsubtrip,
                        FK2221idSaleTrip = saleTrip.ma21idSaleTrip,
                        ma22SaleDate = dateTimeNow,
                        ma22Date = Payment.choosedSubtripSaleDateList.
                            Where(e => e.SubtripItemShoppingCartID == ma14subtrip.ma14idsubtrip).FirstOrDefault().ChoosedDate,
                        ma22Entry = Payment.choosedSubtripSaleDateList.
                            Where(e => e.SubtripItemShoppingCartID == ma14subtrip.ma14idsubtrip).FirstOrDefault().Entry,
                        //Necessário para registrar e bloquear compras que excedam o número limite!!
                        ma22PeopleQuantity = Payment.TouristList.Count()
                    };
                    double totalSubtripValue = 0;

                    int subtripID = int.Parse(Payment.TripItem.SubtripStatus.Split("/")[1]);
                    string type = "0";
                    if (ma14subtrip.ma14idsubtrip == subtripID)
                    {
                        ma17SubtripValue subtripValue = ma14subtrip.ma17SubtripValue
                                            .Where(e => e.ma17idSubtripValue == int.Parse(Payment.TripItem.SubtripStatus.Split("/")[2])).FirstOrDefault();
                        subtripsale.FK2217idSubTripValue = subtripValue.ma17idSubtripValue;
                        type = subtripValue.ma17type;

                        double value = subtripValue.ma17value;

                        float subtripPartnerDiscount = 0;
                        float subtripInfluencerDiscount = 0;
                        float subtripTotalDiscount = 0;
                        //adiciona desconto por influencer, se tiver
                        if (influencerDiscount != null && ma14subtrip.ma14InfluencerDiscount)
                        {
                            subtripInfluencerDiscount += influencerDiscount.ma38DiscountPercent;
                            subtripTotalDiscount += subtripInfluencerDiscount;
                        }
                        //adiciona desconto da parceria, caso esteja válido
                        if (ma14subtrip.ma14PartnerDiscountPercent > 0
                            && dateTimeNow >= ma14subtrip.ma14InitialDiscountDate
                            && dateTimeNow <= ma14subtrip.ma14FinalDiscountDate)
                        {
                            subtripPartnerDiscount += ma14subtrip.ma14PartnerDiscountPercent;
                            subtripTotalDiscount += subtripPartnerDiscount;
                        }
                        value = CalculateValueWithDiscount(value, subtripTotalDiscount);
                        subtripsale.ma22subtripPartnerlDiscount = subtripPartnerDiscount;
                        subtripsale.ma22subtripInfluencerDiscount = subtripInfluencerDiscount;
                        subtripsale.ma22originalSubtripValue = 0;
                        if (subtripValue.ma17type == "0")
                        {
                            foreach (TouristModel TouristModel in Payment.TouristList)
                            {
                                totalSubtripValue += CalculateTouristValue(
                                    value,
                                    Payment.trip.ma27AgeDiscount
                                    .Where(e => e.ma27idAgeDiscount == TouristModel.AgeDiscountID).FirstOrDefault().ma27DiscountPercent
                                    );
                                subtripsale.ma22originalSubtripValue += CalculateTouristValue(subtripValue.ma17value,
                                    Payment.trip.ma27AgeDiscount
                                    .Where(e => e.ma27idAgeDiscount == TouristModel.AgeDiscountID).FirstOrDefault().ma27DiscountPercent);
                            }
                        }
                        else
                        {
                            totalSubtripValue += value;
                            subtripsale.ma22originalSubtripValue += subtripValue.ma17value;
                        }
                    }
                    subtripsale.ma22saleValue = totalSubtripValue;


                    //subtripsale.FK2236idSubtripGroup = GroupAllocate(ma14subtrip.ma16subtripschedule.Where(e => e.ma16entry == subtripsale.ma22Entry).FirstOrDefault().ma16idsubtripschedule,
                        //subtripsale.ma22Date, subtripsale.ma22PeopleQuantity, subtripsale.FK2217idSubTripValue, type);

                    _ma22subTripSaleRepository.Add(subtripsale);
                    foreach (ma11service ma11service in ma14subtrip.ma11service)
                    {
                        ma23servicesale servicesale = new ma23servicesale()
                        {
                            FK2311idService = ma11service.ma11idservice,
                            FK2322idSubTripSale = subtripsale.ma22idSubTripSale,
                            ma23UnitValue = ma11service.ma11Value,
                            ma23TotalValue = ma11service.ma11Value * Payment.TripItem.Services
                                                                    .Where(e => e.ServiceID == ma11service.ma11idservice).FirstOrDefault().Quantity,
                            ma23ServiceQuantity = Payment.TripItem.Services
                                                                    .Where(e => e.ServiceID == ma11service.ma11idservice).FirstOrDefault().Quantity
                        };
                        _ma23serviceSaleRepository.Add(servicesale);
                    }
                }
                foreach (TouristModel TouristModel in Payment.TouristList)
                {
                    int discountPercent = Payment.trip.ma27AgeDiscount
                        .Where(e => e.ma27idAgeDiscount == TouristModel.AgeDiscountID).FirstOrDefault().ma27DiscountPercent;
                    ma28SaleTourist saleTourist = new ma28SaleTourist()
                    {
                        FK2821idSaleTrip = saleTrip.ma21idSaleTrip,
                        FK2827idAgeDiscount = TouristModel.AgeDiscountID,
                        ma28Name = TouristModel.ma28Name,
                        ma28PassportOrRG = TouristModel.ma28PassportOrRG,
                        ma28Age = TouristModel.Age
                    };
                    _ma32saleTouristRepository.Add(saleTourist);
                }
                _ma24paymentRepository.AddList(paymentList);
                if (sale.ma32situation == 3)
                {
                    _emailManagement.SendTransferenceInformationEmail(User);
                }

                //await _dbContext.SaveChangesAsync();
                //return Ok(sale.ma32idSale);
                return Ok();
            }
            catch(Exception e)
            {
                _dbContext.Dispose();
                return BadRequest(e.Message);
            }
            
        }



        [HttpGet("GetSaleList")]
        [Authorize]
        public async Task<IActionResult> GetSaleList([FromQuery] int? page)
        {
            try
            {
                ma01user user = await _userManager.GetUserAsync(HttpContext.User);
                IPagedList<ma32sale> SaleList = _ma32saleRepository.GetSaleList(user.Id, page);
                //var result = new { items = SaleList, metadata = SaleList.GetMetaData() };
                
                PaginationEntity<ma32sale> result = new PaginationEntity<ma32sale>()
                {
                    Items = SaleList,
                    MetaData = new PaginationMetaData()
                    {
                        FirstItemOnPage = SaleList.GetMetaData().FirstItemOnPage,
                        HasNextPage = SaleList.GetMetaData().HasNextPage,
                        HasPreviousPage = SaleList.GetMetaData().HasPreviousPage,
                        IsFirstPage = SaleList.GetMetaData().IsFirstPage,
                        TotalItemCount = SaleList.GetMetaData().TotalItemCount,
                        IsLastPage = SaleList.GetMetaData().IsLastPage,
                        LastItemOnPage = SaleList.GetMetaData().LastItemOnPage,
                        PageCount = SaleList.GetMetaData().PageCount,
                        PageNumber = SaleList.GetMetaData().PageNumber,
                        PageSize = SaleList.GetMetaData().PageSize
                    }
                };
                return Ok(result);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet("GetSale/{SaleID}")]
        [Authorize]
        public async Task<IActionResult> GetSale(int SaleID)
        {
            ma32sale sale = _ma32saleRepository.GetSale(SaleID);
            ma01user user = await _userManager.GetUserAsync(HttpContext.User);
            if (sale.FK3201iduser == user.Id)
            {
                ma33UserAddress userAddress = _ma33UserAddressRepository.GetByUserID(user.Id);
                SaleViewModel saleView = new SaleViewModel { sale = sale, userAddress = userAddress };
                return Ok(saleView);
            }
            else
            {
                return BadRequest("Acesso não autorizado!");
            }
        }

        [HttpGet("GetLastSale")]
        [Authorize]
        public async Task<IActionResult> GetLastSale()
        {
            ma32sale sale = _ma32saleRepository.GetLastSale();
            ma01user user = await _userManager.GetUserAsync(HttpContext.User);
            if (sale.FK3201iduser == user.Id)
            {
                ma33UserAddress userAddress = _ma33UserAddressRepository.GetByUserID(user.Id);
                SaleViewModel saleView = new SaleViewModel { sale = sale, userAddress = userAddress };
                return Ok(saleView);
            }
            else
            {
                return BadRequest("Acesso não autorizado!");
            }
        }

        [HttpGet("GetContract/{SaleID}")]
        [Authorize]
        public async Task<IActionResult> GetContract(int SaleID)
        {
            ma32sale sale = _ma32saleRepository.GetSale(SaleID);
            ma01user user = await _userManager.GetUserAsync(HttpContext.User);
            if (sale.FK3201iduser == user.Id)
            {
                ma33UserAddress userAddress = _ma33UserAddressRepository.GetByUserID(user.Id);
                SaleViewModel saleView = new SaleViewModel { sale = sale, userAddress = userAddress };

                ContractViewModel contract = new ContractViewModel { ma01user = user, SaleModel = saleView };
                return Ok(contract);
            }
            else
            {
                return BadRequest("Acesso não autorizado!");
            }
        }

        [HttpGet("GetDiscount/{DiscountCode}")]
        [Authorize]
        public IActionResult GetDiscount(string DiscountCode)
        {
            var query = _dbContext.ma38InfluencerDiscount
                .Where(e => e.ma38DiscountCode == DiscountCode && e.ma38InitialDate <= dateTimeNow && e.ma38FinalDate >= dateTimeNow);
            if (query.Any())
            {
                return Ok(query.FirstOrDefault());
            }
            return Ok(null);
        }

        public void RemoveShoppingCartList(List<ma18tripitemshoppingcart> tripitemshoppingcartList)
        {
            List<ma29TouristShoppingCart> touristShoppingCartList = new List<ma29TouristShoppingCart>();
            List<ma19SubTripItemShoppingCart> subTripItemShoppingCartList = new List<ma19SubTripItemShoppingCart>();
            List<ma20ServiceItemShoppingCart> serviceItemShoppingCartList = new List<ma20ServiceItemShoppingCart>();
            foreach (ma18tripitemshoppingcart tripitemshoppingcart in tripitemshoppingcartList)
            {
                touristShoppingCartList.AddRange(tripitemshoppingcart.ma29TouristShoppingCart);
                foreach (ma19SubTripItemShoppingCart subTripItemShoppingCart in tripitemshoppingcart.ma19SubTripItemShoppingCart)
                {
                    subTripItemShoppingCartList.Add(subTripItemShoppingCart);
                    serviceItemShoppingCartList.AddRange(subTripItemShoppingCart.ma20ServiceItemShoppingCart);
                }
            }
            _ma29touristShoppingCartRepository.RemoveList(touristShoppingCartList);
            _ma20serviceItemShoppingCartRepository.RemoveList(serviceItemShoppingCartList);
            _ma19subTripItemShoppingCartRepository.RemoveList(subTripItemShoppingCartList);
            _ma18tripItemShoppingCartRepository.RemoveList(tripitemshoppingcartList);
        }

        [Authorize]
        [HttpGet("GetUnavailableDatesWithSubtripID")]
        public IActionResult GetUnavailableDatesWithSubtripID([FromQuery] int scheduleID, [FromQuery] DateTime Date, [FromQuery] int peopleQuantity, [FromQuery] int ValueID)
        {
            bool isAvailable = false;
            ma16subtripschedule ma16Subtripschedule = _ma16subTripScheduleRepository.GetById(scheduleID);

            List<ma22subtripsale> subtripSaleList = _ma22subTripSaleRepository.GetSubtripSaleList(ma16Subtripschedule, Date, ValueID);
            if (subtripSaleList == null)
            {
                return Ok(true);
            }
            ma17SubtripValue subtripValue = _dbContext.ma17SubtripValue.Where(e => e.ma17idSubtripValue == ValueID).FirstOrDefault();
            List<ma36SubtripGroup> subtripGroupList = _ma36SubtripGroupRepository.GetSubtripGroupList(ma16Subtripschedule, Date);
            if (subtripValue.ma17type == "0")
            {
                foreach (ma36SubtripGroup subtripGroup in subtripGroupList)
                {
                    if(subtripGroup.ma36status == "1")
                    {
                        /*
                        if (subtripSaleList.Where(e => e.ma36SubtripGroup.ma36idSubtripGroup == subtripGroup.ma36idSubtripGroup
                         && e.ma17SubtripValue.ma17type == "0").Any())
                        {
                            int AlreadyRegistered = subtripSaleList.Where(e => e.ma36SubtripGroup.ma36idSubtripGroup == subtripGroup.ma36idSubtripGroup
                                && e.ma17SubtripValue.ma17type == "0").Sum(e => e.ma22PeopleQuantity);
                            if (peopleQuantity <= (subtripSaleList.FirstOrDefault().ma14subtrip.ma14vacancy - AlreadyRegistered))
                            {
                                isAvailable = true;
                                break;
                            }
                        }
                        else if (!subtripSaleList.Where(e => e.ma36SubtripGroup.ma36idSubtripGroup == subtripGroup.ma36idSubtripGroup
                             && e.ma17SubtripValue.ma17type != "0").Any())
                        {
                            isAvailable = true;
                            break;
                        }*/
                    }
                }
            }
            else
            {
                foreach (ma36SubtripGroup subtripGroup in subtripGroupList)
                {
                    if (subtripGroup.ma36status == "1")
                    {
                        /*
                        if (!subtripSaleList.Where(e => e.ma36SubtripGroup.ma36idSubtripGroup == subtripGroup.ma36idSubtripGroup).Any())
                        {
                            isAvailable = true;
                            break;
                        }
                        */
                    }     
                }
            }
            isAvailable = true;
            return Ok(isAvailable);
        }

        private static double CalculateTouristValue(double value, int discountPercent)
        {
            var discount = value * (Convert.ToDouble(discountPercent) / 100);
            return value - discount;
        }

        private static double CalculateValueWithDiscount(double value, float discountPercent)
        {
            var discount = value * (Convert.ToDouble(discountPercent) / 100);
            return value - discount;
        }

        private int GroupAllocate(int scheduleID, DateTime Date, int peopleQuantity, int ValueID, string valueType)
        {
            ma16subtripschedule ma16Subtripschedule = _ma16subTripScheduleRepository.GetById(scheduleID);
            List<ma22subtripsale> subtripSaleList = _ma22subTripSaleRepository.GetSubtripSaleList(ma16Subtripschedule, Date, ValueID);
            List<ma36SubtripGroup> subtripGroupList = _ma36SubtripGroupRepository.GetSubtripGroupList(ma16Subtripschedule, Date);

            if (subtripSaleList == null)
            {
                return subtripGroupList.FirstOrDefault().ma36idSubtripGroup;
            }

            if (valueType == "0")
            {
                bool achou = true;
                foreach (ma36SubtripGroup subtripGroup in subtripGroupList)
                {
                    achou = true;
                    if(subtripGroup.ma36status == "1")
                    {
                        /*
                        if (subtripSaleList.Where(e => e.ma36SubtripGroup.ma36idSubtripGroup == subtripGroup.ma36idSubtripGroup
                        && e.ma17SubtripValue.ma17type == "0").Any())
                        {
                            int AlreadyRegistered = subtripSaleList.Where(e => e.ma36SubtripGroup.ma36idSubtripGroup == subtripGroup.ma36idSubtripGroup
                                && e.ma17SubtripValue.ma17type == "0").Sum(e => e.ma22PeopleQuantity);
                            if (peopleQuantity <= (subtripSaleList.FirstOrDefault().ma14subtrip.ma14vacancy - AlreadyRegistered))
                            {
                                achou = true;
                            }
                            else
                            {
                                achou = false;
                            }
                        }
                        else if (subtripSaleList.Where(e => e.ma36SubtripGroup.ma36idSubtripGroup == subtripGroup.ma36idSubtripGroup
                             && e.ma17SubtripValue.ma17type != "0").Any())
                        {
                            achou = false;
                        }
                        if (achou)
                        {
                            return subtripGroup.ma36idSubtripGroup;
                        }
                        */
                    }
                }
            }
            else
            {
                foreach (ma36SubtripGroup subtripGroup in subtripGroupList)
                {
                    if(subtripGroup.ma36status == "1")
                    {
                        /*
                        if (!subtripSaleList.Where(e => e.ma36SubtripGroup.ma36idSubtripGroup == subtripGroup.ma36idSubtripGroup).Any())
                        {
                            return subtripGroup.ma36idSubtripGroup;
                        }*/
                    }
                }
            }
            return 0;
        }
    }
}
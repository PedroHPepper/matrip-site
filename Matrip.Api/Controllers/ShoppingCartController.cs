using Matrip.Domain.Models.Entities;
using Matrip.Domain.Models.TripPurchase;
using Matrip.Web.Domain.Models;
using Matrip.Web.Repositories.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Matrip.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private UserManager<ma01user> _userManager;
        private Ima05TripRepository _tripRepository;
        private Ima04GuideRepository _guideRepository;
        private Ima18TripItemShoppingCartRepository _tripItemShoppingCartRepository;
        private Ima19SubTripItemShoppingCartRepository _subTripItemShoppingCartRepository;
        private Ima20ServiceItemShoppingCartRepository _serviceItemShoppingCartRepository;
        private Ima12SubtripGuideRepository _subtripGuideRepository;
        private Ima30GuideSubtripShoppingCartRepository _GuideSubtripShoppingCartRepository;
        private Ima29TouristShoppingCartRepository _touristShoppingCartRepository;
        public ShoppingCartController(UserManager<ma01user> userManager, Ima05TripRepository tripRepository, Ima04GuideRepository guideRepository,
            Ima18TripItemShoppingCartRepository tripItemShoppingCartRepository, Ima19SubTripItemShoppingCartRepository subTripItemShoppingCartRepository,
            Ima20ServiceItemShoppingCartRepository serviceItemShoppingCartRepository, Ima12SubtripGuideRepository subtripGuideRepository,
            Ima30GuideSubtripShoppingCartRepository GuideSubtripShoppingCartRepository, Ima29TouristShoppingCartRepository touristShoppingCartRepository)
        {
            _userManager = userManager;
            _tripRepository = tripRepository;
            _guideRepository = guideRepository;
            _tripItemShoppingCartRepository = tripItemShoppingCartRepository;
            _subTripItemShoppingCartRepository = subTripItemShoppingCartRepository;
            _serviceItemShoppingCartRepository = serviceItemShoppingCartRepository;
            _subtripGuideRepository = subtripGuideRepository;
            _GuideSubtripShoppingCartRepository = GuideSubtripShoppingCartRepository;
            _touristShoppingCartRepository = touristShoppingCartRepository;
        }


        [Authorize]
        [HttpGet("GetShoppingCart")]
        public async Task<IActionResult> GetShoppingCart()
        {
            //Resgatando Lista de itens de passeio que o cliente escolheu usando id do usuário
            ma01user ma01user = await _userManager.GetUserAsync(HttpContext.User);
            List<ma18tripitemshoppingcart> ma18TripitemshoppingcartList = _tripItemShoppingCartRepository.GetTripItemShoppingCartList(ma01user.Id);

            return Ok(ma18TripitemshoppingcartList);
        }
        [Authorize]
        [HttpGet("GetShoppingCartItem/{TripItemShoppingCartID}")]
        public async Task<IActionResult> GetShoppingCartItem(int TripItemShoppingCartID)
        {
            ma18tripitemshoppingcart ma18tripitemshoppingcart = _tripItemShoppingCartRepository.GetTripItemShoppingCart(TripItemShoppingCartID);
            ma01user ma01user = await _userManager.GetUserAsync(HttpContext.User);
            if (ma18tripitemshoppingcart.FK1801iduser == ma01user.Id)
            {
                return Ok(ma18tripitemshoppingcart);
            }
            return BadRequest();
        }

        [Authorize]
        [HttpPost("AddShoppingCartItem")]
        public async Task<IActionResult> AddTripInCart([FromBody]ChoosedTripPackage choosedTripPackage)
        {
            ma01user ma01user = await _userManager.GetUserAsync(HttpContext.User);

            ma05trip Trip = _tripRepository.GetTrip(choosedTripPackage.TripItem.TripID);
            ma14subtrip ma14subtrip = Trip.ma14subtrip.Where(e => e.ma14idsubtrip == choosedTripPackage.TripItem.SubTripID).FirstOrDefault();
            List<ma11service> ma11serviceList = new List<ma11service>();

            //Criando um objeto de carrinho para passeio e adicionando ao banco
            ma18tripitemshoppingcart tripitemshoppingcart = new ma18tripitemshoppingcart()
            {
                FK1801iduser = ma01user.Id,
                KF1805idtrip = Trip.ma05idtrip
            };
            _tripItemShoppingCartRepository.Add(tripitemshoppingcart);


            /*seleciona todos os serviços e quantidade de serviços. Além disso ele verifica se todos IDs estão de acordo com os subtrips escohidos,
             * excluindo os serviços de subtrips não escolhidas.
            */
            if(choosedTripPackage.TripItem.Services != null)
            {
                foreach (ServiceQuantity ServiceQuantity in choosedTripPackage.TripItem.Services)
                {
                    foreach (ma11service Service in ma14subtrip.ma11service)
                    {
                        if (ServiceQuantity.ServiceID == Service.ma11idservice)
                        {
                            ma11serviceList.Add(Service);
                        }
                    }
                }
            }
            
            //Percorre por cada valor de subpasseio

            //Cria um item de carrinho pra subpasseio e submete ao banco
            int subtripValueID = 0;
            for (int i = 0; i < choosedTripPackage.TripItem.SubtripStatus.Count(); i++)
            {
                int choosedSubtripID = int.Parse(choosedTripPackage.TripItem.SubtripStatus.Split("/")[1]);
                if (ma14subtrip.ma14idsubtrip == choosedSubtripID)
                {

                    subtripValueID = int.Parse(choosedTripPackage.TripItem.SubtripStatus.Split("/")[2]);
                    break;
                }
            }
            ma19SubTripItemShoppingCart SubTripItemShoppingCart = new ma19SubTripItemShoppingCart()
            {
                FK1918idTripItemShoppingCart = tripitemshoppingcart.ma18idtripitemshoppingcart,
                FK1914idsubtrip = ma14subtrip.ma14idsubtrip,
                FK1917idSubtripValue = subtripValueID
            };
            _subTripItemShoppingCartRepository.Add(SubTripItemShoppingCart);
            foreach (ma11service service in ma11serviceList)
            {
                if (service.FK1114idsubtrip == ma14subtrip.ma14idsubtrip)
                {
                    ma20ServiceItemShoppingCart serviceItemShoppingCart = new ma20ServiceItemShoppingCart()
                    {
                        FK2011idService = service.ma11idservice,
                        FK2019idSubTripItemShoppingCart = SubTripItemShoppingCart.ma19idSubTripItemShoppingCart,
                        ma20ServiceQuantity = choosedTripPackage.TripItem.Services.Where(e => e.ServiceID == service.ma11idservice).FirstOrDefault().Quantity
                    };
                    _serviceItemShoppingCartRepository.Add(serviceItemShoppingCart);
                }
            }

            List<ma29TouristShoppingCart> touristShoppingCartList = new List<ma29TouristShoppingCart>();
            foreach (TouristModel tourist in choosedTripPackage.TouristList)
            {
                ma29TouristShoppingCart touristShoppingCart = new ma29TouristShoppingCart()
                {
                    FK2918idTripItemShoppingCart = tripitemshoppingcart.ma18idtripitemshoppingcart,
                    FK2927idAgeDiscount = tourist.AgeDiscountID,
                    ma29Name = tourist.ma28Name,
                    ma29PassportOrRG = tourist.ma28PassportOrRG,
                    ma29Age = tourist.Age
                };
                touristShoppingCartList.Add(touristShoppingCart);
            }
            //submete cada turista separando-os por faixa etária
            _touristShoppingCartRepository.AddList(touristShoppingCartList);
            return Ok();
        }
        [Authorize]
        [HttpDelete("RemoveShoppingCartItem/{TripItemShoppingCartID}")]
        public async Task<IActionResult> RemoveShoppingCartItem(int TripItemShoppingCartID)
        {
            ma01user ma01user = await _userManager.GetUserAsync(HttpContext.User);
            try
            {
                ma18tripitemshoppingcart tripitemshoppingcart = _tripItemShoppingCartRepository.GetTripItemShoppingCart(TripItemShoppingCartID);
                if (tripitemshoppingcart.FK1801iduser == ma01user.Id)
                {
                    List<ma19SubTripItemShoppingCart> subTripItemShoppingCartList = new List<ma19SubTripItemShoppingCart>();
                    List<ma20ServiceItemShoppingCart> serviceItemShoppingCartList = new List<ma20ServiceItemShoppingCart>();

                    foreach (ma19SubTripItemShoppingCart ma19SubTripItemShoppingCart in tripitemshoppingcart.ma19SubTripItemShoppingCart)
                    {
                        subTripItemShoppingCartList.Add(ma19SubTripItemShoppingCart);
                        serviceItemShoppingCartList.AddRange(ma19SubTripItemShoppingCart.ma20ServiceItemShoppingCart);
                    }
                    _serviceItemShoppingCartRepository.RemoveList(serviceItemShoppingCartList);

                    _subTripItemShoppingCartRepository.RemoveList(subTripItemShoppingCartList);

                    _touristShoppingCartRepository.RemoveList(tripitemshoppingcart.ma29TouristShoppingCart.ToList());

                    _tripItemShoppingCartRepository.Remove(tripitemshoppingcart);
                    return Ok();
                }
                else
                {
                    throw new AccessViolationException("Acesso negado!");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
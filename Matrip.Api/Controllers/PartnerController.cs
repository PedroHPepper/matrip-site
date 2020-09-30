using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Matrip.Domain.Models.Entities;
using Matrip.Domain.Models.GuideModels;
using Matrip.Domain.Models.PartnerModels;
using Matrip.Web.Repositories.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Matrip.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PartnerController : ControllerBase
    {
        private UserManager<ma01user> _userManager;
        private Ima04GuideRepository _ma04GuideRepository;
        private Ima25PartnerRepository _ma25PartnerRepository;
        private Ima26PartnerGuideRepository _ma26PartnerGuideRepository;
        private Ima08UFRepository _ma08UFRepository;
        private Ima09CityRepository _ma09CityRepository;

        public PartnerController(UserManager<ma01user> userManager, Ima25PartnerRepository partnerRepository, Ima04GuideRepository
            guideRepository,
            Ima26PartnerGuideRepository partnerGuideRepository, Ima08UFRepository UFRepository, Ima09CityRepository cityRepository)
        {
            _userManager = userManager;
            _ma04GuideRepository = guideRepository;
            _ma25PartnerRepository = partnerRepository;
            _ma26PartnerGuideRepository = partnerGuideRepository;
            _ma08UFRepository = UFRepository;
            _ma09CityRepository = cityRepository;
        }

        [HttpGet("GetPartnerList")]
        public async Task<IActionResult> GetPartnerList()
        {
            ma01user user = await _userManager.GetUserAsync(HttpContext.User);
            if (user.ma01type != "admin")
            {
                return Unauthorized();
            }
            IEnumerable<ma25partner> partnerList = _ma25PartnerRepository.GetAll();
            return Ok(partnerList.ToList());
        }

        [HttpGet("GetPartnerWithGuides/{PartnerID}")]
        public async Task<IActionResult> GetPartnerWithGuides(int PartnerID)
        {
            ma01user user = await _userManager.GetUserAsync(HttpContext.User);
            if (user.ma01type != "admin")
            {
                return Unauthorized();
            }
            ma25partner partner = _ma25PartnerRepository.GetPartnerWithGuideList(PartnerID);
            return Ok(partner);
        }

        [HttpPost("AddPartner")]
        public async Task<IActionResult> AddPartner([FromBody] PartnerModel partner)
        {
            try
            {
                ma01user user = await _userManager.GetUserAsync(HttpContext.User);
                if (user.ma01type != "admin" && user.ma01type != "guide")
                {
                    return Unauthorized();
                }
                ma08uf uf = _ma08UFRepository.GetByInitials(partner.UF);
                ma09city city = _ma09CityRepository.GetByName(uf.ma08iduf, partner.CityName);
                partner.ma25partner.FK2509idcity = city.ma09idcity;
                _ma25PartnerRepository.Add(partner.ma25partner);
                if(user.ma01type == "guide")
                {
                    ma04guide guide = _ma04GuideRepository.GetGuideByUserId(user.Id);
                    ma26PartnerGuide partnerGuide = new ma26PartnerGuide()
                    {
                        FK2604idGuide = guide.ma04idguide,
                        FK2625idPartner = partner.ma25partner.ma25idpartner
                    };
                    _ma26PartnerGuideRepository.Add(partnerGuide);
                }
                return Ok();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("SearchPartners")]
        public IActionResult SearchPartners(string PartnerText)
        {
            try
            {
                List<ma25partner> partnerList = _ma25PartnerRepository.GetSearchPartner(PartnerText);
                return Ok(partnerList);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("UpdatePartner")]
        public async Task<IActionResult> UpdatePartner([FromBody] PartnerModel partner)
        {
            try
            {
                ma01user user = await _userManager.GetUserAsync(HttpContext.User);
                if (user.ma01type != "admin" && user.ma01type != "guide")
                {
                    return Unauthorized();
                }
                ma08uf uf = _ma08UFRepository.GetByInitials(partner.UF);
                if(uf == null)
                {
                    return BadRequest("Estado não encontrado!");
                }
                ma09city city = _ma09CityRepository.GetByName(uf.ma08iduf, partner.CityName);
                if (city == null)
                {
                    return BadRequest("Cidade não encontrado!");
                }

                partner.ma25partner.FK2509idcity = city.ma09idcity;
                ma25partner ma25partner = _ma25PartnerRepository.GetById(partner.ma25partner.ma25idpartner);

                ma25partner.ma25name = partner.ma25partner.ma25name;
                ma25partner.ma25address = partner.ma25partner.ma25address;
                ma25partner.ma25CNPJ = partner.ma25partner.ma25CNPJ;
                ma25partner.ma25companyName = partner.ma25partner.ma25companyName;
                ma25partner.ma25email = partner.ma25partner.ma25email;
                ma25partner.ma25homepage = partner.ma25partner.ma25homepage;
                ma25partner.ma25neighborhood = partner.ma25partner.ma25neighborhood;
                ma25partner.ma25status = partner.ma25partner.ma25status;
                ma25partner.FK2509idcity = city.ma09idcity;
                if (ma25partner != null)
                {
                    _ma25PartnerRepository.Update(ma25partner);
                }
                else
                {
                    return BadRequest("Parceiro não encontrado!");
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("CreateGuide")]
        public async Task<IActionResult> CreateGuide(GuideModel NewGuide)
        {
            ma01user user = await _userManager.GetUserAsync(HttpContext.User);
            if (user.ma01type != "admin")
            {
                return Unauthorized();
            }
            ma01user Guideuser = await _userManager.FindByEmailAsync(NewGuide.Email);
            if (Guideuser != null)
            {
                ma04guide guide = null;
                if (Guideuser.ma01type == "user")
                {
                    guide = new ma04guide()
                    {
                        ma04MEI = NewGuide.MEI,
                        FK0401iduser = Guideuser.Id
                    };
                    _ma04GuideRepository.Add(guide);
                    Guideuser.ma01type = "guide";

                    await _userManager.UpdateAsync(Guideuser);
                    return Ok();
                }
                else if (Guideuser.ma01type == "admin")
                {
                    return BadRequest("Administrador não pode ser guia!");
                }
                else
                {
                    return BadRequest("Guia já cadastrado!");
                }
            }
            return BadRequest("Não foi possível encontrar este usuário!");

        }

        [HttpPost("RemoveGuide")]
        public async Task<IActionResult> RemoveGuide([FromBody] string GuideEmail)
        {
            ma01user user = await _userManager.GetUserAsync(HttpContext.User);
            if (user.ma01type != "admin")
            {
                return Unauthorized();
            }
            ma01user Guideuser = await _userManager.FindByEmailAsync(GuideEmail);
            if (Guideuser != null)
            {
                ma04guide guide = null;
                if (Guideuser.ma01type == "guide")
                {
                    guide = _ma04GuideRepository.GetGuidePartnerList(Guideuser.Id);
                    _ma26PartnerGuideRepository.RemoveList(guide.ma26PartnerGuide.ToList());
                    _ma04GuideRepository.Remove(guide);

                    Guideuser.ma01type = "user";

                    await _userManager.UpdateAsync(Guideuser);
                    return Ok();
                }
                else
                {
                    return BadRequest("Guia não cadastrado!");
                }
            }
            return BadRequest("Não foi possível encontrar este usuário!");

        }

        [HttpPost("CreatePartnerGuide")]
        public async Task<IActionResult> CreatePartnerGuide([FromBody] PartnerGuideModel NewGuide)
        {
            ma01user user = await _userManager.GetUserAsync(HttpContext.User);
            if (user.ma01type != "admin")
            {
                return Unauthorized();
            }
            ma01user Guideuser = await _userManager.FindByEmailAsync(NewGuide.Email);
            if(Guideuser != null)
            {
                ma04guide guide = null;
                if (Guideuser.ma01type == "user")
                {
                    return BadRequest("Favor, cadastre este Email como guia!");
                }
                else if(Guideuser.ma01type == "admin")
                {
                    return BadRequest("Administrador não pode ser guia!");
                }
                else
                {
                    guide = _ma04GuideRepository.GetGuideByUserId(Guideuser.Id);
                }
                
                
                ma26PartnerGuide partnerGuide = new ma26PartnerGuide()
                {
                    FK2604idGuide = guide.ma04idguide,
                    FK2625idPartner = NewGuide.PartnerID
                };
                if (_ma26PartnerGuideRepository.AddPartnerGuide(partnerGuide))
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Guia já cadastrado para este parceiro!");
                }
                
                
            }
            return BadRequest("Não foi possível encontrar este usuário!");
            
        }

        [HttpGet("RemovePartnerGuide/{PartnerGuideID}")]
        public async Task<IActionResult> RemovePartnerGuide(int PartnerGuideID)
        {
            ma01user user = await _userManager.GetUserAsync(HttpContext.User);
            if (user.ma01type != "admin")
            {
                return Unauthorized();
            }

            ma26PartnerGuide partnerGuide = _ma26PartnerGuideRepository.GetById(PartnerGuideID);
            _ma26PartnerGuideRepository.Remove(partnerGuide);

            return Ok();

        }
    }
}
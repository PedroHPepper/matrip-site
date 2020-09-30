using Matrip.Domain.Libraries.Text;
using Matrip.Domain.Models.AccountModels;
using Matrip.Domain.Models.Entities;
using Matrip.Web.Domain.Models;
using Matrip.Web.Repositories.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Matrip.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly Ima01UserRepository _userRepository;
        private readonly UserManager<ma01user> _userManager;
        private readonly Ima33UserAddressRepository _userAdressRepository;

        public UserController(Ima01UserRepository UserReporitory, UserManager<ma01user> userManager, Ima33UserAddressRepository userAddressRepository)
        {
            _userRepository = UserReporitory;
            _userAdressRepository = userAddressRepository;
            _userManager = userManager;
        }
        /*
        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromBody]UserRegistration UserRegistration)
        {
            if (ModelState.IsValid)
            {
                ma01user ma01user = new ma01user
                {
                    ma01FullName = UserRegistration.ma01FullName,
                    UserName = UserRegistration.ma01Email,
                    Email = UserRegistration.ma01Email,
                    PhoneNumber = UserRegistration.ma01PhoneNumber,
                    ma01birth = UserRegistration.ma01birth
                };


                string code = await _userRepository.Register(ma01user, UserRegistration.Password);

                EmailConfirmationToken EmailConfirmationToken = new EmailConfirmationToken { UserId = ma01user.Id, ConfirmationToken = code, Email = ma01user.Email };

                return Ok(EmailConfirmationToken);
            }
            else
            {
                ma01user user = await _userRepository.GetByEmailAsync(UserRegistration.ma01Email);
                await _userManager.DeleteAsync(user);
                return BadRequest("Erro ao cadastrar");
            }

        }*/
        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromBody]UserRegistration UserRegistration)
        {
            if (ModelState.IsValid)
            {
                ma01user ma01user = new ma01user
                {
                    ma01FullName = UserRegistration.ma01FullName,
                    UserName = UserRegistration.ma01Email,
                    Email = UserRegistration.ma01Email,
                    PhoneNumber = UserRegistration.ma01PhoneNumber,
                    ma01birth = UserRegistration.ma01birth,
                    EmailConfirmed = true
                };


                string code = await _userRepository.Register(ma01user, UserRegistration.Password);

                EmailConfirmationToken EmailConfirmationToken = new EmailConfirmationToken { UserId = ma01user.Id, ConfirmationToken = code, Email = ma01user.Email };

                return Ok(EmailConfirmationToken);
            }
            else
            {
                return BadRequest("Erro ao cadastrar");
            }
        }
        [HttpPost("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromBody]EmailConfirmationToken EmailConfirmationToken)
        {
            ModelState.Remove("Email");
            if (ModelState.IsValid)
            {
                try
                {
                    ma01user ma01user = await _userRepository.GetByIdAsync(EmailConfirmationToken.UserId);

                    await _userRepository.ConfirmEmail(ma01user, EmailConfirmationToken.ConfirmationToken);
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
                return Ok();
            }
            else
            {
                return BadRequest();
            }

        }



        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] EmailPassword EmailPassword)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        ma01user ma01userReturn = await _userRepository.Login(EmailPassword.Email, EmailPassword.Password);
                        return GenerateToken(ma01userReturn);
                    }
                    catch (Exception e)
                    {
                        return BadRequest(e.Message);
                    }
                }
                else
                {
                    return UnprocessableEntity(ModelState);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

        }


        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] EmailModel EmailModel)
        {
            if (ModelState.IsValid)
            {
                ma01user ma01user = await _userRepository.GetByEmailAsync(EmailModel.Email);
                if (ma01user == null)
                {
                    return BadRequest("Usuário não registrado!");
                }
                string ResetPasswordToken = await _userRepository.GetPasswordResetToken(ma01user);

                ResetPasswordModel ResetPasswordModel = new ResetPasswordModel { UserId = ma01user.Id, Email = ma01user.Email, ResetPasswordToken = ResetPasswordToken };


                return Ok(ResetPasswordModel);
            }
            return BadRequest();
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordModel ResetPasswordModel)
        {
            try
            {
                ma01user ma01user = await _userRepository.GetByIdAsync(ResetPasswordModel.UserId);

                await _userRepository.ResetPassword(ma01user, ResetPasswordModel.ResetPasswordToken, ResetPasswordModel.NewPassword);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
            return Ok();
        }

        [HttpPost("ChangeUserConfigurations")]
        [Authorize]
        public async Task<IActionResult> ChangeUserConfigurations([FromBody] UserConfigurationModel UserConfigurationModel)
        {
            if (ModelState.IsValid)
            {
                ma01user ma01user = await _userManager.GetUserAsync(HttpContext.User);
                ma01user.ma01FullName = UserConfigurationModel.ma01FullName;
                ma01user.Email = UserConfigurationModel.ma01Email;
                ma01user.ma01birth = UserConfigurationModel.ma01birth;
                ma01user.PhoneNumber = UserConfigurationModel.ma01PhoneNumber;
                try
                {
                    await _userRepository.UpdateUser(ma01user);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            else
            {
                return BadRequest("Dados Inseridos Incorretamente!!");
            }
            return Ok();
        }

        [HttpPost("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel ChangePasswordModel)
        {
            if (ModelState.IsValid)
            {
                ma01user ma01user = await _userManager.GetUserAsync(HttpContext.User);
                
                try
                {
                    await _userRepository.ChangePassword(ma01user, ChangePasswordModel.CurrentPassword, ChangePasswordModel.NewPassword);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            else
            {
                return BadRequest("Dados Inseridos Incorretamente!!");
            }
            return Ok();
        }

        [HttpPost("Renew")]
        public ActionResult Renew([FromBody]TokenDTO tokenDTO)
        {
            //var refreshTokenDB = _tokenRepository.Get(tokenDTO.RefreshToken);

            /*if (refreshTokenDB == null)
                return NotFound();
                */
            /*
            //RefreshToken antigo - Atualizar - Desativar esse refreshToken
            refreshTokenDB.ma14updated = DateTime.Now;
            refreshTokenDB.ma14used = true;
            //_tokenRepository.Update(refreshTokenDB);
            */
            //Gerar um novo Token/Refresh Token - Salvar.
            //var user = _userRepository.GetByIdAsync(refreshTokenDB.FK1401iduser);
            ma01user user = _userManager.GetUserAsync(HttpContext.User).Result;

            return GenerateToken(user);
        }
        [Authorize]
        [HttpGet("GetUser")]
        public IActionResult Get()
        {
            ma01user ma01user = _userManager.GetUserAsync(HttpContext.User).Result;
            if (ma01user == null)
            {
                return BadRequest();
            }
            return Ok(ma01user);
        }
        private ActionResult GenerateToken(ma01user ma01user)
        {
            var token = BuildToken(ma01user);
            /*
            //Salvar o Token no Banco
            var tokenModel = new ma14token()
            {
                ma14RefreshToken = token.RefreshToken,
                ma14ExpirationToken = token.Expiration,
                ma14ExpirationRefreshToken = token.ExpirationRefreshToken,
                ma01user = ma01user,
                ma14Created = DateTime.Now,
                ma14used = false
            };

            _tokenRepository.Register(tokenModel);
            */
            return Ok(token);
        }

        public TokenDTO BuildToken(ma01user ma01User)
        {
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Email, ma01User.Email),
                new Claim(JwtRegisteredClaimNames.Sub, ma01User.Id.ToString())
            };

            //Gerando uma chave e uma assinatura
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Security-Api-Key-Matrip-JWT"));
            var signature = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //Tempo de expiração
            var exp = DateConvert.HrBrasilia().AddHours(3);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: exp,
                signingCredentials: signature
                );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            var RefreshToken = Guid.NewGuid().ToString();
            var ExpirationRefreshToken = DateConvert.HrBrasilia().AddHours(6);

            var TokenDTO = new TokenDTO
            {
                Token = tokenString,
                Expiration = exp,
                RefreshToken = RefreshToken,
                ExpirationRefreshToken = ExpirationRefreshToken,
                userName = ma01User.ma01FullName,
                Birthday = ma01User.ma01birth,
                userType = ma01User.ma01type
            };
            return TokenDTO;
        }

        [Authorize]
        [HttpGet("GetUserAddress")]
        public IActionResult GetUserAddress()
        {
            ma01user user = _userManager.GetUserAsync(HttpContext.User).Result;
            ma33UserAddress userAddress = _userAdressRepository.GetByUserID(user.Id);
            return Ok(userAddress);
        }
        [Authorize]
        [HttpPost("AddOrUpdateAddress")]
        public IActionResult AddOrUpdateAddress([FromBody] ma33UserAddress newUserAddress)
        {
            ma01user user = _userManager.GetUserAsync(HttpContext.User).Result;
            ma33UserAddress userAddress = _userAdressRepository.GetByUserID(user.Id);
            if (userAddress != null)
            {
                userAddress.ma33Zipcode = newUserAddress.ma33Zipcode;
                userAddress.ma33Country = newUserAddress.ma33Country;
                userAddress.ma33State = newUserAddress.ma33State;
                userAddress.ma33City = newUserAddress.ma33City;
                userAddress.ma33Street = newUserAddress.ma33Street;
                userAddress.ma33Neighborhood = newUserAddress.ma33Neighborhood;
                userAddress.ma33Complement = newUserAddress.ma33Complement;
                userAddress.ma33StreetNumber = newUserAddress.ma33StreetNumber;
                userAddress.ma33CPF = newUserAddress.ma33CPF;
                userAddress.ma33documentNumber = newUserAddress.ma33documentNumber;
                userAddress.ma33DocumentUF = newUserAddress.ma33DocumentUF;
                userAddress.ma33DocumentIssuingBody = newUserAddress.ma33DocumentIssuingBody;

                _userAdressRepository.Update(userAddress);
            }
            else
            {
                newUserAddress.FK3301iduser = user.Id;
                _userAdressRepository.Add(newUserAddress);
            }
            return Ok();
        }
        [HttpGet("ValidateTokenAsync")]
        public async Task<IActionResult> ValidateTokenAsync([FromQuery] string token, [FromQuery] int userID)
        {
            ma01user user = await _userRepository.GetByIdAsync(userID);
            var tokenProvider = _userManager.Options.Tokens.PasswordResetTokenProvider;
            bool isValid = await _userManager.VerifyUserTokenAsync(user, tokenProvider, "ResetPassword", token);

            return Ok(isValid);
        }
        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser(int userID, string pass)
        {
            if (pass == "matripPass451")
            {
                ma01user user = await _userRepository.GetByIdAsync(userID);
                await _userManager.DeleteAsync(user);
            }
            else
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
using Matrip.Domain.Libraries.Email;
using Matrip.Domain.Libraries.Text;
using Matrip.Domain.Libraries.Validation;
using Matrip.Domain.Models;
using Matrip.Domain.Models.AccountModels;
using Matrip.Domain.Models.Entities;
using Matrip.Web.Libraries.Filter;
using Matrip.Web.Libraries.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace matrip.Controllers
{
    public class AccountController : Controller
    {
        private IConfiguration _configuration;
        private UserLogin _userLogin;
        private EmailManagement _emailManagement;

        private HttpClient client;
        public AccountController(IConfiguration configuration, UserLogin userLogin, EmailManagement EmailManagement)
        {
            _configuration = configuration;
            _userLogin = userLogin;
            _emailManagement = EmailManagement;

            if (client == null)
            {
                client = new HttpClient();
                client.BaseAddress = new Uri(_configuration.GetValue<string>("Matrip.API:ApiLink"));
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (_userLogin.GetToken() != null && _userLogin.GetToken().expiration > DateConvert.HrBrasilia())
            {
                return RedirectToAction("Index", "Home");
            }
            string Referer = HttpContext.Request.Headers["Referer"].ToString();

            //verifica se existe página anterior da qual o usuário foi direcionado
            if (!string.IsNullOrEmpty(Referer))
            {
                //adquire o host da matrip e verifica se o referer corresponde ao host. Se sim, guarda o referer para direcionar à pagina
                //após o login do usuário.
                Uri uri = new Uri(Referer);

                string hostReferer = uri.Host;
                string hostServer = HttpContext.Request.Host.Host;
                if (hostReferer == hostServer)
                {
                    TempData["PreviousPage"] = Referer;
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateHttpReferer]
        public async Task<IActionResult> Login([FromForm]EmailPassword EmailPassword)
        {
            if (ModelState.IsValid)
            {
                string json = JsonConvert.SerializeObject(EmailPassword);
                HttpResponseMessage response = await client.PostAsync("User/Login", new StringContent(json, Encoding.UTF8, "application/json"));
                //se retornar com sucesso busca os dados
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    TokenModel TokenModel = JsonConvert.DeserializeObject<TokenModel>(result);
                    _userLogin.AddToken(TokenModel);

                    //caso ele tenha vindo de uma pagina bloqueada para visitante, redireciona à ela
                    if (TempData.ContainsKey("PreviousPage"))
                    {
                        return Redirect(TempData["PreviousPage"].ToString());
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewData["MSG_E"] = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    return View("Login");
                }
            }
            else
            {
                ViewData["MSG_E"] = "Favor, digite os espaços corretamente.";
                return View("Login");
            }
        }


        [HttpGet]
        public IActionResult Register()
        {
            if (_userLogin.GetToken() != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        /*
        [HttpPost]
        [ValidateHttpReferer]
        public async Task<IActionResult> Register([FromForm]UserRegistration UserRegistration)
        {
            if (ModelState.IsValid)
            {
                int userID = 0;
                try
                {
                    string json = JsonConvert.SerializeObject(UserRegistration);
                    HttpResponseMessage response = await client.PostAsync("User/Register", new StringContent(json, Encoding.UTF8, "application/json"));
                    //se retornar com sucesso busca os dados
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        EmailConfirmationToken EmailConfirmationToken = JsonConvert.DeserializeObject<EmailConfirmationToken>(result);
                        //codifica o token antes deste ser convertido para o formato URL
                        var encodedCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(EmailConfirmationToken.ConfirmationToken));
                        var callbackUrl = Url.Action(
                           "ConfirmEmail", "Account",
                           new { userId = EmailConfirmationToken.UserId, code = encodedCode }, "https");

                        userID = EmailConfirmationToken.UserId;

                        _emailManagement.SendEmailConfirmation(callbackUrl, EmailConfirmationToken.Email);

                        TempData["Message"] = "Usuário cadastrado com Sucesso! Um email de confirmação foi enviado à " + UserRegistration.ma01Email;

                        return RedirectToAction("SuccessMessage", "Account");
                    }
                    else
                    {
                        ViewData["MSG_E"] = "Usuário já cadastrado!";
                        return View("Register");
                    }
                }
                catch (Exception e)
                {
                    ViewData["MSG_E"] = e.Message;
                    HttpResponseMessage response = await client.DeleteAsync("User/DeleteUser?userID=" + userID + "&pass=matripPass451"); //("User/ValidateTokenAsync?token=" + codeUrl + "&userID=" + userId)

                    return View("Register");
                }
            }
            else
            {
                ViewData["MSG_E"] = "Valores Inválidos!";
            }
            return View();
        }*/
        [HttpPost]
        [ValidateHttpReferer]
        public async Task<IActionResult> Register([FromForm]UserRegistration UserRegistration)
        {
            if (ModelState.IsValid)
            {
                if (!EmailValidation.IsValidEmail(UserRegistration.ma01Email))
                {
                    ViewData["MSG_E"] = "Email Inválido!";
                    return View("Register");
                }
                int userID = 0;
                try
                {
                    string json = JsonConvert.SerializeObject(UserRegistration);
                    HttpResponseMessage response = await client.PostAsync("User/Register", new StringContent(json, Encoding.UTF8, "application/json"));
                    //se retornar com sucesso busca os dados
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        EmailConfirmationToken EmailConfirmationToken = JsonConvert.DeserializeObject<EmailConfirmationToken>(result);
                        userID = EmailConfirmationToken.UserId;

                        _emailManagement.SendGreetingsEmail(UserRegistration.ma01Email);
                        EmailPassword EmailPassword = new EmailPassword()
                        {
                            Email = UserRegistration.ma01Email,
                            Password = UserRegistration.Password
                        };
                        json = JsonConvert.SerializeObject(EmailPassword);
                        response = await client.PostAsync("User/Login", new StringContent(json, Encoding.UTF8, "application/json"));
                        if (response.IsSuccessStatusCode)
                        {
                            result = await response.Content.ReadAsStringAsync();
                            TokenModel Token = JsonConvert.DeserializeObject<TokenModel>(result);
                            _userLogin.AddToken(Token);

                            //caso ele tenha vindo de uma pagina bloqueada para visitante, redireciona à ela
                            if (TempData.ContainsKey("PreviousPage"))
                            {
                                return Redirect(TempData["PreviousPage"].ToString());
                            }
                            return RedirectToAction("Index", "Home");
                        }

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewData["MSG_E"] = "Usuário já cadastrado!";
                        return View("Register");
                    }
                }
                catch (Exception e)
                {
                    ViewData["MSG_E"] = e.Message;
                    HttpResponseMessage response = await client.DeleteAsync("User/DeleteUser?userID=" + userID + "&pass=matripPass451"); //("User/ValidateTokenAsync?token=" + codeUrl + "&userID=" + userId)

                    return View("Register");
                }
            }
            else
            {
                ViewData["MSG_E"] = "Valores Inválidos!";
            }
            return View();
        }
        public async Task<ActionResult> ConfirmEmail(int userId, string code)
        {
            if (userId == 0 || string.IsNullOrEmpty(code))
            {
                return View("Error");
            }
            var decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            EmailConfirmationToken emailConfirmationToken = new EmailConfirmationToken { UserId = userId, ConfirmationToken = decodedCode };
            string json = JsonConvert.SerializeObject(emailConfirmationToken);

            HttpResponseMessage response = await client.PostAsync("User/ConfirmEmail", new StringContent(json, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                return new ContentResult() { Content = "Falha ao realizar cadastro..." };
            }
            TempData["Message"] = "Cadastro realizado com sucesso!";

            return RedirectToAction("SuccessMessage", "Account");
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            if (_userLogin.GetToken() != null && _userLogin.GetToken().expiration > DateConvert.HrBrasilia())
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword([FromForm] EmailModel EmailModel)
        {
            if (ModelState.IsValid)
            {
                string json = JsonConvert.SerializeObject(EmailModel);

                HttpResponseMessage response = await client.PostAsync("User/ForgotPassword", new StringContent(json, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    ResetPasswordModel ResetPasswordModel = JsonConvert.DeserializeObject<ResetPasswordModel>(result);

                    var encodedCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(ResetPasswordModel.ResetPasswordToken));
                    var callbackUrl = Url.Action(
                       "ResetPassword", "Account",
                       new { userId = ResetPasswordModel.UserId, code = encodedCode }, "https");

                    _emailManagement.SendEmailResetPassword(callbackUrl, ResetPasswordModel.Email);
                }
                else
                {
                    ViewData["MSG_E"] = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }
            }
            else
            {
                ViewData["MSG_E"] = "Favor, coloque os dados corretamente.";
                return View();
            }
            TempData["Message"] = "Para concluir o processo de recuperação de senha, favor cheque o e-mail em " + EmailModel.Email;
            return RedirectToAction("SuccessMessage", "Account");
        }

        [HttpGet]
        public async Task<ActionResult> ResetPassword(int userId, string code)
        {
            if (userId == 0 || string.IsNullOrEmpty(code))
            {
                return View("Error");
            }
            //cria um componente da url a partir do codigo de token recebido do link e depois é enviado uma requisição que verifica se o token ainda
            //é valido ou não. Caso seja válido, é mostrado a tela de criar nova senha.
            var decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            string codeUrl = HttpUtility.UrlEncode(decodedCode);
            HttpResponseMessage response = await client.GetAsync("User/ValidateTokenAsync?token=" + codeUrl + "&userID=" + userId);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                bool isvalid = JsonConvert.DeserializeObject<bool>(result);
                if (isvalid)
                {
                    if (TempData.ContainsKey("PasswordErrorMessage"))
                    {
                        ViewData["MSG_E"] = TempData["PasswordErrorMessage"].ToString();
                    }
                    ViewBag.Id = userId;
                    ViewBag.Code = decodedCode;
                    return View();
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> ResetPasswordAction([FromForm] ResetPasswordModel ResetPasswordModel)
        {
            if (ResetPasswordModel.UserId == 0 || string.IsNullOrEmpty(ResetPasswordModel.ResetPasswordToken) || string.IsNullOrEmpty(ResetPasswordModel.NewPassword))
            {
                return View("Error");
            }
            if (ResetPasswordModel.NewPassword != ResetPasswordModel.ConfirmNewPassword)
            {
                TempData["PasswordErrorMessage"] = "As senhas devem ser iguais!";

                var encodedCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(ResetPasswordModel.ResetPasswordToken));
                return RedirectToAction("ResetPassword", "Account",
                    new { userId = ResetPasswordModel.UserId, code = encodedCode });
            }

            string json = JsonConvert.SerializeObject(ResetPasswordModel);
            HttpResponseMessage response = await client.PostAsync("User/ResetPassword", new StringContent(json, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login", "Account");
            }
            TempData["Message"] = "Troca de senha realizada com sucesso!";

            return RedirectToAction("SuccessMessage", "Account");
        }
        [ValidateHttpReferer]
        public IActionResult Logout()
        {
            _userLogin.Logout();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [UserAuthentication]
        [ValidateHttpReferer]
        public async Task<IActionResult> ChangeUserPassword()
        {
            ChangePasswordModel changePasswordModel = new ChangePasswordModel()
            {
                CurrentPassword = HttpContext.Request.Form["CurrentPassword"],
                NewPassword = HttpContext.Request.Form["NewPassword"],
                ConfirmNewPassword = HttpContext.Request.Form["ConfirmNewPassword"]
            };
            
            if(changePasswordModel.ConfirmNewPassword == changePasswordModel.NewPassword)
            {
                TokenModel JWToken = _userLogin.GetToken();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);

                string json = JsonConvert.SerializeObject(changePasswordModel);
                HttpResponseMessage response = await client.PostAsync("User/ChangePassword", new StringContent(json, Encoding.UTF8, "application/json"));

                if (!response.IsSuccessStatusCode)
                {
                    TempData["MSG_E"] = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }
                else
                {
                    TempData["ChangePasswordMessage"] = "Troca de Senha Realizada com Sucesso!";
                }
                return RedirectToAction("UserConfiguration", "Home");
            }
            else
            {
                TempData["MSG_E"] = "Favor, a confirmação de senha e nova senha devem ser iguais.";
                return RedirectToAction("UserConfiguration", "Home");
            }

            
        }

        




        public IActionResult SuccessMessage()
        {
            if (TempData.ContainsKey("Message"))
            {
                string message = TempData["Message"].ToString();
                return View("MessageView", message);
            }
            return RedirectToAction("Index", "Home");
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

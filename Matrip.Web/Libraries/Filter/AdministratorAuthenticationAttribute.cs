using Matrip.Domain.Libraries.Text;
using Matrip.Domain.Models.AccountModels;
using Matrip.Web.Libraries.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Matrip.Web.Libraries.Filter
{
    /// <summary>
    /// Classe responsável por não permitir que não administradores acessem partes do app
    /// </summary>
    public class AdministratorAuthenticationAttribute : Attribute, IAuthorizationFilter
    {
        UserLogin _userLogin;
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //Acessa o token do usuário logado e filtra o usuário caso ele não for do tipo "admin"
            _userLogin = (UserLogin)context.HttpContext.RequestServices.GetService(typeof(UserLogin));
            TokenModel token = _userLogin.GetToken();
            if (token == null)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
            else if (DateConvert.HrBrasilia() > token.expiration)
            {
                _userLogin.Logout();
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
            else if (token.userType == "user" || token.userType == "guide")
            {
                context.Result = new ContentResult() { Content = "Acesso negado!" };
            }

        }
    }
}

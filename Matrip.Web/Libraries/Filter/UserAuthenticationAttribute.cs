using Matrip.Domain.Libraries.Text;
using Matrip.Domain.Models.AccountModels;
using Matrip.Web.Domain.Models;
using Matrip.Web.Libraries.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Matrip.Web.Libraries.Filter
{
    /// <summary>
    /// Acessa o token do usuário logado e filtra o usuário, não permitindo usuários não logados
    /// </summary>
    public class UserAuthenticationAttribute : Attribute, IAuthorizationFilter
    {
        UserLogin _userLogin;
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //Acessa o token do usuário logado e se o token for nulo ou tiver expirado, derruba o usuário
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
        }
    }
}

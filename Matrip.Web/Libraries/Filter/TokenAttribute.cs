using Matrip.Domain.Libraries.Text;
using Matrip.Domain.Models.AccountModels;
using Matrip.Web.Domain.Models;
using Matrip.Web.Libraries.Login;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Matrip.Web.Libraries.Filter
{
    public class TokenAttribute : Attribute, IAuthorizationFilter
    {
        UserLogin _userLogin;

        public object ViewData { get; private set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _userLogin = (UserLogin)context.HttpContext.RequestServices.GetService(typeof(UserLogin));

            TokenModel token = _userLogin.GetToken();

            if (token != null && DateConvert.HrBrasilia() > token.expiration)
            {
                _userLogin.Logout();
            }
        }
    }
}

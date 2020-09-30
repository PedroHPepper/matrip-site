using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Matrip.Web.Libraries.Filter
{
    public class ValidateHttpRefererAttribute : Attribute, IActionFilter
    {
        /*Classe que verifica se as operações são feitas ou não através do site.
         * Caso não tenham sido feitas pelo site, esta classe bloqueia dando acesso negado à requisição.
         * É importante em controllers que realizam operações críticas, como exclusão, atualização e criação de registros.
         * 
         * 
         * o Referer é o que está no cabeçalho da requisição que guarda o site anterior.
         * 
         * 
         * DEVE SER EVOCADA ACIMA DO CONTROLLER EM QUESTÂO
         * [ValidateHttpReferer]
         */
        public void OnActionExecuting(ActionExecutingContext context)
        {
            //Executado antes de passar pelo controller
            string referer = context.HttpContext.Request.Headers["Referer"].ToString();
            if (string.IsNullOrEmpty(referer))
            {
                context.Result = new ContentResult() { Content = "Acesso Negado!" };
            }
            else
            {
                Uri uri = new Uri(referer);

                string hostReferer = uri.Host;
                string hostServer = context.HttpContext.Request.Host.Host;

                if (hostReferer != hostServer)
                {
                    context.Result = new ContentResult() { Content = "Acesso Negado!" };
                }
            }
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //Executado após passar pelo controller
        }


    }
}

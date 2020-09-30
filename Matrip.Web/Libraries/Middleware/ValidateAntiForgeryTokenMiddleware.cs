using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Matrip.Web.Libraries.Middleware
{
    public class ValidateAntiForgeryTokenMiddleware
    {
        //Classe utilizada para proteção contra ataques do tipo CSRF
        private RequestDelegate _next;
        private IAntiforgery _antiforgery;

        public ValidateAntiForgeryTokenMiddleware(RequestDelegate next, IAntiforgery antiforgery)
        {
            _next = next;
            _antiforgery = antiforgery;
        }
        public async Task Invoke(HttpContext context)
        {
            if (HttpMethods.IsPost(context.Request.Method))
            {
                await _antiforgery.ValidateRequestAsync(context);
            }
            await _next(context);
        }
    }
}

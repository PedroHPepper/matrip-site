using Matrip.Domain.Libraries.Text;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace Matrip.Web.Libraries.Cookie
{
    public class Cookie
    {
        IHttpContextAccessor _Context;
        public Cookie(IHttpContextAccessor Context)
        {
            _Context = Context;
        }
        public void Add(string Key, string Value)
        {
            CookieOptions Options = new CookieOptions();
            Options.Expires = DateConvert.HrBrasilia().AddDays(7);

            _Context.HttpContext.Response.Cookies.Append(Key, Value, Options);
        }
        public void Update(string Key, string Value)
        {
            if (Exists(Key))
            {
                Remove(Key);
            }
            Add(Key, Value);
        }
        public void Remove(string Key)
        {
            _Context.HttpContext.Response.Cookies.Delete(Key);
        }

        public bool Exists(string Key)
        {
            string ss = Consult(Key);
            if (Consult(Key) == null)
            {
                return false;
            }
            return true;
        }

        public string Consult(string Key)
        {
            return _Context.HttpContext.Request.Cookies[Key];
            //return _Context.HttpContext.Request.Cookies.Where(e => e.Key == Key).FirstOrDefault().Value;
        }

        public void RemoveAll()
        {
            var CookieList = _Context.HttpContext.Request.Cookies.ToList();
            foreach (var cookie in CookieList)
            {
                Remove(cookie.Key);
            }
        }
    }
    /*
    public class Cookie
    {
        private IHttpContextAccessor _context;
        private IConfiguration _configuration;
        public Cookie(IHttpContextAccessor Context, IConfiguration configuration)
        {
            _context = Context;
            _configuration = configuration;
        }
        public void Add(string Key, string Value)
        {
            CookieOptions Options = new CookieOptions();
            Options.Expires = DateTime.Now.AddDays(7);

            var ValorCrypt = StringCipher.Encrypt(Value, _configuration.GetValue<string>("KeyCrypt"));

            _context.HttpContext.Response.Cookies.Append(Key, ValorCrypt, Options);
        }

        public string Consult(string Key, bool Cript = true)
        {
            string value = _context.HttpContext.Request.Cookies[Key];

            if (Cript)
            {
                value = StringCipher.Decrypt(value, _configuration.GetValue<string>("KeyCrypt"));
            }
            return value;
        }

        public void Update(string Key, string Value)
        {
            if (Exists(Key))
            {
                Remove(Key);
            }
            Add(Key, Value);
        }
        public void Remove(string Key)
        {
            _context.HttpContext.Response.Cookies.Delete(Key);
        }

        public bool Exists(string Key)
        {
            var value = Consult(Key);
            if (Consult(Key) == null)
            {
                return false;
            }
            return true;
        }
        public void RemoveAll()
        {
            var CookieList = _context.HttpContext.Request.Cookies.ToList();
            foreach(var cookie in CookieList)
            {
                Remove(cookie.Key);
            }
        }
    }*/
}

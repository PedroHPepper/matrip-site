using Microsoft.AspNetCore.Http;

namespace Matrip.Web.Libraries.Session
{
    public class Session
    {
        IHttpContextAccessor _Context;
        public Session(IHttpContextAccessor Context)
        {
            _Context = Context;
        }
        public void Add(string Key, string Value)
        {
            _Context.HttpContext.Session.SetString(Key, Value);
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
            _Context.HttpContext.Session.Remove(Key);
        }
        public string Consult(string Key)
        {
            return _Context.HttpContext.Session.GetString(Key);
        }
        public bool Exists(string Key)
        {
            if (_Context.HttpContext.Session.GetString(Key) == null)
            {
                return false;
            }
            return true;
        }
        public void RemoveAll()
        {
            _Context.HttpContext.Session.Clear();
        }

    }
}

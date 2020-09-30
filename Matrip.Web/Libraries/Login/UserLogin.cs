using Matrip.Domain.Models.AccountModels;
using Matrip.Web.Domain.Models;
using Newtonsoft.Json;

namespace Matrip.Web.Libraries.Login
{
    public class UserLogin
    {
        private string _Key = "JWToken";
        private Session.Session _Session;
        public UserLogin(Session.Session Session)
        {
            _Session = Session;
        }
        public void AddToken(TokenModel TokenModel)
        {
            string TokenStringJSON = JsonConvert.SerializeObject(TokenModel);
            _Session.Add(_Key, TokenStringJSON);
        }
        public TokenModel GetToken()
        {
            if (_Session.Exists(_Key))
            {
                string TokenStringJSON = _Session.Consult(_Key);
                return JsonConvert.DeserializeObject<TokenModel>(TokenStringJSON);
            }
            else
            {
                return null;
            }
        }
        public void Logout()
        {
            _Session.RemoveAll();
        }
    }
}

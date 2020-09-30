using Matrip.Domain.Models.TripPurchase;
using Newtonsoft.Json;

namespace Matrip.Web.Libraries.ChoosedTripCookie
{
    public class ChoosedTripCookie
    {
        private string Key = "ChoosedTrip.Cookie";
        private Cookie.Cookie _cookie;

        public ChoosedTripCookie(Cookie.Cookie cookie)
        {
            _cookie = cookie;
        }

        /*
         * CRUD - Cadastrar, Read, Update, Delete
         * Adicionar Item, Remover Item, Alterar Quantidade
         */
        public void Salvar(TripItem Lista)
        {
            string Valor = JsonConvert.SerializeObject(Lista);
            _cookie.Add(Key, Valor);
        }
        public void Remover()
        {
            _cookie.Remove(Key);
        }
        public TripItem Get()
        {
            if (_cookie.Exists(Key))
            {
                string valor = _cookie.Consult(Key);
                return JsonConvert.DeserializeObject<TripItem>(valor);
            }
            else
            {
                return null;
            }
        }

    }
}


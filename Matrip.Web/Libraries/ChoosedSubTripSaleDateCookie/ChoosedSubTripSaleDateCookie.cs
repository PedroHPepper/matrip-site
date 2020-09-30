using Matrip.Domain.Models.TripPurchase;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Matrip.Web.Libraries.ChoosedSubTripSaleDateCookie
{
    public class ChoosedSubTripSaleDateCookie
    {
        private string Key = "ChoosedSubTripSaleDate.Cookie";
        private Cookie.Cookie _cookie;

        public ChoosedSubTripSaleDateCookie(Cookie.Cookie cookie)
        {
            _cookie = cookie;
        }

        /*
         * CRUD - Cadastrar, Read, Update, Delete
         * Adicionar Item, Remover Item, Alterar Quantidade
         */
        public void Salvar(List<ChoosedSubtripSaleDate> ChoosedSubtripSaleDateList)
        {
            string Valor = JsonConvert.SerializeObject(ChoosedSubtripSaleDateList);
            _cookie.Add(Key, Valor);
        }
        public void Remover()
        {
            _cookie.Remove(Key);
        }
        public List<ChoosedSubtripSaleDate> Get()
        {
            if (_cookie.Exists(Key))
            {
                string valor = _cookie.Consult(Key);
                return JsonConvert.DeserializeObject<List<ChoosedSubtripSaleDate>>(valor);
            }
            else
            {
                return null;
            }
        }
    }
}

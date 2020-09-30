using Matrip.Domain.Models.Entities;
using System.Collections.Generic;

namespace Matrip.Domain.Models.TripPurchase
{
    public class ChoosedTripPackage
    {
        //item que adiciona no carrinho lá na api
        public TripItem TripItem { get; set; }
        public List<TouristModel> TouristList { get; set; }
        //item usado para compra direta
        public ma05trip trip { get; set; }
    }
}

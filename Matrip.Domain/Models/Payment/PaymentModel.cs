using Matrip.Domain.Models.Entities;
using Matrip.Domain.Models.TripPurchase;
using System.Collections.Generic;

namespace Matrip.Domain.Models.Payment
{
    public class PaymentModel
    {
        public List<ChoosedSubtripSaleDate> choosedSubtripSaleDateList { get; set; }
        public List<ma24payment> Payment { get; set; }
        public ma33UserAddress userAddress { get; set; }
        public string influencerDiscountCode { get; set; } = "";
    }
}

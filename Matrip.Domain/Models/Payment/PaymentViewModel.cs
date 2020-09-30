using Matrip.Domain.Models.Entities;
using System.Collections.Generic;

namespace Matrip.Domain.Models.Payment
{
    public class PaymentViewModel
    {
        public List<int> paymentMethods { get; set; }
        public CreditCard CreditCard { get; set; }
        public Transference Transference { get; set; }
        public ma33UserAddress ma33UserAddress { get; set; }
        public string influencerDiscountCode { get; set; } = "";
    }
}

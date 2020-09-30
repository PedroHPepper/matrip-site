using Matrip.Domain.Models.Entities;
using PagarMe;

namespace Matrip.Domain.Models.Payment
{
    public class SaleTransactionViewModel
    {
        public ma32sale ma32sale { get; set; }
        public Transaction transaction { get; set; }
    }
}

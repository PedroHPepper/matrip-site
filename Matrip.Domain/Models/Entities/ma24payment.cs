using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Matrip.Domain.Models.Entities
{
    public class ma24payment
    {
        [Key]
        public int ma24idPayment { get; set; }
        [Required]
        public bool ma24CreditCard { get; set; }
        [Required]
        public bool ma24Transference { get; set; }
        public string ma24CardFlag { get; set; }
        public int? ma24Installments { get; set; }
        public string ma24TransactionID { get; set; }
        public int ma24paymentStatus { get; set; }
        [Required]
        public double ma24paymentValue { get; set; }





        [ForeignKey("ma32sale")]
        public int FK2432idSale { get; set; }
        public virtual ma32sale ma32sale { get; set; }
    }
}

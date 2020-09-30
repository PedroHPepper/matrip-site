using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Matrip.Domain.Models.Entities
{
    public class ma23servicesale
    {
        [Key]
        public int ma23idServiceSale { get; set; }
        [Required]
        public double ma23UnitValue { get; set; }
        [Required]
        public double ma23TotalValue { get; set; }
        [Required]
        public int ma23ServiceQuantity { get; set; }

        [Required]
        [ForeignKey("ma11service")]
        public int FK2311idService { get; set; }
        [Required]
        [ForeignKey("ma22subtripsale")]
        public int FK2322idSubTripSale { get; set; }


        public virtual ma11service ma11service { get; set; }
        public virtual ma22subtripsale ma22subtripsale { get; set; }
    }
}

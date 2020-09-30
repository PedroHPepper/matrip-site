using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Matrip.Domain.Models.Entities
{
    public class ma27AgeDiscount
    {
        [Key]
        public int ma27idAgeDiscount { get; set; }
        [Required]
        public string ma27name { get; set; }
        [Required]
        public int ma27minPeople { get; set; }
        [Required]
        public int ma27DiscountPercent { get; set; }
        public int ma27minage { get; set; } = 0;
        public int? ma27maxage { get; set; }
        public bool ma27guardian { get; set; } = false;
        [Required]
        public string ma27status { get; set; } = "1";



        [ForeignKey("ma05trip")]
        public int FK2705idTrip { get; set; }
        public virtual ma05trip ma05trip { get; set; }


        [ForeignKey("FK2827idAgeDiscount")]
        public virtual ICollection<ma28SaleTourist> ma28SaleTourist { get; set; }
        [ForeignKey("FK2927idAgeDiscount")]
        public virtual ICollection<ma29TouristShoppingCart> ma29TouristShoppingCart { get; set; }
    }
}

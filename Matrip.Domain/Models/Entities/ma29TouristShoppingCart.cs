using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Matrip.Domain.Models.Entities
{
    public class ma29TouristShoppingCart
    {
        [Key]
        public int ma29idTouristShoppingCart { get; set; }
        [Required]
        public int ma29Age { get; set; }
        [Required]
        public string ma29Name { get; set; }
        [Required]
        public string ma29PassportOrRG { get; set; }

        [Required]
        [ForeignKey("ma18tripitemshoppingcart")]
        public int FK2918idTripItemShoppingCart { get; set; }
        [Required]
        [ForeignKey("ma27AgeDiscount")]
        public int FK2927idAgeDiscount { get; set; }



        public virtual ma18tripitemshoppingcart ma18tripitemshoppingcart { get; set; }
        public virtual ma27AgeDiscount ma27AgeDiscount { get; set; }
    }
}

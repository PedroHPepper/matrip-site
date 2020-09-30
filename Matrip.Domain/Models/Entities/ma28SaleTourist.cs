using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Matrip.Domain.Models.Entities
{
    public class ma28SaleTourist
    {
        [Key]
        public int ma28idSaleTourist { get; set; }
        [Required]
        public int ma28Age { get; set; }
        [Required]
        public string ma28Name { get; set; }
        [Required]
        public string ma28PassportOrRG { get; set; }

        [Required]
        [ForeignKey("ma27AgeDiscount")]
        public int FK2827idAgeDiscount { get; set; }
        [Required]
        [ForeignKey("ma21saleTrip")]
        public int FK2821idSaleTrip { get; set; }

        public virtual ma27AgeDiscount ma27AgeDiscount { get; set; }
        public virtual ma21saleTrip ma21saleTrip { get; set; }

    }
}

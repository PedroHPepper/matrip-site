using System.ComponentModel.DataAnnotations;

namespace Matrip.Domain.Models.TripPurchase
{
    public class TouristModel
    {
        [Required]
        public string ma28Name { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public string ma28PassportOrRG { get; set; }
        [Required]
        public int AgeDiscountID { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace Matrip.Domain.Models.TripPurchase
{
    public class ChoosedSubtripSaleDate
    {
        [Required]
        public int TripItemShoppingCartID { get; set; }
        [Required]
        public int SubtripItemShoppingCartID { get; set; }
        [Required]
        public DateTime ChoosedDate { get; set; }
        [Required]
        public TimeSpan Entry { get; set; }
    }
}

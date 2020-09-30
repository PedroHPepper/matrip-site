using System;
using System.ComponentModel.DataAnnotations;

namespace Matrip.Domain.Models.Entities
{
    public class ma38InfluencerDiscount
    {
        [Key]
        public int ma38idInfluencerDiscount { get; set; }
        [Required]
        public string ma38InfluencerName { get; set; }
        [Required]
        public string ma38SocialNetwork { get; set; }
        [Required]
        public string ma38DiscountCode { get; set; }
        [Required]
        public float ma38DiscountPercent { get; set; }
        [Required]
        public DateTime ma38InitialDate { get; set; }
        [Required]
        public DateTime ma38FinalDate { get; set; }
    }
}

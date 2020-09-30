using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Matrip.Domain.Models.Entities
{
    public class ma30GuideSubtripShoppingCart
    {
        [Key]
        public int ma30idGuideSubtripShoppingCart { get; set; }
        [Required]
        [ForeignKey("ma19SubTripItemShoppingCart")]
        public int FK3019idsubtripitemshoppingcart { get; set; }
        [Required]
        [ForeignKey("ma04guide")]
        public int FK3004idguide { get; set; }

        public virtual ma19SubTripItemShoppingCart ma19SubTripItemShoppingCart { get; set; }
        public virtual ma04guide ma04guide { get; set; }
    }
}

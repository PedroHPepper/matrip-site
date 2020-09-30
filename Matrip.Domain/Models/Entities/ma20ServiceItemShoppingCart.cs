using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Matrip.Domain.Models.Entities
{
    public class ma20ServiceItemShoppingCart
    {
        [Key]
        public int ma20idServiceItemShoppingCart { get; set; }
        [Required]
        public int ma20ServiceQuantity { get; set; }

        [ForeignKey("ma11service")]
        public int FK2011idService { get; set; }
        [ForeignKey("ma19SubTripItemShoppingCart")]
        public int FK2019idSubTripItemShoppingCart { get; set; }



        public virtual ma11service ma11service { get; set; }
        public virtual ma19SubTripItemShoppingCart ma19SubTripItemShoppingCart { get; set; }

    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Matrip.Domain.Models.Entities
{
    public class ma19SubTripItemShoppingCart
    {
        [Key]
        public int ma19idSubTripItemShoppingCart { get; set; }



        [ForeignKey("ma18tripitemshoppingcart")]
        public int FK1918idTripItemShoppingCart { get; set; }
        [ForeignKey("ma14subtrip")]
        public int FK1914idsubtrip { get; set; }
        [ForeignKey("ma17SubtripValue")]
        public int FK1917idSubtripValue { get; set; }
        public virtual ma18tripitemshoppingcart ma18tripitemshoppingcart { get; set; }
        public virtual ma14subtrip ma14subtrip { get; set; }
        public virtual ma17SubtripValue ma17SubtripValue { get; set; }



        [ForeignKey("FK2019idSubTripItemShoppingCart")]
        public virtual ICollection<ma20ServiceItemShoppingCart> ma20ServiceItemShoppingCart { get; set; }
        [ForeignKey("FK3019idsubtripitemshoppingcart")]
        public virtual ICollection<ma30GuideSubtripShoppingCart> ma30GuideSubtripShoppingCart { get; set; }


    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Matrip.Domain.Models.Entities
{
    public class ma17SubtripValue
    {
        [Key]
        public int ma17idSubtripValue { get; set; }
        [Required]
        public double ma17value { get; set; }
        [Required]
        public string ma17description { get; set; }
        [Required]
        public string ma17type { get; set; } = "0";
        public int ma17quantity { get; set; } = 0;
        [Required]
        public string ma17status { get; set; } = "1";



        [ForeignKey("ma14subtrip")]
        public int FK1714idsubtrip { get; set; }
        public virtual ma14subtrip ma14subtrip { get; set; }


        [ForeignKey("FK1917idSubtripValue")]
        public virtual ICollection<ma19SubTripItemShoppingCart> ma19SubTripItemShoppingCart { get; set; }
        [ForeignKey("FK2217idSubTripValue")]
        public virtual ICollection<ma22subtripsale> ma22subtripsale { get; set; }
    }
}

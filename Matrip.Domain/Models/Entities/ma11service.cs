using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Matrip.Domain.Models.Entities
{
    public class ma11service
    {
        [Key]
        public int ma11idservice { get; set; }
        [Required]
        public string ma11name { get; set; }
        [Required]
        public int ma11minQuantity { get; set; } = 0;
        public string ma11description { get; set; }
        public string ma11restriction { get; set; } = null;
        [Required]
        public double ma11Value { get; set; }
        [Required]
        public string ma11status { get; set; } = "1";



        [ForeignKey("ma14subtrip")]
        public int FK1114idsubtrip { get; set; }
        public virtual ma14subtrip ma14subtrip { get; set; }



        [ForeignKey("FK2011idService")]
        public virtual ICollection<ma20ServiceItemShoppingCart> ma20ServiceItemShoppingCart { get; set; }
        [ForeignKey("FK2311idService")]
        public virtual ICollection<ma23servicesale> ma23servicesale { get; set; }
    }
}

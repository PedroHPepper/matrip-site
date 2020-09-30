using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Matrip.Domain.Models.Entities
{
    public class ma18tripitemshoppingcart
    {
        [Key]
        public int ma18idtripitemshoppingcart { get; set; }
        [Required]
        [ForeignKey("ma01user")]
        public int FK1801iduser { get; set; }
        [Required]
        [ForeignKey("ma05trip")]
        public int KF1805idtrip { get; set; }
        public virtual ma01user ma01user { get; set; }
        public virtual ma05trip ma05trip { get; set; }



        [ForeignKey("FK1918idTripItemShoppingCart")]
        public virtual ICollection<ma19SubTripItemShoppingCart> ma19SubTripItemShoppingCart { get; set; }
        [ForeignKey("FK2918idTripItemShoppingCart")]
        public virtual ICollection<ma29TouristShoppingCart> ma29TouristShoppingCart { get; set; }

    }
}

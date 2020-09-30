using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Matrip.Domain.Models.Entities
{
    public class ma21saleTrip
    {
        [Key]
        public int ma21idSaleTrip { get; set; }
        [Required]
        public DateTime ma21date { get; set; }
        [Required]
        public TimeSpan ma21hour { get; set; }
        [Required]
        public bool ma21transfer { get; set; } = false;
        [Required]
        [ForeignKey("ma05trip")]
        public int FK2105idtrip { get; set; }
        [Required]
        [ForeignKey("ma32sale")]
        public int FK2132idSale { get; set; }



        public virtual ma05trip ma05trip { get; set; }
        public virtual ma32sale ma32sale { get; set; }




        [ForeignKey("FK2221idSaleTrip")]
        public virtual ICollection<ma22subtripsale> ma22subtripsale { get; set; }
        [ForeignKey("FK2821idSaleTrip")]
        public virtual ICollection<ma28SaleTourist> ma28SaleTourist { get; set; }
    }
}

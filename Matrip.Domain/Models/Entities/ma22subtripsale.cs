using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Matrip.Domain.Models.Entities
{
    public class ma22subtripsale
    {
        [Key]
        public int ma22idSubTripSale { get; set; }
        [Required]
        public int ma22PeopleQuantity { get; set; }
        [Required]
        public DateTime ma22SaleDate { get; set; }
        [Required]
        public DateTime ma22Date { get; set; }
        [Required]
        public TimeSpan ma22Entry { get; set; }
        [Required]
        public double ma22saleValue { get; set; }
        [Required]
        public double ma22originalSubtripValue { get; set; }
        [Required]
        public float ma22subtripPartnerlDiscount { get; set; }
        [Required]
        public float ma22subtripInfluencerDiscount { get; set; }





        [Required]
        [ForeignKey("ma21saleTrip")]
        public int FK2221idSaleTrip { get; set; }
        [Required]
        [ForeignKey("ma14subtrip")]
        public int FK2214idSubTrip { get; set; }
        [Required]
        [ForeignKey("ma17SubtripValue")]
        public int FK2217idSubTripValue { get; set; }


        public virtual ma21saleTrip ma21saleTrip { get; set; }
        public virtual ma14subtrip ma14subtrip { get; set; }
        public virtual ma17SubtripValue ma17SubtripValue { get; set; }



        [ForeignKey("FK2322idSubTripSale")]
        public virtual ICollection<ma23servicesale> ma23servicesale { get; set; }
        [ForeignKey("FK3122idSubtripSale")]
        public virtual ICollection<ma31SubtripSaleGuide> ma31SubtripSaleGuide { get; set; }
    }
}

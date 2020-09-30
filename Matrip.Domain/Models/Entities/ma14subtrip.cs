using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Matrip.Domain.Models.Entities
{
    public class ma14subtrip
    {
        [Key]
        public int ma14idsubtrip { get; set; }
        [Required]
        public string ma14name { get; set; }
        [Required]
        public string ma14Description { get; set; }
        [Required]
        public int ma14vacancy { get; set; }
        [Required]
        public int ma14minPeopleQuantity { get; set; }
        public int ma14groupnumber { get; set; } = 1;
        [Required]
        public string ma14address { get; set; }
        [Required]
        public string ma14neighborhood { get; set; }
        public int ma14minimumage { get; set; } = 0;
        public double? ma14minweight { get; set; }
        public double? ma14maxweight { get; set; }
        [Required]
        public bool ma14InfluencerDiscount { get; set; }
        [Required]
        public float ma14PartnerDiscountPercent { get; set; }
        public DateTime? ma14InitialDiscountDate { get; set; }
        public DateTime? ma14FinalDiscountDate { get; set; }
        [Required]
        public float ma14MatripCommission { get; set; }
        [Required]
        public string ma14status { get; set; } = "1";
        public string ma14NeedGroup { get; set; } = "0";



        [ForeignKey("ma05trip")]
        public int FK1405idtrip { get; set; }
        public virtual ma05trip ma05trip { get; set; }

        [ForeignKey("ma25partner")]
        public int FK1425idpartner { get; set; }
        public virtual ma25partner ma25partner { get; set; }




        [ForeignKey("FK1714idsubtrip")]
        public virtual ICollection<ma17SubtripValue> ma17SubtripValue { get; set; }
        [ForeignKey("FK1614idsubtrip")]
        public virtual ICollection<ma16subtripschedule> ma16subtripschedule { get; set; }
        [ForeignKey("FK1114idsubtrip")]
        public virtual ICollection<ma11service> ma11service { get; set; }
        [ForeignKey("FK1214idsubtrip")]
        public virtual ICollection<ma12SubtripGuide> ma12SubtripGuide { get; set; }
        [ForeignKey("FK1514idsubtrip")]
        public virtual ICollection<ma15subtripphoto> ma15subtripphoto { get; set; }
        [ForeignKey("FK1914idsubtrip")]
        public virtual ICollection<ma19SubTripItemShoppingCart> ma19SubTripItemShoppingCart { get; set; }



        [ForeignKey("FK2214idSubTrip")]
        public virtual ICollection<ma22subtripsale> ma22subtripsale { get; set; }
        [ForeignKey("FK3614idsubtrip")]
        public virtual ICollection<ma36SubtripGroup> ma36SubtripGroup { get; set; }

    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Matrip.Domain.Models.Entities
{
    public class ma04guide
    {
        [Key]
        public int ma04idguide { get; set; }
        [Required]
        public string ma04MEI { get; set; }



        [ForeignKey("ma01user")]
        public int FK0401iduser { get; set; }
        public virtual ma01user ma01user { get; set; }



        [ForeignKey("FK1204idguide")]
        public virtual ICollection<ma12SubtripGuide> ma12SubtripGuide { get; set; }
        [ForeignKey("FK2604idGuide")]
        public virtual ICollection<ma26PartnerGuide> ma26PartnerGuide { get; set; }
        [ForeignKey("FK3004idguide")]
        public virtual ICollection<ma30GuideSubtripShoppingCart> ma30GuideSubtripShoppingCart { get; set; }
        [ForeignKey("FK3104idGuide")]
        public virtual ICollection<ma31SubtripSaleGuide> ma31SubtripSaleGuide { get; set; }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Matrip.Domain.Models.Entities
{
    public class ma05trip
    {
        [Key]
        public int ma05idtrip { get; set; }
        [Required]
        public string ma05name { get; set; }
        [Required]
        public string ma05description { get; set; }
        [Required]
        public string ma05transfer { get; set; } = "0";
        public string ma05accompanyingchild { get; set; } = "0";
        [Required]
        public string ma05editsubtrip { get; set; } = "0";
        [Required]
        public string ma05status { get; set; } = "1";
        public string ma05featuredtour { get; set; } = "0";
        public string ma05promotiondescription { get; set; } = "";





        [ForeignKey("ma06tripcategory")]
        public int FK0506idtripcategory { get; set; }
        [ForeignKey("ma09city")]
        public int FK0509idcity { get; set; }
        public virtual ma06tripcategory ma06tripcategory { get; set; }
        public virtual ma09city ma09city { get; set; }




        [ForeignKey("FK1405idtrip")]
        public virtual ICollection<ma14subtrip> ma14subtrip { get; set; }
        [ForeignKey("FK2705idTrip")]
        public virtual ICollection<ma27AgeDiscount> ma27AgeDiscount { get; set; }
        [ForeignKey("FK1305idtrip")]
        public virtual ICollection<ma13tripphoto> ma13tripphoto { get; set; }
        [ForeignKey("KF1805idtrip")]
        public virtual ICollection<ma18tripitemshoppingcart> ma18tripitemshoppingcart { get; set; }


        [ForeignKey("FK2105idtrip")]
        public virtual ICollection<ma21saleTrip> ma21saleTrip { get; set; }

        [ForeignKey("FK3905idTrip")]
        public virtual ICollection<ma39tripEvaluation> ma39tripEvaluation { get; set; }
    }
}

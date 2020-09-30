using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Matrip.Domain.Models.Entities
{
    public class ma02profile
    {
        [Key]
        public int ma02idprofile { get; set; }

        [ForeignKey("ma01user")]
        public int FK0201iduser { get; set; }
        [ForeignKey("ma06tripcategory")]
        public int FK0206idtripcategory { get; set; }



        public virtual ma01user ma01user { get; set; }
        public virtual ma06tripcategory ma06tripcategory { get; set; }
    }
}

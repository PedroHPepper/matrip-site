using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Matrip.Domain.Models.Entities
{
    public class ma31SubtripSaleGuide
    {
        [Key]
        public int ma31idSubtripSaleGuide { get; set; }
        [Required]
        [ForeignKey("ma22subtripsale")]
        public int FK3122idSubtripSale { get; set; }
        [Required]
        [ForeignKey("ma04guide")]
        public int FK3104idGuide { get; set; }




        public virtual ma22subtripsale ma22subtripsale { get; set; }
        public virtual ma04guide ma04guide { get; set; }
    }
}

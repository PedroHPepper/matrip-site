using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Matrip.Domain.Models.Entities
{
    public class ma26PartnerGuide
    {
        [Key]
        public int ma26idPartnerGuide { get; set; }
        [Required]
        public string ma26status { get; set; } = "1";

        [Required]
        [ForeignKey("ma25partner")]
        public int FK2625idPartner { get; set; }
        [Required]
        [ForeignKey("ma04guide")]
        public int FK2604idGuide { get; set; }



        public virtual ma25partner ma25partner { get; set; }
        public virtual ma04guide ma04guide { get; set; }
    }
}

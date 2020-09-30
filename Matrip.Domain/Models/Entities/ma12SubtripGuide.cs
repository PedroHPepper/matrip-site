using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Matrip.Domain.Models.Entities
{
    public class ma12SubtripGuide
    {
        [Key]
        public int ma12idSubtripGuide { get; set; }
        [Required]
        [ForeignKey("ma04guide")]
        public int FK1204idguide { get; set; }
        [Required]
        [ForeignKey("ma14subtrip")]
        public int FK1214idsubtrip { get; set; }




        public virtual ma04guide ma04guide { get; set; }
        public virtual ma14subtrip ma14subtrip { get; set; }
    }
}

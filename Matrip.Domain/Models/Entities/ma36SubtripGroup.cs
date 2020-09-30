using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Matrip.Domain.Models.Entities
{
    public class ma36SubtripGroup
    {
        [Key]
        public int ma36idSubtripGroup { get; set; }
        [Required]
        public int ma36vacancy { get; set; }
        [Required]
        public string ma36status { get; set; } = "1";


        [ForeignKey("ma14subtrip")]
        public int FK3614idsubtrip { get; set; }
        public virtual ma14subtrip ma14subtrip { get; set; }




    }
}

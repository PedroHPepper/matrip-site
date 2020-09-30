using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Matrip.Domain.Models.Entities
{
    public class ma16subtripschedule
    {
        [Key]
        public int ma16idsubtripschedule { get; set; }
        [Required]
        public TimeSpan ma16duration { get; set; }
        [Required]
        public TimeSpan ma16entry { get; set; }
        [Required]
        public TimeSpan ma16exit { get; set; }
        [Required]
        public string ma16days { get; set; }
        [Required]
        public string ma16status { get; set; } = "1";




        [ForeignKey("ma14subtrip")]
        public int FK1614idsubtrip { get; set; }
        public virtual ma14subtrip ma14subtrip { get; set; }
    }
}

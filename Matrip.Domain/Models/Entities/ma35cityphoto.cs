using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Matrip.Domain.Models.Entities
{
    public class ma35cityphoto
    {
        [Key]
        public int ma35idcityphoto { get; set; }
        [Required]
        public DateTime ma35versionDate { get; set; }



        [ForeignKey("ma09city")]
        public int FK3509idcity { get; set; }
        public virtual ma09city ma09city { get; set; }
    }
}

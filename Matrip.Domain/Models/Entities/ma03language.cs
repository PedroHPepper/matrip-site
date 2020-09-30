using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Matrip.Domain.Models.Entities
{
    public class ma03language
    {
        [Key]
        public int ma03idlanguage { get; set; }
        [Required]
        public string ma03name { get; set; }


        [ForeignKey("FK1003idlanguage")]
        public virtual ICollection<ma10userlanguages> ma10userlanguages { get; set; }
    }
}

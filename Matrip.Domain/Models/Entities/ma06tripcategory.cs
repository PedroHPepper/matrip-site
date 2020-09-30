using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Matrip.Domain.Models.Entities
{
    public class ma06tripcategory
    {
        [Key]
        public int ma06idtripcategory { get; set; }
        [Required]
        public string ma06name { get; set; }


        [ForeignKey("FK0206idtripcategory")]
        public virtual ICollection<ma02profile> ma02profile { get; set; }
        [ForeignKey("FK0506idtripcategory")]
        public virtual ICollection<ma05trip> ma05trip { get; set; }
    }
}

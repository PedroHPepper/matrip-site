using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Matrip.Domain.Models.Entities
{
    public class ma09city
    {
        [Key]
        public int ma09idcity { get; set; }
        [Required]
        public string ma09name { get; set; }
        public string ma09Description { get; set; } = "";

        [Required]
        [ForeignKey("ma08uf")]
        public int FK0908iduf { get; set; }
        public virtual ma08uf ma08uf { get; set; }



        [ForeignKey("FK0509idcity")]
        public virtual ICollection<ma05trip> ma05trip { get; set; }
        [ForeignKey("FK2509idcity")]
        public virtual ICollection<ma25partner> ma25partner { get; set; }
        [ForeignKey("FK3509idcity")]
        public virtual ICollection<ma35cityphoto> ma35cityphoto { get; set; }
    }
}

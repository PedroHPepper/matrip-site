using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Matrip.Domain.Models.Entities
{
    public class ma08uf
    {
        [Key]
        public int ma08iduf { get; set; }
        [Required]
        public string ma08name { get; set; }
        public string ma08UFInitials { get; set; }


        [Required]
        [ForeignKey("ma07country")]
        public int FK0807idcountry { get; set; }
        public virtual ma07country ma07country { get; set; }



        [ForeignKey("FK0908iduf")]
        public virtual ICollection<ma09city> ma09city { get; set; }

    }
}

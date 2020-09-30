using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Matrip.Domain.Models.Entities
{
    public class ma07country
    {
        [Key]
        public int ma07idcountry { get; set; }
        [Required]
        public string ma07name { get; set; }


        [ForeignKey("FK0807idcountry")]
        public virtual ICollection<ma08uf> ma05uf { get; set; }
    }
}

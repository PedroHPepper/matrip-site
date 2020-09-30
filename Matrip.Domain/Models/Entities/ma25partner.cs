using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Matrip.Domain.Models.Entities
{
    public class ma25partner
    {
        [Key]
        public int ma25idpartner { get; set; }
        [Required]
        public string ma25name { get; set; }
        public string ma25companyName { get; set; } = "";
        [Required]
        public string ma25CNPJ { get; set; }
        public string ma25homepage { get; set; }
        public string ma25email { get; set; }
        [Required]
        public string ma25address { get; set; }
        [Required]
        public string ma25neighborhood { get; set; }
        public string ma25status { get; set; } = "1";



        [Required]
        [ForeignKey("ma09city")]
        public int FK2509idcity { get; set; }
        public virtual ma09city ma09city { get; set; }





        [ForeignKey("FK2625idPartner")]
        public virtual ICollection<ma26PartnerGuide> ma26PartnerGuide { get; set; }
        [ForeignKey("FK1425idpartner")]
        public virtual ICollection<ma14subtrip> ma14subtrip { get; set; }
    }
}

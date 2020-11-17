using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Matrip.Domain.Models.Entities
{
    public class ma01user : IdentityUser<int>
    {
        [Required]
        public string ma01FullName { get; set; }
        [Required]
        public DateTime ma01birth { get; set; }
        public string ma01type { get; set; } = "user";




        [ForeignKey("FK3301iduser")]
        public virtual ma33UserAddress ma33UserAddress { get; set; }
        [ForeignKey("FK3201iduser")]
        public virtual ICollection<ma32sale> ma32sale { get; set; }
        [ForeignKey("FK1801iduser")]
        public virtual ICollection<ma18tripitemshoppingcart> ma18tripitemshoppingcart { get; set; }
        [ForeignKey("FK0401iduser")]
        public virtual ma04guide ma04guide { get; set; }
        [ForeignKey("FK1001iduser")]
        public virtual ICollection<ma10userlanguages> ma10userlanguages { get; set; }
        [ForeignKey("FK0201iduser")]
        public virtual ICollection<ma02profile> ma02profile { get; set; }
        [ForeignKey("FK3901idUser")]
        public virtual ICollection<ma39tripEvaluation> ma39tripEvaluation { get; set; }
    }
}

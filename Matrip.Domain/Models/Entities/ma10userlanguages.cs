using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Matrip.Domain.Models.Entities
{
    public class ma10userlanguages
    {
        [Key]
        public int ma10iduserlanguages { get; set; }



        [ForeignKey("ma03language")]
        public int FK1003idlanguage { get; set; }
        [ForeignKey("ma01user")]
        public int FK1001iduser { get; set; }



        public virtual ma03language ma03language { get; set; }
        public virtual ma01user ma01user { get; set; }
    }
}

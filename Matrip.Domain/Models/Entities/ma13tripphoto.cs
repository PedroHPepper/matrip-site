using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Matrip.Domain.Models.Entities
{
    public class ma13tripphoto
    {
        [Key]
        public int ma13idtripphoto { get; set; }
        [Required]
        public int ma13photoquantity { get; set; }
        [Required]
        public DateTime ma13versionDate { get; set; }

        [ForeignKey("ma05trip")]
        public int FK1305idtrip { get; set; }
        public virtual ma05trip ma05trip { get; set; }
    }
}

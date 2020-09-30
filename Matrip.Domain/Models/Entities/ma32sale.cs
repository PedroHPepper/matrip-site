using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Matrip.Domain.Models.Entities
{
    public class ma32sale
    {
        [Key]
        public int ma32idSale { get; set; }
        [Required]
        public DateTime ma32SaleDate { get; set; }
        [Required]
        public int ma32situation { get; set; }
        [Required]
        public string ma32terminate { get; set; } = "0";






        [Required]
        [ForeignKey("ma01user")]
        public int FK3201iduser { get; set; }
        public virtual ma01user ma01user { get; set; }



        [ForeignKey("FK2132idSale")]
        public ICollection<ma21saleTrip> ma21saleTrip { get; set; }
        [ForeignKey("FK2432idSale")]
        public ICollection<ma24payment> ma24payment { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Matrip.Domain.Models.Entities
{
    public class ma39tripEvaluation
    {
        [Key]
        public int ma39idTripEvaluation { get; set; }
        public string ma39UserName { get; set; }
        public string ma39Feedback { get; set; }
        public string ma39photoPath { get; set; }
        public string ma39FeedbackAproved { get; set; } = "0";
        public int ma39photoQuantity { get; set; } = 0;



        [ForeignKey("ma05trip")]
        public int FK3905idTrip { get; set; }
        public virtual ma05trip ma05trip { get; set; }

        [ForeignKey("ma01user")]
        public int FK3901idUser { get; set; }
        public virtual ma01user ma01user { get; set; }

        [ForeignKey("ma21saleTrip")]
        public int FK3921idSaleTrip { get; set; }
        public virtual ma21saleTrip ma21saleTrip { get; set; }
    }
}

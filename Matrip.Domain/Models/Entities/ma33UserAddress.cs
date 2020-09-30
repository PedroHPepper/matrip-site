using Matrip.Domain.Libraries.Lang;
using Matrip.Domain.Libraries.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Matrip.Domain.Models.Entities
{
    public class ma33UserAddress
    {
        [Key]
        public int ma33idUserAddress { get; set; }
        [Required]
        public string ma33Zipcode { get; set; }
        [Required]
        public string ma33Country { get; set; }
        [Required]
        public string ma33State { get; set; }
        [Required]
        public string ma33City { get; set; }
        [Required]
        public string ma33Neighborhood { get; set; }
        [Required]
        public string ma33Street { get; set; }
        [Required]
        public string ma33StreetNumber { get; set; }
        public string ma33Complement { get; set; }

        public string ma33CPF { get; set; }
        public string ma33documentNumber { get; set; }
        public string ma33DocumentIssuingBody { get; set; }
        public string ma33DocumentUF { get; set; }



        [ForeignKey("ma01user")]
        public int FK3301iduser { get; set; }

        public virtual ma01user ma01user { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Matrip.Domain.Models.Entities
{
    public class ma34TransferencePendencies
    {
        [Key]
        public int ma34idTransferencePendencies { get; set; }

        [Required]
        public double ma34TransferenceValue { get; set; }
        [Required]
        public int ma34idUser { get; set; }
    }
}

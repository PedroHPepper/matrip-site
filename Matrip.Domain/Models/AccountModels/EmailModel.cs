using System.ComponentModel.DataAnnotations;

namespace Matrip.Domain.Models.AccountModels
{
    public class EmailModel
    {
        [Required]
        public string Email { get; set; }
    }
}

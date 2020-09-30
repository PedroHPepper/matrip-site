using System.ComponentModel.DataAnnotations;

namespace Matrip.Domain.Models.AccountModels
{
    public class EmailPassword
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

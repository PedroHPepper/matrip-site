using System.ComponentModel.DataAnnotations;

namespace Matrip.Domain.Models.AccountModels
{
    public class EmailConfirmationToken
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public string ConfirmationToken { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}

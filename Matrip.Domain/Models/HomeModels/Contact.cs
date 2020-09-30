using System.ComponentModel.DataAnnotations;

namespace Matrip.Domain.Models.HomeModels
{
    public class Contact
    {
        //Essa classe é usada para envio de email através do formulário de contato
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string About { get; set; }
        [Required]
        public string Text { get; set; }
    }
}

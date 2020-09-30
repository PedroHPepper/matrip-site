using System;
using System.ComponentModel.DataAnnotations;

namespace Matrip.Domain.Models.AccountModels
{
    public class UserRegistration
    {
        public string ma01FullName { get; set; }
        [Required]
        public string ma01Email { get; set; }
        public string Password { get; set; }

        [Required]
        public string ma01PhoneNumber { get; set; }
        [Required]
        public DateTime ma01birth { get; set; }
        public string ma01cpf { get; set; }
        public string ma01documentNumber { get; set; }
        public string ma01documentUF { get; set; }




    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace Matrip.Domain.Models.AccountModels
{
    public class UserConfigurationModel
    {
        [Required]
        public string ma01FullName { get; set; }
        [Required]
        public string ma01Email { get; set; }
        [Required]
        public string ma01PhoneNumber { get; set; }
        [Required]
        public DateTime ma01birth { get; set; }
    }
}

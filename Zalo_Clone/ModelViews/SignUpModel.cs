using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zalo_Clone.Models
{
    public class SignUpModel
    {

        [Required]
        public string? UserName { get; set; }
        [EmailAddress]
 
        public string? Email { get; set; } 
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }
        [MaxLength(5)]
        public string? Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

   


    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zalo_Clone.Models
{
    public class SignUpModel
    {
        [Required]
        public string UserName { get; set; }
 
        public string Email { get; set; } 
        public string Password { get; set; }
        public int Type { get; set; }
        [Required]
        public bool IsActivated { get; set; }
        public string? RestoreMail { get; set; }
        public string? PhoneNunber { get; set; }
        [MaxLength(3)]
        public string Gender { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }

    }
}

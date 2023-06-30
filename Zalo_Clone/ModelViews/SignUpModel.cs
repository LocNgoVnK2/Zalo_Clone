using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zalo_Clone.Models
{
    public class SignUpModel
    {
        public string? Id { get; set; }
   
        public bool EmailConfirmed { get; set; } = false;

        [Required]
        public string? UserName { get; set; }
 
        public string? Email { get; set; } 
        public string? Password { get; set; }
        public int Type { get; set; }
        public string? PhoneNumber { get; set; }
        [MaxLength(5)]
        public string? Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public bool? IsActivated { get; set; }
    
        public string? RestoreMail { get; set; }


    }
}

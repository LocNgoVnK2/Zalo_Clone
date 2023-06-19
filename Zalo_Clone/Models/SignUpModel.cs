using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zalo_Clone.Models
{
    public class SignUpModel
    {
        [Required]
        [NotMapped]
        public string UserName { get; set; }
 
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public int Type { get; set; }
        [Required]
        public bool IsConfirmed { get; set; }
        [Required]
        public bool IsActivated { get; set; }
        [Required]
        public string RestoreMail { get; set; } = null!;
        [Required]
        public string PhoneNunber { get; set; } = null!;
    }
}

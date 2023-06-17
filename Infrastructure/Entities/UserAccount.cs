using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Entities
{
    //[Table("USER_ACCOUNT")]
    public class UserAccount : IdentityUser
    {
       
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public string Password { get; set; }
        public int Type { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsActivated { get; set; }
        public string RestoreMail { get; set; }
       
    }
}

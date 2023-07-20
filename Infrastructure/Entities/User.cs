
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    [Table("APP_USER")]
    public class User 
    {
        [Key]
        public string? Id { get; set; }
        public string? UserName { get; set; }

        public string? Password { get; set; }
        public string? Email { get; set; }
        public bool? EmailConfirmed { get; set; } = false;

        public bool? IsActivated { get; set; }
        public string? RestoreMail { get; set; }

        public string? PhoneNumber { get; set; }
        public string? ValidationCode { get; set; }

    }
    
}
